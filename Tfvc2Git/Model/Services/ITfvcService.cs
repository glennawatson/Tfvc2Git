using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tfvc2Git.Model.Services.Model;

namespace Tfvc2Git.Model.Services
{
    public interface ITfvcService
    {
        Task<ArrayResponse<TfvcChangeSetRef>> GetBatchedChangeSets(string organization, BatchedChangeSetRequest request, CancellationToken token);
        
        Task<ArrayResponse<TfvcChangeSetRef>> GetChangeSets(string organization, string project, GetChangeSetsParams parameters, CancellationToken token);
        
        Task<ArrayResponse<TfvcChange>> GetChangeSetChanges(string organization, int id, [AliasAs("$skip")]int? skip = null, [AliasAs("$top")]int? top = null, CancellationToken token = default);

        Task<TfvcChangeSet> GetChangeSet(string organization, int id, string project, GetChangeSetParams parameters, CancellationToken token);

        Task<TfvcItem> GetItem(string organization, string project, string path, GetItemParams parameters, CancellationToken token);

        Task DownloadBatchItems(string organization, string project, string targetPath, GetItemsBatchRequest request, CancellationToken token);

        Task RefreshRepoAndDownloadItemsForChangeSet(string organization, string project, string targetPath, GetItemParams itemParameters, CancellationToken token);
    }
}
