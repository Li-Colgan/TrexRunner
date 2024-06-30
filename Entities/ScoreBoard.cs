using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TrexRunner.Entities
{
    public class ScoreBoard : IGameEntity
    {
        //constants
        private const int TEXTURE_COORDS_NUMBER_X = 655;
        private const int TEXTURE_COORDS_NUMBER_Y = 0;
        private const int TEXTURE_COORDS_NUMBER_WIDTH = 10;
        private const int TEXTURE_COORDS_NUMBER_HEIGHT = 13;
        private const byte NUMBER_DIGITS_TO_DRAW = 5;
        private const int SCORE_MARGIN = 70;
        private const int TEXTURE_COORDS_HI_X = 755;
        private const int TEXTURE_COORDS_HI_Y = 0;
        private const int TEXTURE_COORDS_HI_WIDTH = 20;
        private const int TEXTURE_COORDS_HI_HEIGHT = 13;
        private const int HI_TEXT_MARGIN = 28;
        private const float SCORE_INCREMENT_MULTIPLIER = 0.05f;

        private const float FLASH_ANIMATION_FRAME_LENGTH = 0.333f;
        private const int FLASH_ANIMATION_FLASH_COUNT = 4;

        private const int MAX_SCORE = 99_999;


        //fields
        private Texture2D _texture;
        private Trex _trex;

        private bool _isPlayingFlashAnimation;
        private float _flashAnimationTime;
        private SoundEffect _scoreSfx;
        private double _score;

        //properties
        public double Score 
        { 
            get => _score; 
            set => _score = Math.Max(0, Math.Min(value, MAX_SCORE)); 
        }
        public int HighScore { get; set; }
        public bool HasHighScore => HighScore > 0;
        public int DisplayScore => (int)Math.Floor(Score);
        public int DrawOrder => 100;
        public Vector2 Position { get; set; }

        public ScoreBoard(Texture2D texture, Vector2 position, Trex trex, SoundEffect scoreSfx)
        {
            _trex = trex;
            _texture = texture;
            Position = position;
            _scoreSfx = scoreSfx;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (HasHighScore)
            {
                spriteBatch.Draw(_texture, new Vector2(Position.X - HI_TEXT_MARGIN, Position.Y), new Rectangle(TEXTURE_COORDS_HI_X, TEXTURE_COORDS_HI_Y, TEXTURE_COORDS_HI_WIDTH, TEXTURE_COORDS_HI_HEIGHT), Color.White);

                DrawScore(spriteBatch, HighScore, Position.X);
            }
            if (!_isPlayingFlashAnimation || ((int)(_flashAnimationTime / FLASH_ANIMATION_FRAME_LENGTH) % 2 != 0))
            {
                int score = !_isPlayingFlashAnimation ? DisplayScore : (DisplayScore / 100 * 100);
                DrawScore(spriteBatch, score, Position.X + SCORE_MARGIN);
            }
        }

        private void DrawScore(SpriteBatch spriteBatch, int score, float startPosX)
        {
            int[] scoreDigits = SplitDigits(score);

            float posX = startPosX;

            foreach (int digit in scoreDigits)
            {
                Rectangle textureCoords = GetDigitTextureBounds(digit);

                Vector2 screenPos = new Vector2(posX, Position.Y);

                spriteBatch.Draw(_texture, screenPos, textureCoords, Color.White);

                posX += TEXTURE_COORDS_NUMBER_WIDTH;
            }
        }

        public void Update(GameTime gameTime)
        {
            int oldScore = DisplayScore;
            Score += _trex.Speed * SCORE_INCREMENT_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds;

            if (DisplayScore / 100 != oldScore / 100)
            {
                _isPlayingFlashAnimation = true;
                _flashAnimationTime = 0;
                _scoreSfx.Play(0.8f, 0, 0);
            }
            if (_isPlayingFlashAnimation)
            {
                _flashAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_flashAnimationTime >= FLASH_ANIMATION_FRAME_LENGTH * FLASH_ANIMATION_FLASH_COUNT * 2)
                {
                    _isPlayingFlashAnimation = false;
                }
            }
        }

        private Rectangle GetDigitTextureBounds(int digit)
        {
            if (digit < 0 || digit > 9)
                throw new ArgumentOutOfRangeException("digit", "The value of digit must be between 0 and 9.");
            int posX = digit * TEXTURE_COORDS_NUMBER_WIDTH + TEXTURE_COORDS_NUMBER_X;
            return new Rectangle(posX, TEXTURE_COORDS_NUMBER_Y, TEXTURE_COORDS_NUMBER_WIDTH, TEXTURE_COORDS_NUMBER_HEIGHT);

        }

        private int[] SplitDigits(int score)
        {
            string inputStr = score.ToString().PadLeft(NUMBER_DIGITS_TO_DRAW, '0');
            int[] result = new int[inputStr.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (int)char.GetNumericValue(inputStr[i]);
            }
            return result;
        }

    }
}
