using System.Collections.Generic;
using SubworldLibrary;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using LucidMod.Content.Systems;
using Terraria.ModLoader;
using LucidMod.NPCs;
using Terraria.DataStructures;



namespace LucidMod.Content.Subworlds
{
    public class LucidSubworld : Subworld
{

	public override int Width => 2600;
	public override int Height => 600;

    public const int PIXELS_IN_BLOCK = 16;


	public override bool ShouldSave => true;
	public override bool NoPlayerSaving => false;

	public override List<GenPass> Tasks => new() { new PassLegacy("Subworld", SubworldGeneration) };
        private void SubworldGeneration(GenerationProgress progress, GameConfiguration configuration)
        {
            Main.worldSurface = 600.0;
            progress.Message = "Entering World"; // Sets the text displayed for this pass
			Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            SubworldSystem.hideUnderworld = true;   
            StructureHelper.Generator.GenerateStructure("Content/Structures/LucidSubworld4", new Point16(75, 45), LucidMod.Instance);
            NPC.NewNPC(new EntitySource_SpawnNPC(), 210 * PIXELS_IN_BLOCK, 235 * PIXELS_IN_BLOCK, ModContent.NPCType<SoulHunter>());
            NPC.NewNPC(new EntitySource_SpawnNPC(), 2515 * PIXELS_IN_BLOCK, 265 * PIXELS_IN_BLOCK, ModContent.NPCType<Abbot>());
            NPC.NewNPC(new EntitySource_SpawnNPC(), 1270 * PIXELS_IN_BLOCK, 70 * PIXELS_IN_BLOCK, ModContent.NPCType<OldPhilosopher>());
            NPC.NewNPC(new EntitySource_SpawnNPC(), 1300 * PIXELS_IN_BLOCK, 290 * PIXELS_IN_BLOCK, ModContent.NPCType<Ponderer>());
        }

		// Sets the time to the middle of the day whenever the subworld loads
		public override void OnLoad()
		{
			Main.dayTime = true;
			Main.time = 0;
        
		}

        public override void OnEnter()
        {
            SubworldSystem.noReturn = true;
            Main.LocalPlayer.GetModPlayer<InventorySaveSystem>().SwapInventory();
        }


        public override void OnExit()
        {
			Main.LocalPlayer.GetModPlayer<InventorySaveSystem>().SwapInventory();
        }

		

    }

}