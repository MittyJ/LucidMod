using LucidMod.Content.Subworlds;
using LucidMod.Items;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.NPCs
{
    public class BenightedHusk : ModNPC
    {
        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				// Influences how the NPC looks in the Bestiary
				Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.alpha = 100;
			NPC.height = 40;
			NPC.damage = 14;
			NPC.defense = 6;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3; // Fighter AI, important to choose the aiStyle that matches the NPCID that we want to mimic

			AIType = NPCID.Zombie; // Use vanilla zombie's type when executing AI code. (This also means it will try to despawn during daytime)
			AnimationType = NPCID.Zombie; // Use vanilla zombie's type when executing animation code. Important to also match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
		}

        public override void OnKill() {
			int num = Main.rand.Next(6);
			if (num == 1) {
				Item.NewItem(NPC.GetSource_Death(), NPC.Center, ModContent.ItemType<PhilosopherToken>(), 1, true);
			}
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			// Can only spawn in the ExampleSurfaceBiome and if there are no other ExampleZombieThiefs
			if (SubworldSystem.IsActive<LucidSubworld>()) {
				return 2f;
			}
			return 0;
		}
    }
}