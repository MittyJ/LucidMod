using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.DamageClasses;
using Terraria.GameContent.UI;


namespace LucidMod.Items
{
	public class Zald : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.LucidMod.hjson file.

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = ModContent.GetInstance<MonasticDamage>();
			Item.useTime = 20;
			Item.width = 40;
			Item.height = 40;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.SwordBeam;
			Item.shootSpeed = 3;

		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (Main.rand.NextBool()) {
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
			}

			return false;
		}

	}
}