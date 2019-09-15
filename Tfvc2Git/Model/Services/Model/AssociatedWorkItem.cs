using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class AssociatedWorkItem
    {
        [JsonPropertyName("assignedTo")]
        public string AssignedTo { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("webUrl")]
        public string WebUrl { get; set; }
        [JsonPropertyName("workItemType")]
        public string WorkItemType { get; set; }
    }
}
