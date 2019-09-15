using CommandLine;

namespace Tfvc2Git
{
    public class Options
    {
        [Option('t', "target-directory", Required =  true, HelpText = "The output directory for the target GIT directory. Must be empty.")]
        public string TargetDirectory { get; set; }
        
        [Option('s', "source-uri", Required = true, HelpText = "Location of the source TFVC server.")]
        public string SourceUri { get; set; }
        
        [Option('a', "access-token", Required =  true, HelpText = "Personal access token to the repo.")]
        public string AccessToken { get; set; }
        
        [Option('p', "project-name", Required =  true, HelpText = "The name of the project to convert.")]
        public string ProjectName { get; set; }
        
        [Option('o', "org-name", Required =  true, HelpText = "The name of the organization to convert.")]
        public string OrgName { get; set; }
        
        [Option(longName: "git-ignore", Required =  true, HelpText = "File path to a git ignore file to include.")]
        public string GitIgnoreTemplatePath { get; set; }
        
        [Option(longName: "git-attributes", Required = true, HelpText = "File path to a git attributes file to include")]
        public string GitAttributesTemplatePath { get; set; }
        
        [Option(longName: "from-changeset-id", HelpText = "The from change set.")]
        public int? FromChangeSet { get; set; }
        
        [Option("start-fresh", Required =  false, HelpText = "If the folder should be wiped before starting.")]
        public bool StartFresh { get; set; }
    }
}
