using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LucidMod.Content.Projectiles
{
    public class TinglerBeam : ModProjectile
    {
         public override void SetDefaults() {
			Projectile.width = 52;
			Projectile.height = 68;

			// Copy the ai of any given projectile using AIType, since we want
			// the projectile to essentially behave the same way as the vanilla projectile.
			AIType = 0;

			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.damage = 20;
			Projectile.penetrate = -1;
			Projectile.light = 0.75f;
			Projectile.alpha = 100;
            Projectile.scale = 2;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 400;
			Projectile.tileCollide = false;
            Projectile.velocity = new Microsoft.Xna.Framework.Vector2(0.5f, 0);

			// 1: Projectile.penetrate = 1; // Will hit even if npc is currently immune to player
			// 2a: Projectile.penetrate = -1; // Will hit and unless 3 is use, set 10 ticks of immunity
			// 2b: Projectile.penetrate = 3; // Same, but max 3 hits before dying
			// 5: Projectile.usesLocalNPCImmunity = true;
			// 5a: Projectile.localNPCHitCooldown = -1; // 1 hit per npc max
			// 5b: Projectile.localNPCHitCooldown = 20; // 20 ticks before the same npc can be hit again
		}

		public override void AI() {
			Projectile.rotation = (float)(Projectile.rotation + Math.PI / 30);
    
		}
    }
}