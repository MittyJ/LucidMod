using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using LucidMod.Content.Projectiles;

namespace LucidMod.Items
{
	public class Tingler : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.LucidMod.hjson file.

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = ModContent.GetInstance<MonasticDamage>();
			Item.useTime = 10;
			Item.width = 52;
			Item.height = 68;
			Item.scale = 2;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TinglerBeam>();
			Item.shootSpeed = 3;
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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			Projectile.NewProjectile(source, player.Center, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

			return false;
		}

	}
}