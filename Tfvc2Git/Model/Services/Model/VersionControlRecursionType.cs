using System.Text.Json.Serialization;

namespace Tfvc2Git.Model.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VersionControlRecursionType
    {
        /// <summary>
        /// Return specified item and all descendants
        /// </summary>
        Full,

        /// <summary>
        /// Only return the specified item.
        /// </summary>
        None,

        /// <summary>
        /// Only return the specified item.
        /// </summary>
        OneLevel,

        /// <summary>
        /// Return the specified item and its direct children.
        /// </summary>
        OneLevelPlusNestedEmptyFolders
    }
}
