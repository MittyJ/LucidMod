using System;
using System.Threading.Tasks;
using LucidMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.NPCs
{
	//One block = 16 pixels
	//Screen dimensions: 70 Blocks in height 120 blocks in width
    [AutoloadBossHead]
    public class ProjectionOfLenezald : ModNPC
    {
		private enum BossPhase {
			FIRST,
			SECOND,
			DESPERATION
		}

		private const int FRAME_TO_SECONDS = 60;

		private int firstPhaseTimer = 0;
		private int secondPhaseTimer = 0;
		private int desperationTimer = 0;

		private Vector2 arenaCenter = new Vector2(138 * PIXELS_IN_BLOCK, 97 * PIXELS_IN_BLOCK);
		//Arena left x = 88, right x = 189. Top = 47 bottom = 147

		private int dashTimer = 0;
		BossPhase bossPhase = BossPhase.FIRST;

		public const int PIXELS_IN_BLOCK = 16;

		private const int mainTimerMax = 90;
		// This is a reference property. It lets us write FirstStageTimer as if it's NPC.localAI[1], essentially giving it our own name
		public ref float mainTimer => ref NPC.localAI[1];
        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;

			// Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			// Specify the debuffs it is immune to. Most NPCs are immune to Confused.
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			// This boss also becomes immune to OnFire and all buffs that inherit OnFire immunity during the second half of the fight. See the ApplySecondStageBuffImmunities method.

		}

        public override void SetDefaults() {
			NPC.width = 40;
			NPC.height = 56;
			NPC.damage = 50;
			NPC.defense = 10;
			NPC.lifeMax = 20000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = Item.buyPrice(gold: 5);
			NPC.SpawnWithHigherTime(30);
			NPC.boss = true;
			NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight

			// Default buff immunities should be set in SetStaticDefaults through the NPCID.Sets.ImmuneTo{X} arrays.
			// To dynamically adjust immunities of an active NPC, NPC.buffImmune[] can be changed in AI: NPC.buffImmune[BuffID.OnFire] = true;
			// This approach, however, will not preserve buff immunities. To preserve buff immunities, use the NPC.BecomeImmuneTo and NPC.ClearImmuneToBuffs methods instead, as shown in the ApplySecondStageBuffImmunities method below.

			// Custom AI, 0 is "bound town NPC" AI which slows the NPC down and changes sprite orientation towards the target
			NPC.aiStyle = -1;

            NPC.alpha = 100;


			// The following code assigns a music track to the boss in a simple way.
			if (!Main.dedServ) {
				Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/LenezaldMusic");
			}
		}


		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			// // Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else

		}

		public override void HitEffect(NPC.HitInfo hit) {
			// If the NPC dies, spawn gore and play a sound
			if (Main.netMode == NetmodeID.Server) {
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			if (NPC.life <= 0) {

				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				// This adds a screen shake (screenshake) similar to Deerclops
				PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}
		
		//Arena left x = 88, right x = 189. Top = 47 bottom = 147
		//That is the solid block on the edge not the air
        public override void AI()
        {
            // This should almost always be the first code in AI() as it is responsible for finding the proper player target
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

			if (player.dead)
            {
                // If the targeted player is dead, flee
                NPC.velocity.Y -= 0.4f;
                // This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
                NPC.EncourageDespawn(10);
				return;
			}

            if (NPC.life < NPC.lifeMax * 0.6 && bossPhase == BossPhase.FIRST)
            {
                bossPhase = BossPhase.SECOND;
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }
            if (NPC.life < NPC.lifeMax * 0.03 && bossPhase == BossPhase.SECOND)
            {
                bossPhase = BossPhase.DESPERATION;
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }



            if (bossPhase == BossPhase.FIRST)
            {
                firstPhase(player);
            }
            else if (bossPhase == BossPhase.SECOND)
            {
				secondPhase(player);
            }
            else if (bossPhase == BossPhase.DESPERATION)
            {
                desperation(player);
            }


        }

        //Phase Methods:

        private async void firstPhase(Player player) {
			firstPhaseTimer++;
			if (firstPhaseTimer == 1) {
				teleport(player);
			}
			
			for (int i = 0; i < 7; i++) {
				if (firstPhaseTimer == (i + 1) * FRAME_TO_SECONDS) {
					await dashAtPlayer(player);
					diagonalBladeAttack(player);
					perpendicularBladeAttack(player);
				}
			}
			//8 seconds passed


			for (int i = 0; i < 3; i++) {
				if (firstPhaseTimer == (i + 8) * FRAME_TO_SECONDS) {
					teleportAndBlade(player);
				}
			}
			//11 seconds passed


			for (int i = 0; i < 4; i++) {
				if (firstPhaseTimer == (i * 2 + 11) * FRAME_TO_SECONDS) {
					if (i % 2 == 0) {
						swordWall(player, false);
					} else {
						swordWall(player, true);
					}
					
				}
			}
			//17 seconds passed


			if (firstPhaseTimer > 17.5 * FRAME_TO_SECONDS) {
				firstPhaseTimer = 0;
			}
		}

		private void secondPhase(Player player) {
			secondPhaseTimer++;
			for (int i = 0; i < 15; i++) {
				if (secondPhaseTimer == (1 + i) * FRAME_TO_SECONDS) {
					bulletHell(player);
				}
			}
			//16 seconds passsed

			for (int i = 0; i < 5; i++) {
				if (secondPhaseTimer == (17 + (i * 2)) * FRAME_TO_SECONDS) {
					if (i % 2 == 0) {
						skyBladeAttack(player, 0);
					} else {
						skyBladeAttack(player, 2);
					}
				}
			}
			//23 Seconds passed
			for (int i = 0; i < 4; i++) {
				if (secondPhaseTimer == ((26 + i) * FRAME_TO_SECONDS)) {
					bool left = false;
					if (i % 2 == 0) {
						left = true;
					}
					swordWall(player, left);
				}
			}
			//34 seconds passed

			for (int i = 1; i < 21; i++) {
				if (secondPhaseTimer == (36 + i) * FRAME_TO_SECONDS) {
					teleport(player);
					circleBladeAttack(player);
				}
			}
			//46 seconds passed

			if (secondPhaseTimer > 56 * FRAME_TO_SECONDS) {
				secondPhaseTimer = 0;
			}
		}
		private void desperation(Player player){
			desperationTimer++;
			if (desperationTimer == 20) {
				teleportAndBlade(player);
			} else if (desperationTimer == 45) {
				arroundTeleport(player);
				perpendicularBladeAttack(player);
				diagonalBladeAttack(player);
			}
			if (desperationTimer >= 46) {
				desperationTimer = 0;
			}
		}

		//Attack Methods:

		//Dashes towards the player and shoots five swords towards the player twice
		private async Task dashAndFiveBlades(Player player) {
			Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 25;
			if (target.X < 0) {
				NPC.rotation = (float)(target.ToRotation() + Math.PI);
			} else {
				NPC.rotation = target.ToRotation();
			}
			
			throwFiveBlades(player);
			await wait(250);
			NPC.velocity = target;
			throwFiveBlades(player);
			await wait(500);
			NPC.velocity = new Vector2(0, 0);
			NPC.rotation = 0;
		}

		//Summons a bullet Hell
		private void bulletHell(Player player) {
			int type = randomizeProjectile();
			int damage = NPC.damage / 2;
			var entitySource = NPC.GetSource_FromAI();

			for (int i = 0; i < 3; i++) {
				int ran = Main.rand.Next(-50, 50);
					Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X + (ran * PIXELS_IN_BLOCK), arenaCenter.Y - (60 * PIXELS_IN_BLOCK)), new Vector2(0, 1), type, damage, 0f, Main.myPlayer);
			}

			for (int i = 0; i < 3; i++) {
				int ran = Main.rand.Next(-50, 50);
					Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X + (ran * PIXELS_IN_BLOCK), arenaCenter.Y + (60 * PIXELS_IN_BLOCK)), new Vector2(0, -1), type, damage, 0f, Main.myPlayer);
			}

			for (int i = 0; i < 3; i++) {
				int ran = Main.rand.Next(-25, 25);
					Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X + (60 * PIXELS_IN_BLOCK), arenaCenter.Y + (ran * PIXELS_IN_BLOCK)), new Vector2(-1, 0), type, damage, 0f, Main.myPlayer);
			}

			for (int i = 0; i < 3; i++) {
				int ran = Main.rand.Next(-25, 25);
					Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X - (60 * PIXELS_IN_BLOCK), arenaCenter.Y + (ran * PIXELS_IN_BLOCK)), new Vector2(1, 0), type, damage, 0f, Main.myPlayer);
			}
		}

		//Summons blades in a circle around lenezald and fires them at player, should be pared with teleport
		private void circleBladeAttack(Player player) {
			int type = ModContent.ProjectileType<SpecialLenezaldSword>();
			int damage = NPC.damage / 2;
			var entitySource = NPC.GetSource_FromAI();

			

			ModProjectile[] circleProjectiles = new ModProjectile[8];

			double swordSpacing = Math.PI * 2 / 16;

			for (int i = 0; i < 16; i++) {
				Projectile.NewProjectile(entitySource, new Vector2(NPC.Center.X + (float)Math.Cos(swordSpacing * (i)) * (10 * PIXELS_IN_BLOCK), NPC.Center.Y + (float)Math.Sin(swordSpacing * (i)) * (10 * PIXELS_IN_BLOCK)), new Vector2(0, 0), type, damage, 0f, Main.myPlayer);
			}

		}

		//Shoot five swords in the direction of the player 
		private void throwFiveBlades(Player player) {
			Vector2 position = NPC.Bottom;

			int type = randomizeProjectile();
			int damage = NPC.damage / 2;
			var entitySource = NPC.GetSource_FromAI();

			Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 4;
			Projectile.NewProjectile(entitySource, position, Vector2.Add(target, new Vector2(0, 2)), type, damage, 0f, Main.myPlayer);
			Projectile.NewProjectile(entitySource, position, Vector2.Add(target, new Vector2(0, 4)), type, damage, 0f, Main.myPlayer);
			Projectile.NewProjectile(entitySource, position, target, type, damage, 0f, Main.myPlayer);
			Projectile.NewProjectile(entitySource, position, Vector2.Add(target, new Vector2(0, -2)), type, damage, 0f, Main.myPlayer);
			Projectile.NewProjectile(entitySource, position, Vector2.Add(target, new Vector2(0, -4)), type, damage, 0f, Main.myPlayer);
		}

		//Teleports Above or Underneath the player and shoots a blade at them
		private void underneathBladeAttack(Player player) {
			if (Main.rand.NextBool() == true) {
				NPC.Teleport(new Vector2(player.Center.X , player.Center.Y - (PIXELS_IN_BLOCK * 25)), 1);
			} else {
				NPC.Teleport(new Vector2(player.Center.X , player.Center.Y + (PIXELS_IN_BLOCK * 25)), 1);
			}
			shootPlayerBlade(player);
		}

		//Shoots 4 blades perpendicular like a cross coming from lenezald
		private void perpendicularBladeAttack(Player player) {

			if (NPC.HasValidTarget && mainTimer == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				// Spawn projectile randomly below player, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
				// (The projectiles accelerate from their initial velocity)

				float kitingOffsetX = Utils.Clamp(player.velocity.X * 16, -100, 100);
				Vector2 position = NPC.Bottom;

				int type = randomizeProjectile();
				int damage = NPC.damage / 2;
				var entitySource = NPC.GetSource_FromAI();
				Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 4;

				Projectile.NewProjectile(entitySource, position, new Vector2(0, 3), type, damage, 0f, Main.myPlayer);
				Projectile.NewProjectile(entitySource, position, new Vector2(3, 0), type, damage, 0f, Main.myPlayer);
				Projectile.NewProjectile(entitySource, position, new Vector2(0, -3), type, damage, 0f, Main.myPlayer);
				Projectile.NewProjectile(entitySource, position, new Vector2(-3, 0), type, damage, 0f, Main.myPlayer);
			}
		}

		//Shoots 4 blades diagonally from lenezald
		private void diagonalBladeAttack(Player player) {

			if (NPC.HasValidTarget && mainTimer == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				// Spawn projectile randomly below player, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
				// (The projectiles accelerate from their initial velocity)

				float kitingOffsetX = Utils.Clamp(player.velocity.X * 16, -100, 100);
				Vector2 position = NPC.Bottom;

				int type = randomizeProjectile();
				int damage = NPC.damage / 2;
				var entitySource = NPC.GetSource_FromAI();
				Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 4;

				Projectile.NewProjectile(entitySource, position, new Vector2(3, 3), type, damage, 0f, Main.myPlayer);
				Projectile.NewProjectile(entitySource, position, new Vector2(3, -3), type, damage, 0f, Main.myPlayer);
				Projectile.NewProjectile(entitySource, position, new Vector2(-3, 3), type, damage, 0f, Main.myPlayer);
				Projectile.NewProjectile(entitySource, position, new Vector2(-3, -3), type, damage, 0f, Main.myPlayer);
			}
		}

		//A wall of swords fall from the sky, can shift a number of blocks each iteration
		public void skyBladeAttack(Player player, int shift) {
			if (NPC.HasValidTarget && mainTimer == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				// Spawn projectile randomly below player, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
				// (The projectiles accelerate from their initial velocity)

				float kitingOffsetX = Utils.Clamp(player.velocity.X * 16, -100, 100);
				Vector2 position = NPC.Bottom;

				int type = randomizeProjectile();
				int damage = NPC.damage / 2;
				var entitySource = NPC.GetSource_FromAI();
				Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X, arenaCenter.Y - 376), new Vector2(0, 2), type, damage, 0f, Main.myPlayer);
				for (int i = 1; i < 10; i++) {
					Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X + (11 * PIXELS_IN_BLOCK * i) + shift, arenaCenter.Y - 45 * PIXELS_IN_BLOCK), new Vector2(0, 2), type, damage, 0f, Main.myPlayer);
					Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X + (-11* PIXELS_IN_BLOCK * i) - shift, arenaCenter.Y - 45 * PIXELS_IN_BLOCK), new Vector2(0, 2), type, damage, 0f, Main.myPlayer);
				}

			}
			
		}

		//Teleports randomally around the player and shoots a blade their way
		public void teleportAndBlade(Player player) {
			int playerDistanceTeleport = 20 * PIXELS_IN_BLOCK;
			int sign = 1;
			if (Main.rand.NextBool() == true) {
				sign = -1;
			}
			NPC.Teleport(new Vector2(player.Center.X + (PIXELS_IN_BLOCK * Main.rand.Next(-25, 25)) + playerDistanceTeleport, player.Center.Y + (PIXELS_IN_BLOCK * Main.rand.Next(-15, 15)) + playerDistanceTeleport), 1);
			shootPlayerBlade(player);
		}
		
		//Shoots a blade from lenezald coming at the player
		public void shootPlayerBlade(Player player) {
			if (NPC.HasValidTarget && mainTimer == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				// Spawn projectile randomly below player, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
				// (The projectiles accelerate from their initial velocity)

				float kitingOffsetX = Utils.Clamp(player.velocity.X * 16, -100, 100);
				Vector2 position = NPC.Bottom;

				int type = randomizeProjectile();
				int damage = NPC.damage / 2;
				var entitySource = NPC.GetSource_FromAI();
				Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 4;

				Projectile.NewProjectile(entitySource, position, target, type, damage, 0f, Main.myPlayer);
			}
		}

		//Sends a wall of swords coming at the player from left or right, and leaves a hole for the player to make it into
		public void swordWall(Player player, bool left) {
			if (left) {
				NPC.Teleport(new Vector2(player.Center.X + 30 * PIXELS_IN_BLOCK, player.Center.Y), 1);
			} else {
				NPC.Teleport(new Vector2(player.Center.X + -30 * PIXELS_IN_BLOCK, player.Center.Y), 1);
			}

			int opening = Main.rand.Next(6);
			for (int i = 0; i < 13; i++) {
				if (NPC.HasValidTarget && mainTimer == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				// Spawn projectile randomly below player, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
				// (The projectiles accelerate from their initial velocity)

					int type = randomizeProjectile();
					int damage = NPC.damage / 2;
					var entitySource = NPC.GetSource_FromAI();

					if (i != opening) {
						if (left) {
							Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X + (50 * PIXELS_IN_BLOCK), arenaCenter.Y + (i * 5 * PIXELS_IN_BLOCK)), new Vector2(-3, 0), type, damage, 0f, Main.myPlayer);
							Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X + (50 * PIXELS_IN_BLOCK), arenaCenter.Y - (i * 5 * PIXELS_IN_BLOCK)), new Vector2(-3, 0), type, damage, 0f, Main.myPlayer);
						} else {
							Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X - (50 * PIXELS_IN_BLOCK), arenaCenter.Y + (i * 5 * PIXELS_IN_BLOCK)), new Vector2(3, 0), type, damage, 0f, Main.myPlayer);
							Projectile.NewProjectile(entitySource, new Vector2(arenaCenter.X - (50 * PIXELS_IN_BLOCK), arenaCenter.Y - (i * 5 * PIXELS_IN_BLOCK)), new Vector2(3, 0), type, damage, 0f, Main.myPlayer);
						}
					}
				
				}
			}
		}

		//Util Methods

		//Teleports side to side, or in each corner of the player
		private void arroundTeleport(Player player) {
			int ran = Main.rand.Next(7);

			if (ran == 0) {
				NPC.Teleport(new Vector2(player.Center.X + 45 * PIXELS_IN_BLOCK, player.Center.Y + 15 * PIXELS_IN_BLOCK), 1);
			} else if (ran == 1) {
				NPC.Teleport(new Vector2(player.Center.X - 45 * PIXELS_IN_BLOCK, player.Center.Y + 15 * PIXELS_IN_BLOCK), 1);
			} else if (ran == 2) {
				NPC.Teleport(new Vector2(player.Center.X + 45 * PIXELS_IN_BLOCK, player.Center.Y - 15 * PIXELS_IN_BLOCK), 1);
			} else if (ran == 3) {
				NPC.Teleport(new Vector2(player.Center.X - 45 * PIXELS_IN_BLOCK, player.Center.Y - 15 * PIXELS_IN_BLOCK), 1);
			} else if (ran == 4 || ran == 5) {
				NPC.Teleport(new Vector2(player.Center.X - 45 * PIXELS_IN_BLOCK, player.Center.Y), 1);
			} else if (ran == 6 || ran == 7) {
				NPC.Teleport(new Vector2(player.Center.X + 45 * PIXELS_IN_BLOCK, player.Center.Y), 1);
			}
		}

		//Dashes lenezald at the player
		private async Task dashAtPlayer(Player player) {
			Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 25;
			if (target.X < 0) {
				NPC.rotation = (float)(target.ToRotation() + Math.PI);
			} else {
				NPC.rotation = target.ToRotation();
			}
			SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
			await wait(250);
			NPC.velocity = target;
			await wait(500);
			NPC.velocity = new Vector2(0, 0);
			NPC.rotation = 0;
		}

		private static async Task wait(int milliseconds){
        	await Task.Delay(milliseconds);
    	}

		//Chooses one of lenezalds swords randomly
		private int randomizeProjectile(){
			int num = Main.rand.Next(6);
			switch(num){
				case 0:
					return ModContent.ProjectileType<LenezaldSword>();
				case 1:
					return ModContent.ProjectileType<LenezaldBloodSword>();
				case 2:
					return ModContent.ProjectileType<LenezaldCutlass>();
				case 3:
					return ModContent.ProjectileType<LenezaldExcalibur>();
				case 4:
					return ModContent.ProjectileType<LenezaldMuramasa>();
				case 5:
					return ModContent.ProjectileType<LenezaldPalladium>();
				case 6:
					return ModContent.ProjectileType<LenezaldStarWrath>();
			}
			return ModContent.ProjectileType<LenezaldSword>();
		}

		//Teleports somewhere near the player, but not too close, randomly
		private void teleport(Player player) {
			int playerDistanceTeleport = 25 * PIXELS_IN_BLOCK;
			NPC.Teleport(new Vector2(player.Center.X + (PIXELS_IN_BLOCK * Main.rand.Next(-25, 25)) + playerDistanceTeleport, player.Center.Y + (PIXELS_IN_BLOCK * Main.rand.Next(-15, 15)) + playerDistanceTeleport));
		}

    }
	
}