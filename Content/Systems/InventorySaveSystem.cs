
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
        // Keep in mind that each player has a ModPlayer instance, so none of this should've have been static in the first place
        Item[] cachedInventory;
        Item[] currentInventory;
        Item[] cachedEquipment;
        Item[] currentEquipment;
       
        Item[] cachedArmor;
        Item[] currentArmor;
        
        public void SwapInventory() {
            Player.trashItem = new Item();

            // NOTE: CacheAndSwap is later in this file
            CacheAndSwap(Player.inventory, ref currentInventory, ref cachedInventory);

            // CacheAndSwap(Player.miscEquips, ref currentEquipment, ref cachedEquipment);

            // CacheAndSwap(Player.armor, ref currentArmor, ref cachedArmor);
        }

        // New method to simplify code structure
        private static void CacheAndSwap(Item[] inventory, ref Item[] current, ref Item[] cache) {
            current ??= new Item[inventory.Length];

            // Store the current inventory for later
            for (int i = 0; i < inventory.Length; i++)
                current[i] = inventory[i];

            // Initialize the cache or restore it to the main inventory
            if (cache is null) {
                cache = new Item[inventory.Length];

                for (int i = 0; i < inventory.Length; i++) {
                    inventory[i] = new Item();
                    cache[i] = current[i];
                }
            } else {
                for (int i = 0; i < inventory.Length; i++) {
                    inventory[i] = cache[i];
                    cache[i] = current[i];
                }
            }
        }


        public override void SaveData(TagCompound tag)
        {
            tag["CachedInv"] = cachedInventory;
            // tag["CachedArmor"] = cachedArmor;
            // tag["CachedEquips"] = cachedEquipment;
        }

        public override void LoadData(TagCompound tag)
        {
            cachedInventory = tag.Get<Item[]>("CachedInv");
            // cachedArmor = tag.Get<Item[]>("CachedArmor");
            // cachedEquipment = tag.Get<Item[]>("CachedEquips");
        }
    }
}