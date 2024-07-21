
using LucidMod.Content.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.Items
{
    public class MirrorOfReturn : ModItem
    {
        public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = 1000; // Makes the item worth 1 gold.
			Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
			Item.useTime = 30;
            Item.autoReuse = false;
		}

        public override bool CanUseItem(Player player) {
			bool equipmentEmpty = true;
				for (int i = 0; i < 20; i++) {
					if (Main.LocalPlayer.armor[i].type != 0) {
						equipmentEmpty = false;
					}
				}
				for (int i = 0; i < 5; i++) {
					if (Main.LocalPlayer.miscEquips[i].type != 0) {
						equipmentEmpty = false;
					}
				}
				if (equipmentEmpty && (SubworldSystem.IsActive<LucidSubworld>() || SubworldSystem.IsActive<LenezaldSubworld>())) {
                    return true;
                } else {
                    Main.NewText("You must remove all armor, equipment, accessories, and vanities before you can return");
                    return false;
                }
		}

        public override bool? UseItem(Player player) {
            
            SubworldSystem.Exit();
            return null;

		}
    }
}