using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        private const float JUMP_START_VELOCITY = -480f;
        private const float GRAVITY = 1600f;
        private const float CANCEL_JUMP_VELOCITY = -100f;
        private const float MIN_JUMP_HEIGHT = 40f;

        private const int TREX_RUNNING_SPRITE_ONE_POS_X = TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_POS_WIDTH * 2;
        private const int TREX_RUNNING_SPRITE_ONE_POS_Y = 0;

        private const float RUN_ANIMATION_FRAME_LENGTH = 1 / 10f;

        public const int TREX_DUCKING_SPRITE_ONE_POS_X = TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_POS_WIDTH * 6;
        public const int TREX_DUCKING_SPRITE_ONE_POS_Y = 0;
        public const int TREX_DUCKING_SPRITE_ONE_POS_WIDTH = 59;
        private const float DROP_VELOCITY = 600f;



        //fields
        private Sprite _idleBackgroundTrex;
        private Sprite _idleSprite;
        private Sprite _idleBlinkSprite;
        private SpriteAnimation _blinkAnimation;
        private Random _random;
        private SoundEffect _JumpSound;
        private float _verticalVelocity;
        private float _startPosY;
        private SpriteAnimation _runAnimation;
        private SpriteAnimation _duckAnimation;
        private float _dropVelocity;


        //properties
        public TrexState State { get; private set; }
        public Vector2 Position { get; set; }
        public bool isAlive { get; private set; }
        public float Speed { get; private set; }
        public int DrawOrder { get; set; }

        

        //constructor
        public Trex(Texture2D spriteSheet, Vector2 position, SoundEffect jumpSound)
        {
            Position = position;

            _idleBackgroundTrex = new Sprite(spriteSheet, TREX_IDLE_SPRITE_POS_X, TREX_IDLE_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT);

            State = TrexState.Idle;

            _JumpSound = jumpSound;

            _random = new Random();
            _idleSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT);
            _idleBlinkSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT);
            _blinkAnimation = new SpriteAnimation();
            CreateBlinkAnimation();
            _blinkAnimation.Play();

            _startPosY = position.Y;

            _runAnimation = new SpriteAnimation();
            _runAnimation.AddFrame(new Sprite(spriteSheet, TREX_RUNNING_SPRITE_ONE_POS_X, TREX_RUNNING_SPRITE_ONE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT), 0);
            _runAnimation.AddFrame(new Sprite(spriteSheet, TREX_RUNNING_SPRITE_ONE_POS_X + TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_RUNNING_SPRITE_ONE_POS_Y, TREX_DEFAULT_SPRITE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT), RUN_ANIMATION_FRAME_LENGTH);
            _runAnimation.AddFrame(_runAnimation[0].Sprite, RUN_ANIMATION_FRAME_LENGTH * 2);
            _runAnimation.Play();

            _duckAnimation = new SpriteAnimation();
            _duckAnimation.AddFrame(new Sprite(spriteSheet, TREX_DUCKING_SPRITE_ONE_POS_X, TREX_DUCKING_SPRITE_ONE_POS_Y, TREX_DUCKING_SPRITE_ONE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT), 0);
            _duckAnimation.AddFrame(new Sprite(spriteSheet, TREX_DUCKING_SPRITE_ONE_POS_X + TREX_DUCKING_SPRITE_ONE_POS_WIDTH, TREX_DUCKING_SPRITE_ONE_POS_Y, TREX_DUCKING_SPRITE_ONE_POS_WIDTH, TREX_DEFAULT_SPRITE_POS_HEIGHT), RUN_ANIMATION_FRAME_LENGTH);
            _duckAnimation.AddFrame(_duckAnimation[0].Sprite, RUN_ANIMATION_FRAME_LENGTH * 2);
            _duckAnimation.Play();
        }

        //draw method
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(State == TrexState.Idle)
            {
                _idleBackgroundTrex.Draw(spriteBatch, Position);
                _blinkAnimation.Draw(spriteBatch, Position);
            }
            else if(State == TrexState.Jumping || State == TrexState.Falling)
            {
                _idleSprite.Draw(spriteBatch, Position);
            }
            else if (State == TrexState.Running)
            {
                _runAnimation.Draw(spriteBatch, Position);
            }
            else if (State == TrexState.Ducking)
            {
                _duckAnimation.Draw(spriteBatch, Position);
            }

        }

        //update method
        public void Update(GameTime gameTime)
        {
            if (State == TrexState.Idle)
            {


                if (!_blinkAnimation.isPlaying)
                {
                    CreateBlinkAnimation();
                    _blinkAnimation.Play();
                }

                _blinkAnimation.Update(gameTime);

            }
            else if (State == TrexState.Jumping || State == TrexState.Falling)
            {
                Position = new Vector2(Position.X, Position.Y + _verticalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds + _dropVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                _verticalVelocity += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_verticalVelocity >= 0)
                    State = TrexState.Falling;

                if (Position.Y >= _startPosY)
                {
                    Position = new Vector2(Position.X, _startPosY);
                    _verticalVelocity = 0;
                    State = TrexState.Running;
                }
            }
            else if (State == TrexState.Running)
            {
                _runAnimation.Update(gameTime);
            }
            else if (State == TrexState.Ducking)
            {
                _duckAnimation.Update(gameTime);
            }
            _dropVelocity = 0;
            
        }

        //blink animation frame manager
        private void CreateBlinkAnimation()
        {
            _blinkAnimation.Clear();
            //introduces randomness by calculating blink time as a value between 2 and 10 seconds
            double blinkTimeStamp = _random.NextDouble() * (BLINK_ANIMATION_RANDOM_MAX - BLINK_ANIMATION_RANDOM_MIN) + BLINK_ANIMATION_RANDOM_MIN;

            //do not loop so that upon animation end a new instance is created with a new random interval
            _blinkAnimation.ShouldLoop = false;

            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlinkSprite, (float)blinkTimeStamp);
            //tells when to loop
            _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimeStamp + BLINK_ANIMATION_EYE_CLOSE_TIME);
           
        }

        //jump method
        public bool BeginJump()
        {
            if (State == TrexState.Jumping || State == TrexState.Falling)
                return false;
            _JumpSound.Play();
            State = TrexState.Jumping;
            _verticalVelocity = JUMP_START_VELOCITY;
            return true;
        }

        public bool CancelJump()
        {
            if (State != TrexState.Jumping || (_startPosY - Position.Y) < MIN_JUMP_HEIGHT)
                return false;
            _verticalVelocity = _verticalVelocity < CANCEL_JUMP_VELOCITY ? CANCEL_JUMP_VELOCITY : 0;
            return true;
        }

        public bool Duck()
        {
            if (State == TrexState.Jumping || State == TrexState.Falling)
                return false;
            State = TrexState.Ducking;
            return true;
        }

        public bool CancelDuck()
        {
            if (State != TrexState.Ducking)
            {
                return false;
            }
            State = TrexState.Running;
            return true;
        }

        public bool Drop()
        {
            if (State != TrexState.Falling && State != TrexState.Jumping)
                return false;
            State = TrexState.Falling;
            _dropVelocity = DROP_VELOCITY;
            return true;

        }
    }
}
