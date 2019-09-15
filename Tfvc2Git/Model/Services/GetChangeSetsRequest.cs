using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services
{
    public class GetChangeSetsRequest
    {
        [JsonPropertyName("exclude")]
        public bool? Exclude { get; set; }
        
        [JsonPropertyName("serverPath")]
        public string ServerPath { get; set; }
    }
}
