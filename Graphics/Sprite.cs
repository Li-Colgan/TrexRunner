﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrexRunner.Graphics
{
    public class Sprite
    {
        //variables
        public Texture2D Texture { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        //set tint to white by default
        public Color TintColor { get; set; } = Color.White;

        //constructor
        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, new Rectangle(X, Y, Width, Height), TintColor);
        }
    }
}
