using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LucidMod.Content.DamageClasses;
using Terraria.DataStructures;

namespace LucidMod.Items
{
	[AutoloadEquip(EquipType.Wings)]
	public class InsigniaOfRenezald : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.LucidMod.hjson file.
		public static readonly int MonasticDamageModifier = 15;


		public override void SetStaticDefaults() {
			// These wings use the same values as the solar wings
			// Fly time: 180 ticks = 3 seconds
			// Fly speed: 9
			// Acceleration multiplier: 2.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(140, 9f, 2.5f);
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(ModContent.GetInstance<MonasticDamage>()) *= 1 + MonasticDamageModifier / 100f;
			player.maxRunSpeed *= 1.75f;
			player.runAcceleration *= 1.1f;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
			ascentWhenFalling = 0.85f; // Falling glide speed
			ascentWhenRising = 0.15f; // Rising speed
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		
	}
}