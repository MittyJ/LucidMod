
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

/*
    Findings:
    SubworldSystem.isActive() does not enable if subworlds are loading
*/

namespace LucidMod.Content.Systems
{
    public class InventorySaveSystem : ModPlayer
    {

        public static Item[] cachedInventory = new Item[59];
        public static Item[] currentInventory = new Item[59];
        public static Item[] cachedEquipment = new Item[5];
        public static Item[] currentEquipment = new Item[5];
       
        public static Item[] cachedArmor = new Item[20];
        public static Item[] currentArmor = new Item[20];
        



        public static void SwapInventory() {
            Main.LocalPlayer.trashItem = new Item(0);

            for (int i = 0; i < 59; i++) {
                currentInventory[i] = Main.LocalPlayer.inventory[i];
            }
            if (cachedInventory[0] == null) {
                for (int i = 0; i < 59; i++) {
                    cachedInventory[i] = new Item();
                }
            }
            
            for (int i = 0; i < 59; i++) {
                Main.LocalPlayer.inventory[i] = cachedInventory[i];
            }
            for (int i = 0; i < 59; i++) {
                cachedInventory[i] = currentInventory[i];
            }
            ////////////////////////////////////////////////////////////////////
            if (cachedEquipment[0] == null) {
                for (int i = 0; i < 5; i++) {
                    currentEquipment[i] = Main.LocalPlayer.miscEquips[i];
                }
                for (int i = 0; i < 5; i++) {
                    Main.LocalPlayer.miscEquips[i] = new Item(0);
                }
                for (int i = 0; i < 5; i++) {
                    cachedEquipment[i] = currentEquipment[i];
                }
            } else {
                for (int i = 0; i < 5; i++) {
                    currentEquipment[i] = Main.LocalPlayer.miscEquips[i];
                }
                for (int i = 0; i < 5; i++) {
                    Main.LocalPlayer.miscEquips[i] = new Item(cachedEquipment[i].type);
                }
                for (int i = 0; i < 5; i++) {
                    cachedEquipment[i] = currentEquipment[i];
                }
            }
            /////////////////////////////////////////////////////////////////
            if (cachedArmor[0] == null) {
                for (int i = 0; i < 20; i++) {
                    currentArmor[i] = Main.LocalPlayer.armor[i];
                }
                for (int i = 0; i < 20; i++) {
                    Main.LocalPlayer.armor[i] = new Item(0);
                }
                for (int i = 0; i < 20; i++) {
                    cachedArmor[i] = currentArmor[i];
                }
            } else {
                for (int i = 0; i < 20; i++) {
                    currentArmor[i] = Main.LocalPlayer.armor[i];
                }
                for (int i = 0; i < 20; i++) {
                    Main.LocalPlayer.armor[i] = new Item(cachedArmor[i].type);
                }
                for (int i = 0; i < 20; i++) {
                    cachedArmor[i] = currentArmor[i];
                }
            }

            
            
            
        }


        public override void SaveData(TagCompound tag)
            {
                tag["CachedInv"] = cachedInventory.ToList();
                //tag["CachedArmor"] = cachedArmor;
            }

        public override void LoadData(TagCompound tag)
            {
                cachedInventory = tag.GetList<Item>("CachedInv").ToArray();
                //cachedArmor = tag.Get<Item[]>("CachedArmor");
            }
    }
}