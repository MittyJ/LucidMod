
using LucidMod.Content.Subworlds;
using LucidMod.NPCs;
using SubworldLibrary;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.Items
{
    public class InsigniaOfLenezald : ModItem
    {
        public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = 1000; // Makes the item worth 1 gold.
			Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
			Item.useTime = 30;
            Item.autoReuse = false;
            Item.consumable = true;
		}

        public override bool CanUseItem(Player player) {
            if (NPC.AnyNPCs(ModContent.NPCType<ProjectionOfLenezald>())) {
                return false;
            }
			if (SubworldSystem.IsActive<LenezaldSubworld>()) {
                return true;
            } else {
                Main.NewText("You can only awaken Lenezald in his layer (Talk to the Old Philosopher and accept his quest)");
                return false;
            }
			
		}

        public override bool ConsumeItem(Player player) {
			return false;
		}

        public override bool? UseItem(Player player) {
            NPC.NewNPC(new EntitySource_SpawnNPC(), (int) Main.LocalPlayer.Center.X, (int) Main.LocalPlayer.Center.Y + 300, ModContent.NPCType<ProjectionOfLenezald>());
            return true;
		}
    }
}