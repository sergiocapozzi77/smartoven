using Newtonsoft.Json.Linq;
using SmartOvenV2.Annotations;
using Syncfusion.XForms.ProgressBar;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace SmartOvenV2.Models
{
    public class Ingredient
    {
        public string Name { get; }

        public double Quantity { get; }

        public string Unit { get; }

        public string Group { get; }

        public Ingredient(string name, double quantity, string unit, string group)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Group = group;
        }

        internal static Ingredient FromJToken(JToken recipeStep)
        {
            return new Ingredient(
                recipeStep.Value<string>("Name"),
                recipeStep.Value<double>("Quantity"),
                recipeStep.Value<string>("Unit"),
                recipeStep.Value<string>("Group")
                );
        }
    }
}
