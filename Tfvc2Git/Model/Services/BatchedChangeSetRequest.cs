using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services
{
    public class BatchedChangeSetRequest
    {
        [JsonPropertyName("changesetIds")]
        public List<int> ChangeSetIds { get; set; }
        
        [JsonPropertyName("commentLength")]
        public int CommentLength { get; set; }
        
        [JsonPropertyName("includeLinks")]
        public bool IncludeLinks { get; set; }
    }
}
