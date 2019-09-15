using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services
{
    public class ArrayResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Values { get; set; }
        
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
