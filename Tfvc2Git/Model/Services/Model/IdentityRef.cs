using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class IdentityRef
    {
        [JsonPropertyName("descriptor")]
        public string Descriptor { get; set; }
        [JsonPropertyName("directoryAlias")]
        public string DirectoryAlias { get; set; }
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonPropertyName("inactive")]
        public bool Inactive { get; set; }
        [JsonPropertyName("isAadIdentity")]
        public bool IsAadIdentity { get; set; }
        [JsonPropertyName("isContainer")]
        public bool IsContainer { get; set; }
        [JsonPropertyName("isDeletedInOrigin")]
        public bool IsDeletedInOrigin { get; set; }
        [JsonPropertyName("profileUrl")]
        public string ProfileUrl { get; set; }
        [JsonPropertyName("uniqueName")]
        public string UniqueName { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
