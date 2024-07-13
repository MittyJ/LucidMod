

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.Content.Projectiles
{
    public class LenezaldSword : ModProjectile
    {

        public override void SetDefaults() {
			Projectile.width = 67; // The width of projectile hitbox
			Projectile.height = 20; // The height of projectile hitbox

			// Copy the ai of any given projectile using AIType, since we want
			// the projectile to essentially behave the same way as the vanilla projectile.
			AIType = 0;

			Projectile.friendly = false; // Can the projectile deal damage to enemies?
			Projectile.hostile = true;
			Projectile.damage = 50;
			Projectile.penetrate = -1;
			Projectile.light = 0.75f;
			// Projectile.alpha = 100;
			Projectile.extraUpdates = 2;
			Projectile.scale = 1f;
			Projectile.timeLeft = 6000;
			Projectile.tileCollide = false;

			// 1: Projectile.penetrate = 1; // Will hit even if npc is currently immune to player
			// 2a: Projectile.penetrate = -1; // Will hit and unless 3 is use, set 10 ticks of immunity
			// 2b: Projectile.penetrate = 3; // Same, but max 3 hits before dying
			// 5: Projectile.usesLocalNPCImmunity = true;
			// 5a: Projectile.localNPCHitCooldown = -1; // 1 hit per npc max
			// 5b: Projectile.localNPCHitCooldown = 20; // 20 ticks before the same npc can be hit again
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
    }
}