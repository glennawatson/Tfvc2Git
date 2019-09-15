using System.Collections.Generic;
using System.Text.Json.Serialization;
using Tfvc2Git.Model.Services.Model;

namespace Tfvc2Git.Model.Services
{
    public class GetItemsBatchRequest
    {
        [JsonPropertyName("includeContentMetadata")]
        public bool? IncludeContentMetadata { get; set; }
        
        [JsonPropertyName("includeLinks")]
        public bool? IncludeLinks { get; set; }
        
        [JsonPropertyName("itemDescriptors")]
        public List<TfvcItemDescriptor> ItemDescriptors { get; set; }
    }
}
