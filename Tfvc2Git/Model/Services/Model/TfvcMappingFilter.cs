using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcMappingFilter
    {
        [JsonPropertyName("exclude")]
        public bool Exclude { get; set; }
        
        [JsonPropertyName("serverPath")]
        public string ServerPath { get; set; }
    }
}
