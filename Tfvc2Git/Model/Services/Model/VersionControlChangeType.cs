using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services.Model
{
    [Flags]
    [DataContract]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VersionControlChangeType
    {
        [EnumMember(Value = "add")]
        Add = 1,
        
        [EnumMember(Value = "all")]
        All = 8191,

        [EnumMember(Value = "branch")]
        Branch = 64,

        [EnumMember(Value = "delete")]
        Delete = 16,

        [EnumMember(Value = "edit")]
        Edit = 2,

        [EnumMember(Value = "encoding")]
        Encoding = 4,

        [EnumMember(Value = "lock")]
        Lock = 256,
        
        [EnumMember(Value = "merge")]
        Merge = 128,
        
        [EnumMember(Value = "none")]
        None = 0,

        [EnumMember(Value = "property")]
        Property = 4096,

        [EnumMember(Value = "rename")]
        Rename = 8,

        [EnumMember(Value = "rollback")]
        Rollback = 512,

        [EnumMember(Value = "sourceRename")]
        SourceRename = 1024,

        [EnumMember(Value = "targetRename")]
        TargetRename = 2048,

        [EnumMember(Value = "undelete")]
        Undelete = 32,
    }
}
