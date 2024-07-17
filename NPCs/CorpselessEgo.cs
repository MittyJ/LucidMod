
using System;
using LucidMod.Content.Subworlds;
using LucidMod.Items;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.NPCs
{
    public class CorpselessEgo : ModNPC
    {
        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				// Influences how the NPC looks in the Bestiary
				Velocity = 3f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			NPC.width = 40;
			NPC.alpha = 150;
			NPC.height = 40;
			NPC.damage = 30;
			NPC.defense = 6;
			NPC.lifeMax = 600;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
            NPC.noTileCollide = true;

		}

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            Vector2 target = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 2;

			NPC.velocity = target;
        }

        public override void OnKill() {
			Item.NewItem(NPC.GetSource_Death(), NPC.Center, ModContent.ItemType<UnboundSoul>(), 1, true);
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			// Can only spawn in the ExampleSurfaceBiome and if there are no other ExampleZombieThiefs
			if (SubworldSystem.IsActive<LucidSubworld>()) {
				return .1f;
			}
			return 0;
		}
    }
}
