using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Kaboom.Sources
{
    /// <summary>
    /// Definition of an animated Sprite
    /// </summary>
    class SpriteSheet
    {
        private Texture2D _SpriteSheet;
        private Rectangle _frameSize;
        private double _frameSpeed;
        private int _currentAnimation;
        private int _currentFrame;
        private int[] _totalFrames;
        private int _totalAnimations;
        private double _currentElapsedTime;

        /// <summary>
        /// Constructor
        /// <param name="text"> Texture Loaded from the Library</param>
        /// <param name="framesPerAnim"> the number of frames each animations contains</param>
        /// <param name="animations"> the number of animations the sheet contains</param>
        /// <param name="frameSpeed"> the speed of rendering of the animation</param>
        /// </summary>
        public SpriteSheet(Texture2D text, int[] framesPerAnim, int animations = 1, double frameSpeed = 30.0)
        {
            this._SpriteSheet = text;
            this._frameSpeed = frameSpeed;
            this._currentAnimation = 0;
            this._currentFrame = 0;
            this._totalFrames = framesPerAnim;
            this._totalAnimations = animations;
            this._currentElapsedTime = 0;

                this._frameSize = new Rectangle(0, 0, (int)(this._SpriteSheet.Width / this._totalFrames.Max()),
                                                (int)(this._SpriteSheet.Height / this._totalAnimations));
        }

        /// <summary>
        /// Update the current SpriteSheet and its animations.
        /// There, it just play the animations repeatedly.
        /// <param name="gameTime">Current Game Timer</param> 
        /// </summary>
        public void Update(GameTime gameTime)
        {
         this._currentElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

         if (this._currentElapsedTime >= 1 / this._frameSpeed)
         {
             this._currentElapsedTime = 0.0;
             ++this._currentFrame;
         }

         if (this._currentFrame >= this._totalFrames[this._currentAnimation])
         {
             this._currentFrame = 0;
             ++this._currentAnimation;
             if (this._currentAnimation >= this._totalAnimations)
                 this._currentAnimation = 0;
         }

        }

        /// <summary>
        /// Draw the current sprite of the animation on screen
        /// <param name="sb">SpriteBatch used to draw textures</param>   
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime t, Point r)
        {
            sb.Draw(
                this._SpriteSheet,
                new Rectangle(
                    (r.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                    (r.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                    Camera.Instance.DimX,
                    Camera.Instance.DimY),
                new Rectangle(
                    this._frameSize.Width * this._currentFrame,
                    this._frameSize.Height * (int)this._currentAnimation,
                    this._frameSize.Width,
                    this._frameSize.Height),
                Color.White);
        }
    };
                   
 
    /// <summary>
    /// Definition of a Font who represent a Latin Style alphabet (26char)
    /// plus the numerics and several special char.
    /// </summary>
    class TTFont
    {
        private SpriteFont   _font;
        //private Rectangle   _frameSize;
        //private Rectangle   _frameBlit;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="font">the loaded font who represent the current used alphabet</param>
        public TTFont(SpriteFont font)
        {
            this._font = font;
            //this._frameSize = new Rectangle(0, 0, (int)(this._font.Width / 5),
            //                     (int)(this._font.Height / 8));

            //this._frameBlit.Width = this._frameSize.Width;
            //this._frameBlit.Height = this._frameSize.Height;
        }

        /// <summary>
        /// Method who allow the user to display a string on the screen at the position he wants
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw textures</param>
        /// <param name="str">the string to display on screen</param>
        /// <param name="posInScreen">the position where the display ll be printed</param>
        public void putstrOnScreen(SpriteBatch sb, string str, Rectangle posInScreen)
        {
            sb.Begin();
            sb.DrawString(this._font, "<- Mets ton message ici ->", new Vector2(posInScreen.X, posInScreen.Y), Color.White);
            sb.End();

            //string alphanum = " abcdefghijklmnopqrstuvwxyz0123456789.:!?";
            //int pos = 0;

            //this._frameBlit = posInScreen;
            //sb.Begin();
            //for (int i = 0; i < str.Length; ++i)
            //{
            //    pos = alphanum.IndexOf(str[i]);

            //    sb.Draw(
            //        this._font,
            //        new Rectangle(
            //            this._frameBlit.X,
            //            this._frameBlit.Y,
            //            (int)(this._frameBlit.Width),
            //            (int)(this._frameBlit.Height)),
            //        new Rectangle(
            //            this._frameSize.Width * pos % 5,
            //            this._frameSize.Height * pos / 5,
            //            this._frameSize.Width,
            //            this._frameSize.Height),
            //        Color.White);

            //    this._frameBlit.X += this._frameSize.Width;
            //}
            //sb.End();
        }
    };
}