using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    public class ReferenceLinks
    {
        [JsonPropertyName("links")]
        public object Links { get; set; }
    }
}
