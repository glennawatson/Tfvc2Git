using System.Text.Json.Serialization;
using Tfvc2Git.Model.Services.Model;

namespace Tfvc2Git.Model.Services
{
    public class GetItemParams
    {
        [AliasAs("fileName")]
        public string fileName { get; set; }
        [AliasAs("download")]
        public bool download { get; set; }
        [AliasAs("scopePath")]
        public string scopePath { get; set; }
        [AliasAs("recursionLevel")]
        public VersionControlRecursionType? recursionLevel { get; set; }
        [AliasAs("versionDescriptor.version")]
        public string Version { get; set; }
        [AliasAs("versionDescriptor.versionType")]
        public TfvcVersionType? VersionType { get; set; }
        [AliasAs("versionDescriptor.versionOption")]
        public TfvcVersionOption VersionOption { get; set; }
        [AliasAs("includeContent")]
        public bool? includeContent { get; set; }
    }
}
