using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOvenV2.Models
{
    public class Method
    {
        public string Description { get; }

        public string Group { get; }

        public Method(string description, string group)
        {
            Description = description;
            Group = group;
        }

        internal static Method FromJToken(JToken recipeStep)
        {
            return new Method(
                recipeStep.Value<string>("Description"),
                recipeStep.Value<string>("Group")
                );
        }
    }
}
