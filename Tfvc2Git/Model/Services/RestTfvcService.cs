using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Polly;
using Tfvc2Git.Extensions;
using Tfvc2Git.Model.Services.Model;

namespace Tfvc2Git.Model.Services
{
    public class RestTfvcService : ITfvcService
    {
        private static readonly Random Jitterer = new Random();

        private static readonly HttpStatusCode[] HttpStatusCodesWorthRetrying =
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.TooManyRequests, // 429
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };

        private readonly HttpClient _httpClient;
        private readonly HttpClient _fileHttpClient;
        private readonly Uri _baseUri;

        public RestTfvcService(Uri baseUri, string personalAccessToken)
        {
            _baseUri = baseUri;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", personalAccessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _fileHttpClient = new HttpClient() {Timeout = TimeSpan.FromMinutes(30)};
            _fileHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", personalAccessToken);
        }

        public async Task<ArrayResponse<TfvcChangeSetRef>> GetBatchedChangeSets(
            string organization,
            BatchedChangeSetRequest request,
            CancellationToken token)
        {
            var uri = new Uri(_baseUri, $"/{organization}/_apis/tfvc/changesetsbatch?api-version=5.1");

            HttpRequestMessage MessageGenerator()
            {
                var content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(request));
                return new HttpRequestMessage(HttpMethod.Post, uri) {Content = content};
            }

