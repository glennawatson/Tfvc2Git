using System;

namespace Tfvc2Git.Model.Services
{
    public class AliasAsAttribute : Attribute
    {
        public AliasAsAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}
