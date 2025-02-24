using Macrocosm.Common.Utility;
using Macrocosm.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Macrocosm.Backgrounds
{
    public class MoonSurfaceBgStyle : ModSurfaceBgStyle
	{
		public override bool ChooseBgStyle()
		{
			return !Main.gameMenu && Main.LocalPlayer.GetModPlayer<MacrocosmPlayer>().ZoneMoon;
		}
        // Use this to keep far Backgrounds like the mountains.
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }

        public override int ChooseFarTexture()
        {
            return -1;
        }

        public override int ChooseMiddleTexture()
        {
            return -1;
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return mod.GetBackgroundSlot("Backgrounds/MoonSurfaceMid");
        }
        public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
        {
            float a = 1300f;
            float b = 1750f;
            int[] textureSlots = new int[] {
                mod.GetBackgroundSlot("Backgrounds/MoonSurfaceFar"),
                mod.GetBackgroundSlot("Backgrounds/MoonSurfaceMid"),
                mod.GetBackgroundSlot("Backgrounds/MoonSurfaceNear"),
            };
            int length = textureSlots.Length;
            for (int i = 0; i < textureSlots.Length; i++)
            {
                //Custom: bgScale,textureslot,patallaz,these 2 numbers....,Top and Start?
                float bgParallax = 0.37f + 0.2f - (0.1f * (length - i));
                int textureSlot = textureSlots[i];
                Main.instance.LoadBackground(textureSlot);
                float bgScale = 2.5f;
                int bgW = (int)(Main.backgroundWidth[textureSlot] * bgScale);
                SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / bgParallax);
                float screenOff = typeof(Main).GetFieldValue<float>("screenOff", Main.instance);
                float scAdj = typeof(Main).GetFieldValue<float>("scAdj", Main.instance);
                int bgStart = (int)(-Math.IEEERemainder(Main.screenPosition.X * bgParallax, bgW) - (bgW / 2));
                int bgTop = (int)((-Main.screenPosition.Y + screenOff / 2f) / (Main.worldSurface * 16.0) * a + b) + (int)scAdj - ((length - i) * 200);
                if (Main.gameMenu)
                {
                    bgTop = 320;
                }
                Color backColor = typeof(Main).GetFieldValue<Color>("backColor", Main.instance);
                int bgLoops = Main.screenWidth / bgW + 2;
                if (Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
                {
                    for (int k = 0; k < bgLoops; k++)
                    {
                        Main.spriteBatch.Draw(Main.backgroundTexture[textureSlot],
                            new Vector2(bgStart + bgW * k, bgTop),
                            new Rectangle(0, 0, Main.backgroundWidth[textureSlot], Main.backgroundHeight[textureSlot]),
                            backColor, 0f, default, bgScale, SpriteEffects.None, 0f);
                    }
                }
            }
            return false;
        }

    }
}