            return await RetryHttpWithBodyValue<ArrayResponse<TfvcChangeSetRef>>(_httpClient, MessageGenerator, token);
        }

        public async Task<ArrayResponse<TfvcChangeSetRef>> GetChangeSets(
            string organization,
            string project,
            GetChangeSetsParams parameters,
            CancellationToken token)
        {
            var builder = new UriBuilder(_baseUri + $"/{organization}/{project}/_apis/tfvc/changesets");

            var query = HttpUtility.ParseQueryString("?api-version=5.1");
            ConvertParamsToQuery(parameters, query);

            builder.Query = query.ToString();

            var uri = builder.Uri;
            return await RetryHttpWithBodyValue<ArrayResponse<TfvcChangeSetRef>>(
                _httpClient,
                () => new HttpRequestMessage(HttpMethod.Get, uri),
                token);
        }

        public async Task<ArrayResponse<TfvcChange>> GetChangeSetChanges(
            string organization,
            int id,
            int? skip = null,
            int? top = null,
            CancellationToken token = default)
        {
            var builder = new UriBuilder(_baseUri + $"/{organization}/_apis/tfvc/changesets/{id}/changes");

            var query = HttpUtility.ParseQueryString("?api-version=5.1");

            if (skip != null)
            {
                query.Add("$skip", skip.ToString());
            }

            if (top != null)
            {
                query.Add("$top", top.ToString());
            }

            builder.Query = query.ToString();

            var uri = builder.Uri;
            return await RetryHttpWithBodyValue<ArrayResponse<TfvcChange>>(
                _httpClient,
                () => new HttpRequestMessage(HttpMethod.Get, uri),
                token);
        }

        public async Task<TfvcChangeSet> GetChangeSet(
            string organization,
            int id,
            string project,
            GetChangeSetParams parameters,
            CancellationToken token)
        {
            var builder = new UriBuilder(_baseUri + $"/{organization}/{project}/_apis/tfvc/changesets/{id}");

            var query = HttpUtility.ParseQueryString("?api-version=5.1");

            ConvertParamsToQuery(parameters, query);

            builder.Query = query.ToString();

            var uri = builder.Uri;
            return await RetryHttpWithBodyValue<TfvcChangeSet>(
                _httpClient,
                () => new HttpRequestMessage(HttpMethod.Get, uri),
                token);
        }

        public async Task<TfvcItem> GetItem(string organization, string project, string path, GetItemParams parameters,
            CancellationToken token)
        {
            var builder = new UriBuilder(_baseUri + $"/{organization}/{project}/_apis/tfvc/items");

            var query = HttpUtility.ParseQueryString("?api-version=5.1");
            query["path"] = path;

            ConvertParamsToQuery(parameters, query);

            builder.Query = query.ToString();

            var uri = builder.Uri;
            return await RetryHttpWithBodyValue<TfvcItem>(
                _httpClient,
                () => new HttpRequestMessage(HttpMethod.Get, uri),
                token);
        }

        public async Task DownloadBatchItems(string organization, string project, string targetPath, GetItemsBatchRequest batchRequest, CancellationToken token)
        {
            var builder = new UriBuilder(_baseUri + $"{organization}/{project}/_apis/tfvc/itembatch");

            var query = HttpUtility.ParseQueryString("?api-version=5.1");
            
            builder.Query = query.ToString();
            
            var uri = builder.Uri;
            HttpRequestMessage MessageGenerator()
            {
                var request = new HttpRequestMessage() {Method = HttpMethod.Post, RequestUri = uri};
                var jsonSettings = new JsonSerializerOptions() { IgnoreNullValues = true,  };
                var content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(batchRequest, jsonSettings));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request.Content = content;
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));
                return request;
            }

            var response = await RetryHttpWithResponse(_fileHttpClient, MessageGenerator, token);

            response.EnsureSuccessStatusCode();

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            await ReadExtractZipFileStream(project, targetPath, token, response);
        }
        
        public async Task RefreshRepoAndDownloadItemsForChangeSet(
            string organization,
            string project,
            string targetPath,
            GetItemParams itemParameters,
            CancellationToken token)
        {
            var builder = new UriBuilder(_baseUri + $"/{organization}/{project}/_apis/tfvc/items");

            var query = HttpUtility.ParseQueryString("?api-version=5.1");

            ConvertParamsToQuery(itemParameters, query);

            builder.Query = query.ToString();

            var uri = builder.Uri;
            var response = await RetryHttpWithResponse(
                _httpClient,
                () => new HttpRequestMessage(HttpMethod.Get, uri),
                token);
            
            response.EnsureSuccessStatusCode();

            DeleteFilesApartFromGit(targetPath);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            await ReadExtractZipFileStream(project, targetPath, token, response);
        }

        private static void DeleteFilesApartFromGit(string targetPath)
        {
            var ignoreFiles =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase) {".gitignore", ".gitattributes", ".git"};

            foreach (var file in Directory.EnumerateFiles(
                targetPath,
                "*.*",
                new EnumerationOptions() {RecurseSubdirectories = true, IgnoreInaccessible = true}))
            {
                if (ignoreFiles.Contains(Path.GetFileName(file)))
                {
                    continue;
                }

                File.Delete(file);
            }
        }

        private static async Task ReadExtractZipFileStream(string project, string targetPath, CancellationToken token,
            HttpResponseMessage response)
        {
            var downloadPath = Path.GetTempFileName();

            try
            {
                {
                    await using var streamToWriteTo = File.Open(downloadPath, FileMode.Create);
                    await using var stream = await response.Content.ReadAsStreamAsync();
                    await stream.CopyToAsync(streamToWriteTo, 4096, token);
                }

                ExtractZipFile(project, targetPath, downloadPath);
            }
            finally
            {
                File.Delete(downloadPath);
            }
        }
        private static void ExtractZipFile(string project, string targetPath, string downloadPath)
        {
            var zipFile = ZipFile.OpenRead(downloadPath);

            foreach (var entry in zipFile.Entries)
            {
                var strippedPath = entry.FullName.Replace($"$/{project}/", "");

                var targetFilePath = Path.Combine(targetPath, strippedPath);
                targetFilePath.CreateFileDirectory();
                entry.ExtractToFile(targetFilePath, true);
            }
        }

        private static void ConvertParamsToQuery<T>(T parameters, NameValueCollection query)
            where T : class
        {
            foreach (var property in parameters.GetType().GetProperties())
            {
                var attribute = property.GetCustomAttribute(typeof(AliasAsAttribute)) as AliasAsAttribute;

                var name = attribute?.Name ?? property.Name;

                var value = Convert.ToString(property.GetValue(parameters));

                if (!string.IsNullOrWhiteSpace(value))
                {
                    query.Add(name, value);
                }
            }
        }

        private static async Task<T> RetryHttpWithBodyValue<T>(HttpClient client,
            Func<HttpRequestMessage> requestGenerator, CancellationToken token)
        {
            var response = await RetryHttpWithResponse(client, requestGenerator, token);

            response.EnsureSuccessStatusCode();

            return await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(),
                cancellationToken: token);
        }

        private static Task<HttpResponseMessage> RetryHttpWithResponse(HttpClient client,
            Func<HttpRequestMessage> requestGenerator, CancellationToken token)
        {
            var maxDelay = TimeSpan.FromSeconds(2);
            var seedDelay = TimeSpan.FromMilliseconds(100);
            var maxRetries = 10;

            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(x => HttpStatusCodesWorthRetrying.Contains(x.StatusCode))
                .WaitAndRetryAsync(DecorrelatedJitter(maxRetries, seedDelay, maxDelay));

            return policy.ExecuteAsync(() => client.SendAsync(requestGenerator.Invoke(), token));
        }

        private static IEnumerable<TimeSpan> DecorrelatedJitter(int maxRetries, TimeSpan seedDelay, TimeSpan maxDelay)
        {
            var retries = 0;

            var seed = seedDelay.TotalMilliseconds;
            var max = maxDelay.TotalMilliseconds;
            var current = seed;

            while (++retries <= maxRetries)
            {
                current = Math.Min(max,
                    Math.Max(seed,
                        current * 3 *
                        Jitterer
                            .NextDouble())); // adopting the 'Decorrelated Jitter' formula from https://www.awsarchitectureblog.com/2015/03/backoff.html.  Can be between seedMs and previousMs * 3.  Mustn't exceed maxDelay.
                yield return TimeSpan.FromMilliseconds(current);
            }
        }
    }
}
