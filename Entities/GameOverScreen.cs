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
    public class GameOverScreen : IGameEntity
    {
        private const int GAME_OVER_TEXTURE_POS_X = 655;
        private const int GAME_OVER_TEXTURE_POS_Y = 14;
        private const int GAME_OVER_TEXTURE_POS_WIDTH = 192;
        private const int GAME_OVER_TEXTURE_POS_HEIGHT = 14;

        private const int BUTTON_TEXTURE_POS_X = 1;
        private const int BUTTON_TEXTURE_POS_Y = 1;
        private const int BUTTON_TEXTURE_POS_WIDTH = 38;
        private const int BUTTON_TEXTURE_POS_HEIGHT = 34;

        private Sprite _textSprite;
        private Sprite _buttonSprite;

        public Vector2 Position { get; set; }

        public bool IsEnabled { get; set; }

        private Vector2 ButtonPosition => Position + new Vector2(GAME_OVER_TEXTURE_POS_WIDTH / 2 - BUTTON_TEXTURE_POS_WIDTH / 2, GAME_OVER_TEXTURE_POS_HEIGHT + 20);

        public int DrawOrder => 100;

        public GameOverScreen(Texture2D spriteSheet)
        {
            _textSprite = new Sprite
            (
                spriteSheet, 
                GAME_OVER_TEXTURE_POS_X, 
                GAME_OVER_TEXTURE_POS_Y, 
                GAME_OVER_TEXTURE_POS_WIDTH, 
                GAME_OVER_TEXTURE_POS_HEIGHT
            );
            _buttonSprite = new Sprite
            (
                spriteSheet, 
                BUTTON_TEXTURE_POS_X, 
                BUTTON_TEXTURE_POS_Y, 
                BUTTON_TEXTURE_POS_WIDTH, 
                BUTTON_TEXTURE_POS_HEIGHT
            );

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsEnabled)
                return;
            _textSprite.Draw(spriteBatch, Position);
            _buttonSprite.Draw(spriteBatch, ButtonPosition);
        }

        public void Update(GameTime gameTime)
        {
            if(!IsEnabled)
                return;

        }
    }
}
