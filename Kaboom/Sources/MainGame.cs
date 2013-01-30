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
        private int widthRef_;
        private Microsoft.Xna.Framework.DisplayOrientation oldOrientation_;

        /// <summary>
        /// Create the game instance
        /// </summary>
        public MainGame()
        {
            this.graphics_ = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = true,
                    SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight | DisplayOrientation.Portrait
                };
            em_ = new Event();
            maxZoom_ = new int[2];
            this.height_ = 1;
            this.width_ = 1;
            this.widthRef_ = 1;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Load heavy content and resources that shouldn't be copied
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
            this.widthRef_ = GraphicsDevice.Viewport.Width;
            this.map_ = new Map(this, this.spriteBatch_, this.width_, this.height_);
            this.Components.Add(this.map_);
            this.oldOrientation_ = this.Window.CurrentOrientation;

            if (this.Window.CurrentOrientation == DisplayOrientation.Portrait)
            {
                maxZoom_[1] = GraphicsDevice.Viewport.Width / this.width_;
                if (maxZoom_[1] >
                    (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Height)) / this.height_)
                    maxZoom_[1] = (GraphicsDevice.Viewport.Height - (int) (0.1 * GraphicsDevice.Viewport.Width)) /
                                  this.height_;
                maxZoom_[0] = GraphicsDevice.Viewport.Height / this.width_;
                if (maxZoom_[0] >
                    (GraphicsDevice.Viewport.Width - (int)(0.1 * GraphicsDevice.Viewport.Height)) / this.height_)
                    maxZoom_[0] = (GraphicsDevice.Viewport.Width - (int) (0.1 * GraphicsDevice.Viewport.Width)) /
                                  this.height_;
                Camera.Instance.DimY = maxZoom_[1];
                Camera.Instance.DimX = maxZoom_[1];
                Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[1] * this.width_)) / 2;
                Camera.Instance.OffY = (GraphicsDevice.Viewport.Height - (int)(0.1 * GraphicsDevice.Viewport.Width) -
                                        (maxZoom_[1] * this.height_)) / 2 + (int)(0.1 * GraphicsDevice.Viewport.Width);
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
                    maxZoom_[1] = (GraphicsDevice.Viewport.Width - (int)(0.1 * GraphicsDevice.Viewport.Height)) /
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
            if (this.oldOrientation_ != this.Window.CurrentOrientation && this.widthRef_ != GraphicsDevice.Viewport.Width)
            {
                if (this.Window.CurrentOrientation == DisplayOrientation.Portrait)
                {
                    Camera.Instance.DimY = maxZoom_[1];
                    Camera.Instance.DimX = maxZoom_[1];
                    Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[1] * this.width_)) / 2;
                    Camera.Instance.OffY = (GraphicsDevice.Viewport.Height -
                                            (int)(0.1 * GraphicsDevice.Viewport.Width) -
                                            (maxZoom_[1] * this.height_)) / 2 +
                                           (int)(0.1 * GraphicsDevice.Viewport.Width);
                }
                else
                {
                    Camera.Instance.DimY = maxZoom_[0];
                    Camera.Instance.DimX = maxZoom_[0];
                    Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[0] * this.width_)) / 2;
                    Camera.Instance.OffY = (GraphicsDevice.Viewport.Height -
                                            (int)(0.1 * GraphicsDevice.Viewport.Height) -
                                            (maxZoom_[0] * this.height_)) / 2 +
                                           (int)(0.1 * GraphicsDevice.Viewport.Height);
                }
                this.em_.isZoomed_ = false;
                this.oldOrientation_ = this.Window.CurrentOrientation;
                this.widthRef_ = GraphicsDevice.Viewport.Width;
            }

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
                        try
                        {
                            var where = this.map_.GetCoordByPos(ret.Pos);
                            Camera.Instance.OffX = -1 * (where.X * 80) + GraphicsDevice.Viewport.Width / 2;
                            if (this.Window.CurrentOrientation == DisplayOrientation.Portrait)
                                Camera.Instance.OffY = -1 * (where.Y * 80) + (int)(0.1 * GraphicsDevice.Viewport.Width) + GraphicsDevice.Viewport.Height / 2;
                            else
                                Camera.Instance.OffY = -1 * (where.Y * 80) + (int)(0.1 * GraphicsDevice.Viewport.Height) + GraphicsDevice.Viewport.Height / 2;

                        }
                        catch (Exception)
                        {
                            Camera.Instance.OffX = -1 * ((this.width_ * 80) / 2) + GraphicsDevice.Viewport.Width / 2;
                            if (this.Window.CurrentOrientation == DisplayOrientation.Portrait)
                                Camera.Instance.OffY = -1 * ((this.height_ * 80) /2) + (int)(0.1 * GraphicsDevice.Viewport.Width) + GraphicsDevice.Viewport.Height / 2;
                            else
                                Camera.Instance.OffY = -1 * ((this.height_ * 80) /2) + (int)(0.1 * GraphicsDevice.Viewport.Height) + GraphicsDevice.Viewport.Height / 2;
                        }
                        Camera.Instance.DimX = 80;
                        Camera.Instance.DimY = 80;
                        
                        break;
                    case Action.Type.ZoomOut:
                        if (this.Window.CurrentOrientation == DisplayOrientation.Portrait)
                        {
                            Camera.Instance.DimY = maxZoom_[1];
                            Camera.Instance.DimX = maxZoom_[1];
                            Camera.Instance.OffX = (GraphicsDevice.Viewport.Width - (maxZoom_[1] * this.width_)) / 2;
                            Camera.Instance.OffY = (GraphicsDevice.Viewport.Height -
                                                    (int)(0.1 * GraphicsDevice.Viewport.Width) -
                                                    (maxZoom_[1] * this.height_)) / 2 +
                                                   (int)(0.1 * GraphicsDevice.Viewport.Width);
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
                        try
                        {
                            this.map_.AddNewEntity(
                                new UnitestEntity(2, KaboomResources.Textures["pony"], EVisibility.Transparent),
                                this.map_.GetCoordByPos(ret.Pos));
                        }
                        catch
                        {

                        }
                        break;
                }
                ret = this.em_.GetEvents();                 
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
        }

        /// <summary>
        /// Draw game components (Map)
        /// </summary>
        /// <param name="gameTime">Game clock</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);

            base.Draw(gameTime);
        }
    }
}