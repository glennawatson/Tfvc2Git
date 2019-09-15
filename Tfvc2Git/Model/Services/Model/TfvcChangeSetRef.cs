using System;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcChangeSetRef
    {
        [JsonPropertyName("author")]
        public IdentityRef Author { get; set; }
        [JsonPropertyName("changesetId")]
        public int ChangesetId { get; set; }
        [JsonPropertyName("checkedInBy")]
        public IdentityRef CheckedInBy { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        [JsonPropertyName("commentTruncated")]
        public bool CommentTruncated { get; set; }
        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
