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
    public class Trex : IGameEntity
    {
        //constants
        private const int TREX_IDLE_SPRITE_POS_X = 40;
        private const int TREX_IDLE_SPRITE_POS_Y = 0;
        
        public const int TREX_DEFAULT_SPRITE_POS_X = 848;
        public const int TREX_DEFAULT_SPRITE_POS_Y = 0;
        public const int TREX_DEFAULT_SPRITE_POS_WIDTH = 44;
        public const int TREX_DEFAULT_SPRITE_POS_HEIGHT = 52;

        private const float BLINK_ANIMATION_RANDOM_MIN = 2f;
        private const float BLINK_ANIMATION_RANDOM_MAX = 10f;
        private const float BLINK_ANIMATION_EYE_CLOSE_TIME =0.5f;

        //fields
        private Sprite _idleBackgroundTrex;
        private Sprite _idleSprite;
        private Sprite _idleBlinkSprite;
        private SpriteAnimation _blinkAnimation;
        private Random _random;

        //properties
        public TrexState State { get; private set; }
        public Vector2 Position { get; set; }
        public bool isAlive { get; private set; }
        public float Speed { get; private set; }
        public int DrawOrder { get; set; }

        //constructor
        public Trex(Texture2D spriteSheet, Vector2 position)
        {
            Position = position;

            _idleBackgroundTrex = new Sprite(spriteSheet, TREX_IDLE_SPRITE_POS_X, TREX_IDLE_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT);

            State = TrexState.Idle;

            _random = new Random();
            _idleSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT);
            _idleBlinkSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT);

            CreateBlinkAnimation();
            _blinkAnimation.Play();
        }

        //draw method
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(State == TrexState.Idle)
            {
                _idleBackgroundTrex.Draw(spriteBatch, Position);
                _blinkAnimation.Draw(spriteBatch, Position);
            }

            
        }

        //update method
        public void Update(GameTime gameTime)
        {
            if(State == TrexState.Idle)
            {


                if (!_blinkAnimation.isPlaying)
                {
                    CreateBlinkAnimation();
                    _blinkAnimation.Play();
                }                

                _blinkAnimation.Update(gameTime);

            }
        }

        //blink animation frame manager
        private void CreateBlinkAnimation()
        {
            _blinkAnimation = new SpriteAnimation();
            //introduces randomness by calculating blink time as a value between 2 and 10 seconds
            double blinkTimeStamp = _random.NextDouble() * (BLINK_ANIMATION_RANDOM_MAX - BLINK_ANIMATION_RANDOM_MIN) + BLINK_ANIMATION_RANDOM_MIN;

            //do not loop so that upon animation end a new instance is created with a new random interval
            _blinkAnimation.ShouldLoop = false;

            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlinkSprite, (float)blinkTimeStamp);
            //tells when to loop
            _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimeStamp + BLINK_ANIMATION_EYE_CLOSE_TIME);
           
        }
    }
}
