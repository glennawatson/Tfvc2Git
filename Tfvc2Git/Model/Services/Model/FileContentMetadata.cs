using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class FileContentMetadata
    {
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }	
        [JsonPropertyName("encoding")]
        public int Encoding { get; set; }	
        [JsonPropertyName("extension")]
        public string Extension { get; set; }	
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }	
        [JsonPropertyName("isBinary")]
        public bool IsBinary { get; set; }	
        [JsonPropertyName("isImage")]
        public bool IsImage { get; set; }	
        [JsonPropertyName("vsLink")]
        public string VsLink { get; set; }	
    }
}
