using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    public class CactusGroup : Obstacle
    {
        public enum GroupSize
        {
            Small = 0,
            Medium = 1,
            Large = 2
        }
        public const int SMALL_CACTUS_SPRITE_HEIGHT = 36;
        public const int SMALL_CACTUS_SPRITE_WIDTH = 17;
        public const int SMALL_CACTUS_SPRITE_X = 228;
        public const int SMALL_CACTUS_SPRITE_Y = 0;
        public const int LARGE_CACTUS_SPRITE_HEIGHT = 51;
        public const int LARGE_CACTUS_SPRITE_WIDTH = 25;
        public const int LARGE_CACTUS_SPRITE_X = 332;
        public const int LARGE_CACTUS_SPRITE_Y = 0;
        private const int COLLISION_BOX_INSET = 8;

        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Sprite.Width, Sprite.Height);
                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return box;
            }
        }
        public bool IsLarge { get; }
        public GroupSize Size { get; }
        public Sprite Sprite { get; private set; }

        public CactusGroup(Texture2D spriteSheet, bool isLarge, GroupSize size, Trex trex, Vector2 position) : base(trex, position)
        {
            IsLarge = isLarge;
            Size = size;
            Sprite = GenerateSprite(spriteSheet);

        }

        private Sprite GenerateSprite(Texture2D spriteSheet)
        {
            Sprite sprite = null;
            int spriteWidth = 0;
            int spriteHeight = 0;
            int spriteX = 0;
            int spriteY = 0;

            if (!IsLarge) //create small group
            {
                spriteWidth = SMALL_CACTUS_SPRITE_WIDTH;
                spriteHeight = SMALL_CACTUS_SPRITE_HEIGHT;
                spriteX = SMALL_CACTUS_SPRITE_X;
                spriteY = SMALL_CACTUS_SPRITE_Y;
            }
            else //create large group
            {
                spriteWidth = LARGE_CACTUS_SPRITE_WIDTH;
                spriteHeight = LARGE_CACTUS_SPRITE_HEIGHT;
                spriteX = LARGE_CACTUS_SPRITE_X;
                spriteY = LARGE_CACTUS_SPRITE_Y;

            }
            int offsetX = 0;
            int width = spriteWidth;

            if (Size == GroupSize.Small)
            {
                offsetX = 0;
            }
            if (Size == GroupSize.Medium)
            {
                offsetX = 1;
                width = spriteWidth * 2;
            }
            else
            {
                offsetX = 3;
                width = spriteWidth * 3;
            }
            sprite = new Sprite
            (
                spriteSheet, 
                spriteX + offsetX * spriteWidth, 
                spriteY, width, 
                spriteHeight
            );


            return sprite;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, Position);
        }
    }
}
