using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    public abstract class Obstacle : IGameEntity
    {

        //fields
        private Trex _trex;
        protected Sprite _sprite;
        //properties
        public abstract Rectangle CollisionBox { get; }
        public int DrawOrder {get; set;}
        public Vector2 Position { get; private set; }
        

        protected Obstacle(Trex trex, Vector2 position)
        {
            _trex = trex;
            Position = position;
        }


        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public virtual void Update(GameTime gameTime)
        {
            float posX = Position.X - _trex.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2(posX, Position.Y);
        }


    }
}
