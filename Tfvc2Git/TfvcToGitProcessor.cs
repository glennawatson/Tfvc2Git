using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tfvc2Git.Extensions;
using Tfvc2Git.Model.Services;
using Tfvc2Git.Model.Services.Model;

namespace Tfvc2Git
{
    public class TfvcToGitProcessor
    {
        private readonly ITfvcService _tfvcApi;
        private readonly Options _options;

        public TfvcToGitProcessor(Options options)
        {
            _options = options;

            _tfvcApi = new RestTfvcService(
                new Uri(options.SourceUri),
                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(":" + options.AccessToken)));
        }

        public async Task Process(CancellationToken token)
        {
            if (_options.StartFresh || !Directory.Exists(_options.TargetDirectory))
            {
                await InitGitRepo();
            }

            const int pageSize = 50;

            var changeSets = GetAll(page => _tfvcApi.GetChangeSets(
                _options.OrgName,
                _options.ProjectName,
                new GetChangeSetsParams()
                {
                    Skip = pageSize * page,
                    Top = pageSize,
                    OrderBy = "id asc",
                    MaxCommentLength = 4096,
                    FromId = _options.FromChangeSet
                },
                default));

            await foreach (var changeSet in changeSets.WithCancellation(token))
            {
                Console.WriteLine($"Processing change set {changeSet.ChangesetId}");
                File.Copy(_options.GitAttributesTemplatePath, Path.Combine(_options.TargetDirectory, ".gitattributes"),
                    true);
                File.Copy(_options.GitIgnoreTemplatePath, Path.Combine(_options.TargetDirectory, ".gitignore"), true);


                var changes = GetAll(page =>
                    _tfvcApi.GetChangeSetChanges(
                        _options.OrgName,
                        changeSet.ChangesetId,
                        pageSize * page,
                        pageSize,
                        token));

                await ProcessChanges(changeSet, changes, token);

                var commitMessagePath = Path.GetTempFileName();
                File.WriteAllText(commitMessagePath,
                    changeSet.Comment +
                    $"{Environment.NewLine}{Environment.NewLine}From TFVC change set: {changeSet.ChangesetId}");

                var environmentVariables =
                    new Dictionary<string, string>
                    {
                        ["GIT_AUTHOR_DATE"] = changeSet.CreatedDate.ToString("O"),
                        ["GIT_COMMITTER_DATE"] = changeSet.CreatedDate.ToString("O"),
                        ["GIT_AUTHOR_NAME"] = changeSet.Author.DisplayName,
                        ["GIT_COMMITTER_NAME"] = changeSet.Author.DisplayName,
                        ["GIT_AUTHOR_EMAIL"] = changeSet.Author.UniqueName,
                        ["GIT_COMMITTER_EMAIL"] = changeSet.Author.UniqueName
                    };

                await ExecuteGit(_options.TargetDirectory, "add .");
                await ExecuteGit(_options.TargetDirectory, environmentVariables, "commit", "-F", commitMessagePath,
                    "--no-gpg-sign");
            }
        }

        private async Task InitGitRepo()
        {
            if (Directory.Exists(_options.TargetDirectory))
            {
                Directory.Delete(_options.TargetDirectory, true);
            }

            Directory.CreateDirectory(_options.TargetDirectory);

            await ExecuteGit(_options.TargetDirectory, "init");
        }

