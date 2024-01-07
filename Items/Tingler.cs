using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;

namespace LucidMod.Items
{
	public class Tingler : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.LucidMod.hjson file.

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = ModContent.GetInstance<MonasticDamage>();
			Item.useTime = 10;
			Item.width = 40;
			Item.height = 40;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox) {
			noHitbox = false;
			hitbox = new Rectangle(hitbox.X, hitbox.Y, 200, 200);
		}

	}
}