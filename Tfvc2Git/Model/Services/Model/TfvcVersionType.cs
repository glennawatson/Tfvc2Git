using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TfvcVersionType
    {
        change,
        changeset,
        date,
        latest,
        mergeSource,
        none,
        shelveset,
        tip,
    }
}
