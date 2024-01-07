using System.Collections.Generic;
using SubworldLibrary;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using LucidMod.Content.Systems;
using System;



namespace LucidMod.Content.Subworlds
{
    public class LucidSubworld : Subworld
{

	public override int Width => 8400;
	public override int Height => 1000;

	public InventorySaveSystem inventorySaveSystem = new InventorySaveSystem();

	public override bool ShouldSave => true;
	public override bool NoPlayerSaving => false;

	public override List<GenPass> Tasks => new() { new PassLegacy("Subworld", SubworldGeneration) };
        private void SubworldGeneration(GenerationProgress progress, GameConfiguration configuration)
        {
            Main.worldSurface = 600.0;
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
			 Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            SubworldSystem.hideUnderworld = true;
			StructureHelper.Generator.GenerateStructure("Content/Structures/LucidJungle", new Terraria.DataStructures.Point16(0, 42), LucidMod.Instance);
        
        }

		// Sets the time to the middle of the day whenever the subworld loads
		public override void OnLoad()
		{
			Main.dayTime = true;
			Main.time = 27000;
			
		}

        public override void OnEnter()
        {
            inventorySaveSystem.SwapInventory();
        }


        public override void OnExit()
        {
			inventorySaveSystem.SwapInventory();
        }

		

    }

}