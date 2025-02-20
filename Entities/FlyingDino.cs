﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    public class FlyingDino : Obstacle
    {
        private const int TEXTURE_COORDS_X = 134;
        private const int TEXTURE_COORDS_Y = 0;
        private const int TEXTURE_COORDS_WIDTH = 46;
        private const int TEXTURE_COORDS_HEIGHT = 42;
        private const float ANIMATION_FRAME_LENGTH = 0.2f;
        private SpriteAnimation _animation;
        private const int VERTICAL_COLLISION_INSET = 10;
        private const int HORIZONTAL_COLLISION_INSET = 6;
        private const float SPEED_PPS = 80f;

        private Trex _trex;
        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle collisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), TEXTURE_COORDS_WIDTH, TEXTURE_COORDS_HEIGHT);
                collisionBox.Inflate(-VERTICAL_COLLISION_INSET, -HORIZONTAL_COLLISION_INSET);
                return collisionBox;
            }
        }

        public FlyingDino(Trex trex, Vector2 position, Texture2D spriteSheet) : base(trex, position)
        {
            _trex = trex;
            Sprite spriteA = new Sprite(spriteSheet, TEXTURE_COORDS_X, TEXTURE_COORDS_Y, TEXTURE_COORDS_WIDTH, TEXTURE_COORDS_HEIGHT);
            Sprite spriteB = new Sprite(spriteSheet, TEXTURE_COORDS_X + TEXTURE_COORDS_WIDTH, TEXTURE_COORDS_Y, TEXTURE_COORDS_WIDTH, TEXTURE_COORDS_HEIGHT);

            _animation = new SpriteAnimation();
            _animation.AddFrame(spriteA, 0);
            _animation.AddFrame(spriteB, ANIMATION_FRAME_LENGTH);
            _animation.AddFrame(spriteA, ANIMATION_FRAME_LENGTH * 2);
            _animation.ShouldLoop = true;
            _animation.Play();
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animation.Draw(spriteBatch, Position);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(_trex.IsAlive)
            {
                _animation.Update(gameTime);
                Position = new Vector2(Position.X - SPEED_PPS * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            }
        }
    }
}
