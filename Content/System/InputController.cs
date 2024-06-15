using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TrexRunner.Entities;

namespace TrexRunner.Content.System
{
    public class InputController
    {
        private Trex _trex;
        private KeyboardState _previousKeyboardState;

        public InputController(Trex trex)
        {
            _trex = trex;
        }

        public void ProcessControls(GameTime gametime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
            {
                if (_trex.State != TrexState.Jumping)
                    _trex.BeginJump();
            }
            else if (_trex.State == TrexState.Jumping && !keyboardState.IsKeyDown(Keys.Up))
            {
                _trex.CancelJump();
            }
            _previousKeyboardState = keyboardState;
        }
    }
}
