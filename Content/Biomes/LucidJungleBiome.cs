
using LucidMod.Content.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace LucidMod.Content.Biomes
{
    public class LucidJungleBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;


        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/LucidJungleMusic");

        public override bool IsBiomeActive(Player player) {
			// First, we will use the exampleBlockCount from our added ModSystem for our first custom condition
			bool isActive = SubworldSystem.IsActive<LucidSubworld>();
			return isActive;
		}

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }
}