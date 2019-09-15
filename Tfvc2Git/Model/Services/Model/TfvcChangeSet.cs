using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class TfvcChangeSet
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }
        [JsonPropertyName("author")]
        public IdentityRef Author { get; set; }
        [JsonPropertyName("changes")]
        public List<TfvcChange> Changes { get; set; }
        [JsonPropertyName("changesetId")]
        public int ChangesetId { get; set; }
        [JsonPropertyName("checkedInBy")]
        public IdentityRef CheckedInBy { get; set; }
        [JsonPropertyName("checkinNotes")]
        public List<CheckinNote> CheckinNotes { get; set; }
        [JsonPropertyName("collectionId")]
        public string CollectionId { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        [JsonPropertyName("commentTruncated")]
        public bool CommentTruncated { get; set; }
        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonPropertyName("hasMoreChanges")]
        public bool HasMoreChanges { get; set; }
        [JsonPropertyName("policyOverride")]
        public TfvcPolicyOverrideInfo PolicyOverride { get; set; }
        [JsonPropertyName("teamProjectIds")]
        public List<string> TeamProjectIds { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("workItems")]
        public List<AssociatedWorkItem> WorkItems { get; set; }
    }
}
