using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcPolicyOverrideInfo
    {
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        
        [JsonPropertyName("policyFailures")]
        public List<TfvcPolicyFailureInfo> PolicyFailures { get; set; }
    }
}
