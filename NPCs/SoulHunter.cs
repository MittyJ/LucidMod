using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.Systems;
using LucidMod.Items;
using Terraria.DataStructures;

namespace LucidMod.NPCs
{
	[AutoloadHead]
	public class SoulHunter : ModNPC
	{
		//Quest: Gather Philospher tokens from benighted husks, quest rewards armor

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
			NPC.defense = 200;
			NPC.lifeMax = 10000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Chenezald",
			};
		}

		public override string GetChat() {
            int num = Main.rand.Next(2);
            switch (num) {
                case 0:
                    return "Welcome to the Lucid Jungle, I am Chenezald";
                case 1:
                    return "This place isn't physical, anyone enlightened enough can access it from the physical world";
                case 2:
                    return "You must urgently talk to the Old Philosopher. Legend says he resides in the sky";
            }
            return "Hello I am Chenezald, my chat is broken";
		}


		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Quest";
			button2 = "Inquire";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				bool questComplete = false;
				int slot = -1;
				for (int i = 0; i < 59; i++) {
					if (Main.LocalPlayer.inventory[i].type == ModContent.ItemType<PhilosopherToken>()) {
						if (Main.LocalPlayer.inventory[i].stack >= 5) {
							questComplete = true;
							slot = i;
						}
					}
				}
				if (questComplete) {
					Main.npcChatText = "Thank you! For your help I can give you the monk's armor";
					Main.LocalPlayer.inventory[slot].stack -= 5;
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, ModContent.ItemType<MonasticHood>());
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, ModContent.ItemType<MonasticRobe>()); 
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, ModContent.ItemType<MonasticLeggings>()); 
				} else {
					Main.npcChatText = "Go find me 5 philospher tokens by killing benighted husks, if you do so I can give you the monks armor set";
				}
			} else {
				Main.npcChatText = "Benighted husks are those who came to the lucid jungle and died, but their physical body still exists";
			}
			}

		
	}
}