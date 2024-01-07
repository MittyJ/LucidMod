
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Items;
using SubworldLibrary;
using LucidMod.Content.Subworlds;
using LucidMod.Content.Systems;

namespace LucidMod.NPCs
{
	[AutoloadHead]
	public class Ponderer : ModNPC
	{

		InventorySaveSystem inventorySaveSystem = new InventorySaveSystem();
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1; // The total amount of frames the NPC has
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
				"Renezald",
				"Chenezald",
				"Fentazald",
				"Menezald"
			};
		}

		public override string GetChat() {
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
			button = "Shop";
			button2 = "Ponder";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = "Shop";
			} else {
				SubworldSystem.Enter<LucidSubworld>();	
				}
			}

			

		public override void AddShops() {
			var npcShop = new NPCShop(Type, "Shop")
				.Add<Tingler>()
				.Add<Zald>();
				
				npcShop.Add(new Item(ModContent.ItemType<InsigniaOfRenezald>()) {
					shopCustomPrice = 2,
					shopSpecialCurrency = LucidMod.PhilospherTokenId
				});


				npcShop.Register();
		}
			

		public override void ModifyActiveShop(string shopName, Item[] items) {
			foreach (Item item in items) {
				// Skip 'air' items and null items.
				if (item == null || item.type == ItemID.None) {
					continue;
				}
			}
		}

		
	}
}