        private async Task ProcessChanges(TfvcChangeSetRef changeSet, IAsyncEnumerable<TfvcChange> changes, CancellationToken token)
        {
            var fileChanges = new List<TfvcChange>(1024);
            await foreach (var change in changes.WithCancellation(token))
            {
                var itemPath = GetFileSystemPath(change.Item.Path);

                if (change.Item.IsBranch || change.Item.IsSymLink || change.Item.IsPendingChange)
                {
                    continue;
                }

                try
                {
                    if (change.ChangeType.HasFlag(VersionControlChangeType.Delete))
                    {
                        if (change.Item.IsFolder)
                        {
                            if (Directory.Exists(itemPath))
                            {
                                Directory.Delete(itemPath, true);
                            }
                        }
                        else
                        {
                            if (File.Exists(itemPath))
                            {
                                File.Delete(itemPath);
                            }
                        }
                    }
                    else if (change.ChangeType.HasFlag(VersionControlChangeType.Rename))
                    {
                        var oldPath = GetFileSystemPath(change.SourceServerItem);

                        if (change.Item.IsFolder)
                        {
                            itemPath.CreateFileDirectory();
                            Directory.Move(oldPath, itemPath);
                        }
                        else
                        {
                            itemPath.CreateFileDirectory();

                            File.Move(oldPath, itemPath, true);
                        }
                    }
                    else if (change.ChangeType.HasFlag(VersionControlChangeType.Edit) ||
                             change.ChangeType.HasFlag(VersionControlChangeType.Add) ||
                             change.ChangeType.HasFlag(VersionControlChangeType.Encoding) ||
                             change.ChangeType.HasFlag(VersionControlChangeType.Undelete) ||
                             change.ChangeType.HasFlag(VersionControlChangeType.Rollback) ||
                             change.ChangeType.HasFlag(VersionControlChangeType.Merge))
                    {
                        if (change.Item.IsFolder)
                        {
                            if (!Directory.Exists(itemPath))
                            {
                                Directory.CreateDirectory(itemPath);
                            }
                        }
                        else
                        {
                            fileChanges.Add(change);
                        }
                    }

//                    else if ()
//                    {
//                    }
//                    else if (change.ChangeType.HasFlag(VersionControlChangeType.Lock))
//                    {
//                    }
//                    else if ())
//                    {
//                    }
//                    else if (change.ChangeType.HasFlag(VersionControlChangeType.None))
//                    {
//                    }
//                    else if (change.ChangeType.HasFlag(VersionControlChangeType.Property))
//                    {
//                    }
//                    else if ()
//                    {
//                    }
//                    else if (change.ChangeType.HasFlag(VersionControlChangeType.SourceRename))
//                    {
//                    }
//                    else if (change.ChangeType.HasFlag(VersionControlChangeType.TargetRename))
//                    {
//                    }
//                    else if ()
//                    {
//                    }
//                    else
//                    {
//                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


            if (fileChanges.Count == 0)
            {
                return;
            }

            var descriptors = fileChanges.Select(x => new TfvcItemDescriptor()
            {
                Path = x.Item.Path,
                Version = x.Item.Version.ToString(),
                VersionOption = TfvcVersionOption.none,
                VersionType = TfvcVersionType.changeset
            });

            try
            {
                await DownloadItemsBatch(token, descriptors);
            }
            catch (Exception e)
            {
                try
                {
                    await DownloadReplaceChangeSet(changeSet, token);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        private async Task DownloadReplaceChangeSet(TfvcChangeSetRef changeSet, CancellationToken token)
        {
            await _tfvcApi.RefreshRepoAndDownloadItemsForChangeSet(
                _options.OrgName,
                _options.ProjectName,
                _options.TargetDirectory,
                new GetItemParams()
                {
                    VersionType = TfvcVersionType.changeset,
                    Version = changeSet.ChangesetId.ToString()
                },
                token);
        }

        private async Task DownloadItemsBatch(CancellationToken token, IEnumerable<TfvcItemDescriptor> descriptors)
        {
            var getItemsBatchRequest = new GetItemsBatchRequest() {ItemDescriptors = descriptors.ToList()};
            await _tfvcApi.DownloadBatchItems(
                _options.OrgName,
                _options.ProjectName,
                _options.TargetDirectory,
                getItemsBatchRequest, token);
        }

        private string GetFileSystemPath(string itemCurrentPath)
        {
            var strippedPath = itemCurrentPath.Replace($"$/{_options.ProjectName}", "").TrimStart('/');
            var itemPath = Path.Combine(_options.TargetDirectory, strippedPath);
            return itemPath;
        }

        private static Task ExecuteGit(string targetDirectory, params string[] arguments)
        {
            return ExecuteGit(targetDirectory, new Dictionary<string, string>(), arguments);
        }

        private static Task ExecuteGit(string targetDirectory, IDictionary<string, string> environment,
            params string[] arguments)
        {
            return Task.Run(() =>
            {
                using var process = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        FileName = "git",
                        CreateNoWindow = true,
                        WorkingDirectory = targetDirectory,
                        Arguments = string.Join(" ", arguments)
                    }
                };

                foreach (var value in environment)
                {
                    process.StartInfo.EnvironmentVariables.Add(value.Key, value.Value);
                }

                process.Start();
                process.WaitForExit(); // Will wait indefinitely until the process exits
            });
        }

        private static IAsyncEnumerable<T> GetAll<T>(Func<int, Task<ArrayResponse<T>>> resultFunc)
        {
            return GetAll(resultFunc, x => true);
        }

        private static async IAsyncEnumerable<T> GetAll<T>(
            Func<int, Task<ArrayResponse<T>>> resultFunc,
            Func<T, bool> includeFilter)
        {
            var page = 0;

            bool needsMore;

            do
            {
                var changeSets = await resultFunc(page);


                foreach (var changeSet in changeSets.Values.Where(includeFilter))
                {
                    yield return changeSet;
                }

                page++;

                needsMore = changeSets.Count != 0;
            } while (needsMore);
        }
    }
}
