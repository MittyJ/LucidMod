using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace LucidMod.Content.Subworlds
{
    public class ExampleGenPass : GenPass
	{
		 private Action<GenerationProgress> method;

        public ExampleGenPass(Action<GenerationProgress> method)
            : base("", 1f)
        {
            this.method = method;
        }

        public ExampleGenPass(float weight, Action<GenerationProgress> method)
            : base("", weight)
        {
            this.method = method;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            method(progress);
        }

    }
}