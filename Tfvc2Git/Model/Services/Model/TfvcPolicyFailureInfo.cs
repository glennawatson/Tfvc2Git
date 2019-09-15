using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcPolicyFailureInfo
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        
        [JsonPropertyName("policyName")]
        public string PolicyName { get; set; }
    }
}
