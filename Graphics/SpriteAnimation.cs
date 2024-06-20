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

        //fields and properties
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
        public bool ShouldLoop { get; set; } = true;

        //methods
        //adds a new frame (with the sprite and timestamp) to an animation
        public void AddFrame(Sprite sprite, float timeStamp)
        {
            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);
            _frames.Add(frame);
        }

        //updates animation progress using elapsed time. loops if shouldloop is true.
        public void Update(GameTime gameTime)
        {
            if (isPlaying)
            {
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (PlaybackProgress > Duration)
                {
                    if(ShouldLoop)
                        PlaybackProgress -= Duration;
                    else
                        Stop();
                }
                
            }
        }

        //draws current animation frame if frames exist.
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteAnimationFrame frame = CurrentFrame;

            if (frame != null)
                frame.Sprite.Draw(spriteBatch, position);
        }
        
        //plays animation
        public void Play()
        {
            isPlaying = true;
        }

        //stops animation, resetting playback progress.
        public void Stop()
        {
            isPlaying = false;
            PlaybackProgress = 0;
        }

        //retrieves a frame of an animation using its index
        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index<0 || index >= _frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Animation frame at index " + index + " does not exist.");
            return _frames[index];
        }

        //clears frames
        public void Clear()
        {
            Stop();
            _frames.Clear();
        }

        public static SpriteAnimation CreateSimpleAnimation(Texture2D texture, Point startPos, int width, int height, Point offset, int frameCount, float frameLength)
        {
            if(texture == null)
                throw new ArgumentNullException(nameof(texture));
            SpriteAnimation anim = new SpriteAnimation();

            for (int i = 0; i < frameCount; i++)
            {
                Sprite sprite = new Sprite(texture, startPos.X + i * offset.X, startPos.Y + i * offset.Y, width, height);
                anim.AddFrame(sprite, frameLength * i);
                if (i == frameCount - 1)
                    anim.AddFrame(sprite, frameLength * (i + 1));
                
            }
            return anim;
        }
    }
}
