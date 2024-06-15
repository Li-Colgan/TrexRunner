using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrexRunner.Graphics
{
    public class SpriteAnimationFrame
    {
        //field
        private Sprite _sprite;
        
        //properties
        public Sprite Sprite { 
            //same as get{return _sprite}
            get => _sprite; 
            //example of how to get specific exceptions
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Sprite cannot be null.");
                _sprite = value;
            }
        }
        public float TimeStamp { get; }

        //constructor
        public SpriteAnimationFrame(Sprite sprite, float timeStamp)
        {
            Sprite = sprite;
            TimeStamp = timeStamp;
        }
    }
}
