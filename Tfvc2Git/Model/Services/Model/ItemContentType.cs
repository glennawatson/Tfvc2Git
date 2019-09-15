using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    [DataContract]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemContentType
    {
        [EnumMember(Value = "base64Encoded")]
        Base64Encoded,
        
        [EnumMember(Value = "rawText")]
        RawText,
    }
}
