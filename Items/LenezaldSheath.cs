
using System;
using LucidMod.Content.DamageClasses;
using LucidMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.Items
{
    public class LenezaldSheath : ModItem
    {
        const int PIXELS_IN_BLOCK = 16;
        const int SHOT_SPEED = 20;
        const int BLOCKS_AWAY_FROM_PLAYER = 10;
        double swordSpacing = Math.PI * 2 / 6;
        Projectile[] weaponProjectiles = new Projectile[6];
        int weaponIndex = 0;
        public override void SetDefaults()
		{
			Item.damage = 21;
			Item.DamageType = ModContent.GetInstance<MonasticDamage>();
			Item.useTime = 10;
			Item.width = 20;
			Item.height = 52;
			Item.scale = 2;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.knockBack = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
            Item.shoot = ProjectileID.BeeArrow;
            Item.noMelee = true;
		}


        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            for (int i = 0; i < 6; i++) {
                if (weaponProjectiles[i] != null) {
                    weaponProjectiles[i].position = new Vector2(player.Center.X + (float)Math.Cos(swordSpacing * i) * (BLOCKS_AWAY_FROM_PLAYER * PIXELS_IN_BLOCK), player.Center.Y + (float)Math.Sin(swordSpacing * i) * (BLOCKS_AWAY_FROM_PLAYER * PIXELS_IN_BLOCK));
                    if (target.Y > player.Center.Y) {
                        weaponProjectiles[i].rotation = (float)((float) (Math.PI / 2) - Math.Atan((target.X - player.Center.X) / (target.Y - player.Center.Y)));
                    } else {
                        weaponProjectiles[i].rotation = (float)((float) (Math.PI / 2) + Math.Atan((target.X - player.Center.X) / (target.Y - player.Center.Y))) * -1;
                    }
                }
            }

        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            if (player.altFunctionUse == 2) {
                for (int i = 0; i < 6; i++) {
                    if (weaponProjectiles[i] != null) {
                        weaponProjectiles[i].Kill();
                        weaponProjectiles[i] = null;
                    }
                }
                foreach (var projectile in Main.ActiveProjectiles) {
                    if (projectile.type == ModContent.ProjectileType<LenezaldSheathSword>()) {
                        projectile.Kill();
                    }
                }
                int projCount = 0;
                for (int i = 0; i < 6; i++) {
                    Projectile.NewProjectile(source, new Vector2(player.Center.X + (float)Math.Cos(swordSpacing * i) * (10 * PIXELS_IN_BLOCK), player.Center.Y + (float)Math.Sin(swordSpacing * i) * (10 * PIXELS_IN_BLOCK)), new Vector2(0, 0), ModContent.ProjectileType<LenezaldSheathSword>(), damage, 0f, Main.myPlayer);
                }
                foreach (var projectile in Main.ActiveProjectiles) {
                    if (projectile.type == ModContent.ProjectileType<LenezaldSheathSword>()) {
                        if (projCount < 6) {
                            weaponProjectiles[projCount] = projectile;
                            projCount++;
                        }
                    }
                }
                weaponIndex = 0;
            } else {
                if (weaponIndex < 6) {
                    weaponProjectiles[weaponIndex].velocity = new Vector2((float)(Math.Cos(weaponProjectiles[weaponIndex].rotation) * SHOT_SPEED), (float)(Math.Sin(weaponProjectiles[weaponIndex].rotation) * SHOT_SPEED));
                    weaponProjectiles[weaponIndex] = null;
                    weaponIndex++;
                }
            }
            

			return false;
		}
    }
}