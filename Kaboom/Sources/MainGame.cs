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
    class MainGame : Game
    {
        private readonly GraphicsDeviceManager graphics_;
        private SpriteBatch spriteBatch_;

        /// <summary>
        /// Create the game instance
        /// </summary>
        public MainGame()
        {
            this.graphics_ = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = true
                };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Load heavy content and resources
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch_ = new SpriteBatch(GraphicsDevice);

            KaboomResources.Textures["background1"] = Content.Load<Texture2D>("background1");
            KaboomResources.Textures["background2"] = Content.Load<Texture2D>("background2");
            KaboomResources.Textures["background3"] = Content.Load<Texture2D>("background3");
            KaboomResources.Textures["pony"] = Content.Load<Texture2D>("pony");
        }

        /// <summary>
        /// Initialise game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.Components.Add(new Map(this, this.spriteBatch_, 20, 40));
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.DragComplete | GestureType.DoubleTap;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////// Patch
        private Vector2 oldDrag_ = Vector2.Zero;
        private bool isZoomed_ = false;
        ////////////////////////////////////////////////////////////////////////////////////////////////// EndofPatch

        /// <summary>
        /// Update game and game components
        /// </summary>
        /// <param name="gameTime">Game clock</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Gesture test
            while (TouchPanel.IsGestureAvailable)
            {
                var g = TouchPanel.ReadGesture();

                if (g.GestureType == GestureType.DoubleTap)
                {
                    Camera.Instance.DimX = Camera.Instance.DimX + (this.isZoomed_ ? -30 : 30);
                    Camera.Instance.DimY = Camera.Instance.DimY + (this.isZoomed_ ? -30 : 30);
                    this.isZoomed_ = !this.isZoomed_;
                }

                if (g.GestureType == GestureType.FreeDrag)
                {
                    if (oldDrag_ != Vector2.Zero)
                    {
                        Camera.Instance.OffX += (int)(g.Position.X - oldDrag_.X);
                        Camera.Instance.OffY += (int)(g.Position.Y - oldDrag_.Y);
                    }

                    this.oldDrag_ = g.Position;
                }
                if (g.GestureType == GestureType.DragComplete)
                    this.oldDrag_ = Vector2.Zero;
            }
        }

        /// <summary>
        /// Draw game components
        /// </summary>
        /// <param name="gameTime">Game clock</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);

            base.Draw(gameTime);
        }
    }
}