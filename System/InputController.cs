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
        private bool _isBlocked;
        private KeyboardState _previousKeyboardState;


        public InputController(Trex trex)
        {
            _trex = trex;
        }

        public void ProcessControls(GameTime gametime)
        {
            
            KeyboardState keyboardState = Keyboard.GetState();

            if(!_isBlocked)
            {
                bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasJumpKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);
                if(isJumpKeyPressed && !wasJumpKeyPressed)
                {
                    if (_trex.State != TrexState.Jumping)
                        _trex.BeginJump();
                }
                else if (_trex.State == TrexState.Jumping && !isJumpKeyPressed)
                {
                    _trex.CancelJump();
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (_trex.State == TrexState.Jumping || _trex.State == TrexState.Falling)
                        _trex.Drop();
                    else
                        _trex.Duck();
                }
                else if (_trex.State == TrexState.Ducking || !keyboardState.IsKeyDown(Keys.Down))
                {
                    _trex.CancelDuck();
                }
                _previousKeyboardState = keyboardState;

            }
            _previousKeyboardState = keyboardState;
            _isBlocked = false;
        }

        public void TemptInputBlock()
        {
            _isBlocked = true;
        }
    }
}
