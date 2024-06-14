using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrexRunner.Graphics
{
    public class SpriteAnimation
    {
        private List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame> ();

        //indexer
        public SpriteAnimationFrame this[int index]
        {
            get
            {
                return GetFrame(index); 
            }
        }

        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                return _frames.Where(f => f.TimeStamp <= PlaybackProgress)
                    .OrderBy(f => f.TimeStamp)
                    //return null if no frames in animation
                    .LastOrDefault();
            }
        }

        public float Duration
        {
            get
            {
                if (!_frames.Any())
                    return 0;
                return _frames.Max(f => f.TimeStamp);
            }
        }
        public bool isPlaying { get; private set; }
        public float PlaybackProgress { get; private set; }

        public void AddFrame(Sprite sprite, float timeStamp)
        {
            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);
            _frames.Add(frame);
        }

        public void Update(GameTime gameTime)
        {
            if (isPlaying)
            {
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (PlaybackProgress > Duration)
                    PlaybackProgress -= Duration;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteAnimationFrame frame = CurrentFrame;

            if (frame != null)
                frame.Sprite.Draw(spriteBatch, position);
        }
        
        public void Play()
        {
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
            PlaybackProgress = 0;
        }

        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index<0 || index >= _frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Animation frame at index " + index + " does not exist.");
            return _frames[index];
        }
    }
}
