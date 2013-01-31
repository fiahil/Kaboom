using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private readonly int[] maxZoom_;
        private int widthRef_;
        private DisplayOrientation oldOrientation_;
        private readonly List<Player> players_;
        private readonly Gameplay gameplay_;

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
            this.em_ = new Event();
            this.players_ = new List<Player>();
            this.gameplay_ = new Gameplay();
            this.maxZoom_ = new int[2];
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

            this.players_.Add(new Player(this.map_, "Player 1", true));
            this.players_.Add(new Player(this.map_, "Player 2"));

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

            ///////////////////////////////////////////////////////TODO: Not A TODO: Gesture test
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
                        foreach (var player in this.players_)
                        {
                            if (player.TurnToPlay)
                            {
                                try
                                {
                                    if (player.Name == "Player 1")
                                    {
                                        var coord = this.map_.GetCoordByPos(ret.Pos);
                                        var entity = new UnitestEntity(this.map_.GetMaxZIndexOnCoord(coord) + 1,
                                                                       new SpriteSheet(
                                                                           KaboomResources.Textures["background2"],
                                                                           new[] {1}), EVisibility.Transparent);
                                        this.gameplay_.OnNewEntity(entity);
                                        this.map_.AddNewEntity(entity, coord);
                                        player.TurnToPlay = false;
                                    }
                                    else
                                    {
                                        var coord = this.map_.GetCoordByPos(ret.Pos);
                                        var entity = new UnitestEntity(this.map_.GetMaxZIndexOnCoord(coord) + 1,
                                                                       new SpriteSheet(
                                                                           KaboomResources.Textures["background3"],
                                                                           new[] {1}), EVisibility.Transparent);
                                        this.gameplay_.OnNewEntity(entity);
                                        this.map_.AddNewEntity(entity, coord);
                                        player.TurnToPlay = false;
                                    }
                                }
                                catch
                                {
                                    player.TurnToPlay = false;
                                }
                            }
                            else
                            {
                                player.TurnToPlay = true;
                            }
                        }
                        break;
                }
                ret = this.em_.GetEvents();
            }

            ///////////////////////////////////////////////////////////////TODO: Not a TODO: Gameplay handler. Done above

            ///////////////////////////////////////////////////////////////TODO: Not a TODO: BackButton handler
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