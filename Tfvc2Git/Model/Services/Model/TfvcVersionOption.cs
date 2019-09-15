using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TfvcVersionOption
    {
        none,
        previous,
        useRename
    }
}
