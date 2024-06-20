using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    public class Cloud : SkyObject
    {
        private const int CLOUD_SPRITE_X = 87;
        private const int CLOUD_SPRITE_Y = 0;
        private const int CLOUD_SPRITE_WIDTH = 46;
        private const int CLOUD_SPRITE_HEIGHT = 17;
        private Sprite _sprite;
        public override float Speed => _trex.Speed * 0.5f;
        public Cloud(Texture2D spriteSheet, Trex trex, Vector2 position) : base(trex, position)
        {
            _sprite = new Sprite(spriteSheet, CLOUD_SPRITE_X, CLOUD_SPRITE_Y, CLOUD_SPRITE_WIDTH, CLOUD_SPRITE_HEIGHT);
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite.Draw(spriteBatch, Position);
        }
    }
}
