using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.DamageClasses;
using Terraria.DataStructures;

namespace LucidMod.Items
{
	[AutoloadEquip(EquipType.Wings)]
	public class InsigniaOfRenezald : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.LucidMod.hjson file.
		public static readonly int MonasticDamageModifier = 15;


		public override void SetStaticDefaults() {
			// These wings use the same values as the solar wings
			// Fly time: 180 ticks = 3 seconds
			// Fly speed: 9
			// Acceleration multiplier: 2.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(9000, 16f, 6f);
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.accessory = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(ModContent.GetInstance<MonasticDamage>()) *= 1 + MonasticDamageModifier / 100f;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
			ascentWhenFalling = 0.85f; // Falling glide speed
			ascentWhenRising = 0.15f; // Rising speed
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		
	}
}