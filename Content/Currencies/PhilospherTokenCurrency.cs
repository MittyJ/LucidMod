
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;

namespace LucidMod.Content.Currencies
{
    public class PhilospherTokenCurrency : CustomCurrencySingleCoin
    {
        public PhilospherTokenCurrency(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap) {
			this.CurrencyTextKey = CurrencyTextKey;
			CurrencyTextColor = Color.BlueViolet;
		}
    }
}