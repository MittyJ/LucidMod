
using LucidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LucidMod.Items
{
    public class MonasticStaff : ModItem
    {
        public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = ModContent.GetInstance<MonasticDamage>();
			Item.useTime = 10;
			Item.width = 42;
			Item.height = 40;
			Item.scale = 2;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 10;
			Item.value = 0;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox) {
			noHitbox = false;
			hitbox = new Rectangle(hitbox.X, hitbox.Y, 84, 80);
		}
    }
}