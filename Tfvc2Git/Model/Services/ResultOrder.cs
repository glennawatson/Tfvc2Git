using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services
{
    [DataContract]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResultOrder
    {
        [EnumMember(Value = "")]
        Descending,

        [EnumMember(Value = "asc")]
        Ascending,
    }
}
