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
	public class Abbot : ModNPC
	{
		//Quest: Kill a miniboss, gain wings

		bool questAsked = false;
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
				"Anthoniezald",
			};
		}

		public override string GetChat() {
            int num = Main.rand.Next(2);
            switch (num) {
                case 0:
                    return "I am Anthoniezald, an old abbot. I've lived in this realm for decades";
                case 1:
                    return "The Old Philiosopher lives in the sky of this realm, you will need to talk to him";
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
				bool questComplete = false;
				int slot = -1;
				for (int i = 0; i < 59; i++) {
					if (Main.LocalPlayer.inventory[i].type == ModContent.ItemType<UnboundSoul>()) {
						if (Main.LocalPlayer.inventory[i].stack >= 1) {
							questComplete = true;
							slot = i;
						}
					}
				}
				if (questComplete && questAsked) {
					Main.npcChatText = "Thank you! For your help I can give you the Insignia of Renezald";
					Main.LocalPlayer.inventory[slot].stack -= 1;
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, ModContent.ItemType<InsigniaOfRenezald>()); 
				} else {
					Main.npcChatText = "Aquire me an Unbound Soul by defeating a Corpseless Ego, and I will aid you greatly in your journey. Corpseless egos awaken at night";
					questAsked = true;
				}
			} else {
				Main.npcChatText = "Corpseless Egos are those who have completely detached themselves from the physical world, only existing in this realm";
			}
			}

		
	}
}