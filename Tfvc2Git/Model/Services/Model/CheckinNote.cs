using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class CheckinNote
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
