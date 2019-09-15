using System;

namespace Tfvc2Git.Model.Services
{
    public class GetChangeSetsParams
    {
        [AliasAs("maxCommentLength")]
        public int? MaxCommentLength { get; set; }
        
        [AliasAs("$skip")]
        public int? Skip { get; set; }
        
        [AliasAs("$top")]
        public int? Top { get; set; }
        
        [AliasAs("$orderby")]
        public string OrderBy { get; set; }

        [AliasAs("searchCriteria.includeLinks")]
        public bool? IncludeLinks { get; set; }
        
        [AliasAs("searchCriteria.followRenames")]
        public bool? FollowRenames { get; set; }
        
        [AliasAs("searchCriteria.toId")]
        public int? ToId { get; set; }
        
        [AliasAs("searchCriteria.fromId")]
        public int? FromId { get; set; }
        
        [AliasAs("searchCriteria.toDate")]
        public DateTime? ToDate { get; set; }
        
        [AliasAs("searchCriteria.fromDate")]
        public DateTime? FromDate { get; set; }
        
        [AliasAs("searchCriteria.author")]
        public string Author { get; set; }
        
        [AliasAs("searchCriteria.itemPath")]
        public string ItemPath { get; set; }
    }
}
