using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrexRunner.Entities
{
    public interface ICollidable
    {
        Rectangle CollisionBox { get; }
    }
}
