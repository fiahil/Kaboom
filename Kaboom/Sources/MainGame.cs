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
        private readonly Event em_;
        private Map map_;
        private int height_;
        private int width_;
        private int[] maxZoom_;

        /// <summary>
        /// Create the game instance
        /// </summary>
        public MainGame()
        {
            this.graphics_ = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = true
                };
            em_ = new Event();
            maxZoom_ = new int[2];
            this.height_ = 1;
            this.width_ = 1;
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

            this.height_ = 20;
            this.width_ = 40;
            this.map_ = new Map(this, this.spriteBatch_, this.width_, this.height_);
            this.Components.Add(this.map_);
            if (this.Window.CurrentOrientation == DisplayOrientation.Portrait)
            {
                maxZoom_[1] = GraphicsDevice.Viewport.Width / this.width_;
                if (maxZoom_[1] >
                    (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Height)) / this.height_)
                    maxZoom_[1] = (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Height)) /
                                  this.height_;
                maxZoom_[0] = GraphicsDevice.Viewport.Height / this.width_;
                if (maxZoom_[0] >
                    (GraphicsDevice.Viewport.Width - (int) (0.1 * GraphicsDevice.Viewport.Width)) / this.height_)
                    maxZoom_[0] = (GraphicsDevice.Viewport.Width - (int) (0.1 * GraphicsDevice.Viewport.Width)) /
                                  this.height_;
                Camera.Instance.DimY = maxZoom_[0];
                Camera.Instance.DimX = maxZoom_[0];
                Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[1] * this.width_)) / 2;
                Camera.Instance.OffY = (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Height) -
                                        (maxZoom_[1] * this.height_)) / 2 + (int) (0.1 * GraphicsDevice.Viewport.Height);
            }
            else
            {
                maxZoom_[0] = GraphicsDevice.Viewport.Width / this.width_;
                if (maxZoom_[0] >
                    (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Height)) / this.height_)
                    maxZoom_[0] = (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Height)) /
                                  this.height_;
                maxZoom_[1] = GraphicsDevice.Viewport.Height / this.width_;
                if (maxZoom_[1] >
                    (GraphicsDevice.Viewport.Width - (int) (0.1 * GraphicsDevice.Viewport.Width)) / this.height_)
                    maxZoom_[1] = (GraphicsDevice.Viewport.Width - (int) (0.1 * GraphicsDevice.Viewport.Width)) /
                                  this.height_;
                Camera.Instance.DimY = maxZoom_[0];
                Camera.Instance.DimX = maxZoom_[0];
                Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[0] * this.width_)) / 2;
                Camera.Instance.OffY = (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Height) -
                                        (maxZoom_[0] * this.height_)) / 2 + (int) (0.1 * GraphicsDevice.Viewport.Height);
            }
        }

        /// <summary>
        /// Update game and game components
        /// </summary>
        /// <param name="gameTime">Game clock</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ///////////////////////////////////////////////////////TODO: Gesture test
            var ret = this.em_.GetEvents();
            while (ret.ActionType != Action.Type.NoEvent)
            {
                switch (ret.ActionType)
                {
                    case Action.Type.NoEvent:
                        break;
                    case Action.Type.Drag:
                        Camera.Instance.OffX += ret.DeltaX;
                        Camera.Instance.OffY += ret.DeltaY;
                        break;
                    case Action.Type.ZoomIn:
                        Camera.Instance.DimX = 80;
                        Camera.Instance.DimY = 80;
                        // TODO : It's Nawak
                        Camera.Instance.OffX = (int) (ret.Pos.X);
                        Camera.Instance.OffY = (int) (ret.Pos.Y);
                        break;
                    case Action.Type.ZoomOut:
                        if (this.Window.CurrentOrientation == DisplayOrientation.Portrait)
                        {
                            Camera.Instance.DimY = maxZoom_[0];
                            Camera.Instance.DimX = maxZoom_[0];
                            Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[1] * this.width_)) / 2;
                            Camera.Instance.OffY = (GraphicsDevice.Viewport.Height -
                                                    (int) (0.1 * GraphicsDevice.Viewport.Height) -
                                                    (maxZoom_[1] * this.height_)) / 2 +
                                                   (int) (0.1 * GraphicsDevice.Viewport.Height);
                        }
                        else
                        {
                            Camera.Instance.DimY = maxZoom_[0];
                            Camera.Instance.DimX = maxZoom_[0];
                            Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[0] * this.width_)) / 2;
                            Camera.Instance.OffY = (GraphicsDevice.Viewport.Height -
                                                    (int) (0.1 * GraphicsDevice.Viewport.Height) -
                                                    (maxZoom_[0] * this.height_)) / 2 +
                                                   (int) (0.1 * GraphicsDevice.Viewport.Height);
                        }
                        break;
                    case Action.Type.Tap:
                        this.map_.AddNewEntity(
                            new UnitestEntity(2, KaboomResources.Textures["pony"], EVisibility.Transparent),
                            this.map_.GetCoordByPos(ret.Pos));
                        break;
                }
                ret = this.em_.GetEvents();                 
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