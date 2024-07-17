
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Items;
using SubworldLibrary;
using LucidMod.Content.Subworlds;
using LucidMod.Content.Systems;
using Terraria.DataStructures;
using System.Formats.Asn1;
using Terraria.ModLoader.IO;

namespace LucidMod.NPCs
{
	[AutoloadHead]
	public class Ponderer : ModNPC
	{
		bool equipmentGiven = false;
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
				"Fentazald",
			};
		}

		public override string GetChat() {
			if (SubworldSystem.IsActive<LucidSubworld>()) {
				return "This is the Lucid Jungle. You can reach it when sufficently enlightened. To help you survive, I have some gear you can equip";
			}
			return "I am the greatest Monk and I will be your guide";
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			if (Condition.DownedKingSlime.IsMet()) {
				return true;
			} else {
				return false;
			}
		}



		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			
			if (SubworldSystem.IsActive<LucidSubworld>()) {
				button = "Equip";
			} else {
				button = "Ponder";
			}

		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (!firstButton) {
				return;
			}
			if (SubworldSystem.IsActive<LucidSubworld>()) {
				if (!equipmentGiven) {
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, ModContent.ItemType<MonasticStaff>());
					Item.NewItem(new EntitySource_Misc("Quest"), NPC.Center, 3097);
					//Shield of Cuthulu
					equipmentGiven = true;
				} else {
					Main.npcChatText = "I have already given you my equipment";
				}
				
			} else {
				SubworldSystem.Enter<LucidSubworld>();	
			}
		}

		public override void SaveData(TagCompound tag) {
			if (equipmentGiven) {
				// Note that at this point it may have less than 10 stolen items, if another mod or part of our decides to save the NPC
				tag["equipmentGiven"] = equipmentGiven;
			}
		}

		public override void LoadData(TagCompound tag) {
			equipmentGiven = tag.GetBool("equipmentGiven");
		}
		
	}
}