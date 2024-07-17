using LucidMod.Content.Currencies;
using LucidMod.Items;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace LucidMod
{
	public class LucidMod : Mod
	{
		public static int PhilospherTokenId;

        public static LucidMod Instance { get; set; }

        public LucidMod() => Instance = this;
        public override void Load()
        {
            PhilospherTokenId = CustomCurrencyManager.RegisterCurrency(new PhilospherTokenCurrency(ModContent.ItemType<PhilosopherToken>(), 999, "Philospher Token"));
        }

    }
}