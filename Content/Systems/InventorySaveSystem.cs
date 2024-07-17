using System;
using System.Linq;
using LucidMod.Content.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using LucidMod;

/*
    Findings:
    SubworldSystem.isActive() does not enable if subworlds are loading
*/

namespace LucidMod.Content.Systems
{
    public class InventorySaveSystem : ModPlayer
    {

        public static Item[] cachedInventory = new Item[59];
        public static Item[] cachedEquipment = new Item[59];
        public static Item[] currentEquipment = new Item[59];
        public static Item[] currentInventory = new Item[59];
        



        public static void SwapInventory() {
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

            // for (int i = 0; i < 59; i++) {
            //     currentEquipment[i] = Main.LocalPlayer.armor[i];
            // }
            // if (cachedEquipment[0] == null) {
            //     for (int i = 0; i < 59; i++) {
            //         cachedEquipment[i] = new Item();
            //     }
            // }
            
            // for (int i = 0; i < 59; i++) {
            //     Main.LocalPlayer.armor[i] = cachedEquipment[i];
            // }
            // for (int i = 0; i < 59; i++) {
            //     cachedEquipment[i] = currentEquipment[i];
            // }
            
        }


        public override void SaveData(TagCompound tag)
            {
                tag["CachedInv"] = cachedInventory.ToList();
            }

        public override void LoadData(TagCompound tag)
            {
                cachedInventory = tag.GetList<Item>("CachedInv").ToArray();
            }
    }
}