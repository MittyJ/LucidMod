using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.Systems;

namespace LucidMod.NPCs
{
	[AutoloadHead]
	public class Abbot : ModNPC
	{
		//Quest: Kill a miniboss, gain wings


		InventorySaveSystem inventorySaveSystem = new InventorySaveSystem();
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 23; // The total amount of frames the NPC has
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ShimmerTownTransform[Type] = false; // Allows for this NPC to have a different texture after touching the Shimmer liquid.
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Anthoniezald",
			};
		}

		public override string GetChat() {
            int num = Main.rand.Next(2);
            switch (num) {
                case 0:
                    return "Welcome to the Lucid Jungle, I am Anthoniezald";
                case 1:
                    return "This place isn't physical, anyone enlightened enough can access it from the physical world";
                case 2:
                    return "While the Lucid Jungle is not a real place, it is far from a figment of the imagination";
            }
            return "Hello I am Anthoniezald, my chat is broken";
		}


		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Quest";
			button2 = "Inquire";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
			} else {
			}
			}

		
	}
}