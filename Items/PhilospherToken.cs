using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LucidMod.Items
{
    public class PhilospherToken : ModItem
    {
        public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
		}

        public override bool CanStack(Item source)
        {
            return base.CanStack(source);
        }

    }
}