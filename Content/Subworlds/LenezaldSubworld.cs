using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucidMod.Content.Systems;
using SubworldLibrary;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace LucidMod.Content.Subworlds
{
    public class LenezaldSubworld : Subworld
    {
        public override int Width => 280;
	public override int Height => 280;

    public const int PIXELS_IN_BLOCK = 16;


	public override bool ShouldSave => false;
	public override bool NoPlayerSaving => false;

	public override List<GenPass> Tasks => new() { new PassLegacy("Subworld", SubworldGeneration) };
        private void SubworldGeneration(GenerationProgress progress, GameConfiguration configuration)
        {
            Main.worldSurface = 600.0;
            progress.Message = "Entering World"; // Sets the text displayed for this pass
			Main.rockLayer = 0; // Hides the cavern layer way out of bounds
            SubworldSystem.hideUnderworld = true;   
            StructureHelper.Generator.GenerateStructure("Content/Structures/LenezaldArena", new Point16(40, 0), LucidMod.Instance);
        }

		// Sets the time to the middle of the day whenever the subworld loads
		public override void OnLoad()
		{
			Main.dayTime = true;
			Main.time = 27000;
        
		}

        public override void OnEnter()
        {
            Main.LocalPlayer.GetModPlayer<InventorySaveSystem>().SwapInventory();
            SubworldSystem.noReturn = true;
        }


        public override void OnExit()
        {
			Main.LocalPlayer.GetModPlayer<InventorySaveSystem>().SwapInventory();
        }
    }
}