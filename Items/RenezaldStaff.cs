
using LucidMod.Content.DamageClasses;
using LucidMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.Items
{
    public class RenezaldStaff : ModItem
    {
        public override void SetDefaults()
		{
			Item.damage = 21;
			Item.DamageType = ModContent.GetInstance<MonasticDamage>();
			Item.useTime = 10;
			Item.width = 52;
			Item.height = 52;
			Item.scale = 2;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MonkFlare>();
			Item.shootSpeed = 3;
		}

		
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox) {
			noHitbox = false;
			hitbox = new Rectangle(hitbox.X, hitbox.Y, 200, 200);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			Projectile.NewProjectile(source, new Vector2((int) player.Center.X, (int) player.Center.Y - 52), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
			Projectile.NewProjectile(source, new Vector2((int) player.Center.X, (int) player.Center.Y + 52), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
			Projectile.NewProjectile(source, new Vector2((int) player.Center.X - 52, (int) player.Center.Y), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
			Projectile.NewProjectile(source, new Vector2((int) player.Center.X + 52, (int) player.Center.Y), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
    }
}