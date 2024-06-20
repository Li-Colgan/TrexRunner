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
    internal class Star : SkyObject
    {
        private const int STAR_SPRITE_X = 644;
        private const int STAR_SPRITE_Y = 2;
        private const int STAR_SPRITE_WIDTH = 9;
        private const int STAR_SPRITE_HEIGHT = 9;
        private const float ANIMATION_FRAME_LENGTH = 0.4f;
        private const int ANIMATION_NUMBER_OF_FRAMES = 3;
        SpriteAnimation _animation;
        private IDayNightCycle _dayNightCycle;


        public override float Speed => _trex.Speed * 0.2f;

        public Star(IDayNightCycle dayNightCycle, Texture2D spriteSheet, Trex trex, Vector2 position) : base(trex, position)
        {
            _dayNightCycle = dayNightCycle;
            _animation = SpriteAnimation.CreateSimpleAnimation
            (
                spriteSheet,
                new Point( STAR_SPRITE_X, STAR_SPRITE_Y), 
                STAR_SPRITE_WIDTH, 
                STAR_SPRITE_HEIGHT,
                new Point(0, STAR_SPRITE_HEIGHT),
                ANIMATION_NUMBER_OF_FRAMES,
                ANIMATION_FRAME_LENGTH
            );
            _animation.ShouldLoop = true;
            _animation.Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(_trex.IsAlive)
                _animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(_dayNightCycle.IsNight)
                _animation.Draw(spriteBatch, Position);
        }
    }
}
