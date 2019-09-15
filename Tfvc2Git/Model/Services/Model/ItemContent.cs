using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class ItemContent
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
        
        [JsonPropertyName("contentType")]
        public ItemContentType ContentType { get; set; }
    }
}
