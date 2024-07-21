using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.Systems;
using SubworldLibrary;
using LucidMod.Content.Subworlds;
using Terraria.DataStructures;
using LucidMod.Items;
using Terraria.ModLoader.IO;

namespace LucidMod.NPCs
{
	[AutoloadHead]
	public class OldPhilosopher : ModNPC
	{
		int chatIndex = 1;
		bool hasGottenLenezald = false;

		InventorySaveSystem inventorySaveSystem = new InventorySaveSystem();
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 26; // The total amount of frames the NPC has
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
			NPC.lifeMax = 10000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Renezald",
			};
		}

		public override string GetChat() {
			return "I am Renezald the Old Philosophor. I ask you to assist me in vanquishing a great threat against this realm: Lenezald";
		}


		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Next";
			button2 = "Quest";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				switch (chatIndex) {
					case 1:
						chatIndex++;
						Main.npcChatText = "Lenezald has disturbed the balance of this realm. He is a malicous monk and must be vanquished";
						break;
					case 2:
						chatIndex++;
						Main.npcChatText = "In order to fight him, you will need proper armor and weaponary. Perhaps some people in this realm can help you with that";
						break;
					case 3:
						chatIndex++;
						Main.npcChatText = "My quest for you is to go into Lenezald's layer and defeat him; be warned Lenezald is one of the most powerful monks";
						break;
					case 4:
						Main.npcChatText = "Lenezald is so powerful that his layer exists in a realm of his own making. Beware";
						chatIndex = 0;
						break;
				}
			} else {
				if (!hasGottenLenezald) {
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, ModContent.ItemType<InsigniaOfLenezald>());
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, ModContent.ItemType<RenezaldStaff>());
					Main.npcChatText = "You will need these for the fight. Click 'Quest' again when you are ready to go to his lair";
					hasGottenLenezald = true;
				} else {
					SubworldSystem.Enter<LenezaldSubworld>();
				}
					
			}
		}

		public override void SaveData(TagCompound tag) {
			if (hasGottenLenezald) {
				tag["hasGottenLenezald"] = hasGottenLenezald;
			}
		}

		public override void LoadData(TagCompound tag) {
			hasGottenLenezald = tag.GetBool("hasGottenLenezald");
		}
		
	}
}
