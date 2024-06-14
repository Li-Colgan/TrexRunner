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
    public class Trex : IGameEntity
    {
        private const int TREX_IDLE_SPRITE_POS_X = 40;
        private const int TREX_IDLE_SPRITE_POS_Y = 0;
        

        public const int TREX_DEFAULT_SPRITE_POS_X = 848;
        public const int TREX_DEFAULT_SPRITE_POS_Y = 0;
        public const int TREX_DEFAULT_SPRITE_POS_WIDTH = 44;
        public const int TREX_DEFAULT_SPRITE_POS_HEIGHT = 52;

        private Sprite _idleBackgroundTrex;
        private SpriteAnimation _idleAnimation;

        public TrexState State { get; private set; }
        public Vector2 Position { get; set; }
        public bool isAlive { get; private set; }
        public float Speed { get; private set; }
        public int DrawOrder { get; set; }

        public Trex(Texture2D spriteSheet, Vector2 position)
        {
            Position = position;

            _idleBackgroundTrex = new Sprite(spriteSheet, TREX_IDLE_SPRITE_POS_X, TREX_IDLE_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT);

            State = TrexState.Idle;

            _idleAnimation = new SpriteAnimation();
            _idleAnimation.AddFrame(new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT), 0);
            _idleAnimation.AddFrame(new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT), 1/20f);
            //tells when to loop
            _idleAnimation.AddFrame(new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT), 1/20f * 2);
            _idleAnimation.Play();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(State == TrexState.Idle)
            {
                _idleBackgroundTrex.Draw(spriteBatch, Position);
                _idleAnimation.Draw(spriteBatch, Position);
            }

            
        }

        public void Update(GameTime gameTime)
        {
            if(State == TrexState.Idle)
            {
                _idleAnimation.Update(gameTime);
            }
        }
    }
}
