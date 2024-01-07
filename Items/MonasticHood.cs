using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using LucidMod.Content.DamageClasses;

namespace LucidMod.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class MonasticHood : ModItem
    {
        public static readonly int MonasticDamageModifier = 6;

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 3; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			player.GetDamage(ModContent.GetInstance<MonasticDamage>()) *=  1 + MonasticDamageModifier / 100f;
		}
    }
}