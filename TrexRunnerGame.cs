using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TrexRunner.Content.System;
using TrexRunner.Entities;
using TrexRunner.Graphics;

namespace TrexRunner
{
    public class TrexRunnerGame : Game
    {
        //consts
        private const string ASSET_NAME_SPRITESHEET = "TrexSpritesheet";
        private const string ASSET_NAME_SFX_HIT = "hit";
        private const string ASSET_NAME_SFX_SCORE_REACHED = "score-reached";
        private const string ASSET_NAME_SFX_BUTTON_PRESS = "button-press";

        public const int WINDOW_WIDTH = 600;
        public const int WINDOW_HEIGHT = 150;

        public const int TREX_START_POS_Y = WINDOW_HEIGHT - 16 - Trex.TREX_DEFAULT_SPRITE_POS_HEIGHT;
        public const int TREX_START_POS_X = 1;
        private const float FADE_IN_ANIMATION_SPEED = 820f;
        private const int SCORE_BOARD_POS_X = WINDOW_WIDTH - 130;
        private const int SCORE_BOARD_POS_Y = 10;

        //fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

      
        private SoundEffect _sfxHit;
        private SoundEffect _sfxScoreReached;
        private SoundEffect _sfxButtonPress;
        private Texture2D _spriteSheetTexture;
        private Texture2D _fadeInTexture;
        private float _fadeInTexturePosX;

        private Trex _trex;
        private ScoreBoard _scoreBoard;
        private InputController _inputController;

        private EntityManager _entityManager;

        private GroundManager _groundManager;

        private KeyboardState _previousKeyboardState;

        //props
        public GameState State { get; private set; }


        public TrexRunnerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _entityManager = new EntityManager();
            State = GameState.Initial;
            _fadeInTexturePosX = Trex.TREX_DEFAULT_SPRITE_POS_WIDTH;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //load sfx
            _sfxHit = Content.Load<SoundEffect>(ASSET_NAME_SFX_HIT);
            _sfxButtonPress = Content.Load<SoundEffect>(ASSET_NAME_SFX_BUTTON_PRESS);
            _sfxScoreReached = Content.Load<SoundEffect>(ASSET_NAME_SFX_SCORE_REACHED);

            //load spritesheet
            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);

            _fadeInTexture = new Texture2D(GraphicsDevice, 1, 1);
            _fadeInTexture.SetData(new Color[] { Color.White });

            //load trex
            _trex = new Trex(_spriteSheetTexture, new Vector2(TREX_START_POS_X, TREX_START_POS_Y), _sfxButtonPress);
            _trex.DrawOrder = 10;
            _trex.JumpComplete += trex_JumpComplete;

            _scoreBoard = new ScoreBoard(_spriteSheetTexture, new Vector2(SCORE_BOARD_POS_X, SCORE_BOARD_POS_Y), _trex);
            //_scoreBoard.Score = 498;
            //_scoreBoard.HighScore = 12345;

            _inputController = new InputController(_trex);

            _groundManager = new GroundManager(_spriteSheetTexture, _entityManager, _trex);

            _entityManager.AddEntity(_trex);
            _entityManager.AddEntity(_groundManager);
            _entityManager.AddEntity(_scoreBoard);
            _groundManager.Initialise();
        }

        private void trex_JumpComplete(object sender, EventArgs e)
        {
            if(State == GameState.Transition)
            {
                State = GameState.Playing;
                _trex.Initialise();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();

            if (State == GameState.Playing)
                _inputController.ProcessControls(gameTime);
            else if (State == GameState.Transition)
                _fadeInTexturePosX += (float)gameTime.ElapsedGameTime.TotalSeconds * FADE_IN_ANIMATION_SPEED;
            else if (State == GameState.Initial)
            {
                bool isStartKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasStartKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);
            
                if (isStartKeyPressed && !wasStartKeyPressed)
                {
                    StartGame();
                }
            }
            _entityManager.Update(gameTime);

            _previousKeyboardState = keyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _entityManager.Draw(gameTime, _spriteBatch);

            if(State == GameState.Initial || State == GameState.Transition)
            {
                _spriteBatch.Draw(_fadeInTexture, new Rectangle((int)Math.Round(_fadeInTexturePosX), 0, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool StartGame()
        {
            if (State != GameState.Initial)
                return false;
            State = GameState.Transition;
            _trex.BeginJump();
            return true;

        }
    }
}