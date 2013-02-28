using System;
using System.IO;
using System.Xml.Serialization;
using Kaboom.Serializer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Kaboom.Sources
{
    class CurrentElement
    {
        public Point Coord;
        public VirtualBomb Entity;

        public CurrentElement()
        {
            Coord = new Point(-1, -1);
            Entity = null;
        }
    }

    class MainGame : Game
    {
        private readonly GraphicsDeviceManager graphics_;
        private SpriteBatch spriteBatch_;
        private readonly string level_;
        private readonly Event em_;
        private Map map_;
        private Hud hud_;
        private CurrentElement currentBomb_;

        /// <summary>
        /// Create the game instance
        /// </summary>
        public MainGame(string level)
        {
            currentBomb_ = new CurrentElement();
            this.graphics_ = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = true,
                    SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight | DisplayOrientation.Portrait
                };
            this.level_ = level;
            this.em_ = new Event();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Load a MapElements
        /// </summary>
        /// <param name="filename">filename to fetch</param>
        /// <returns>Map description</returns>
        private MapElements LoadLevel(string filename)
        {
            var serializer = new XmlSerializer(typeof(MapElements));
            using (var fs = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, filename + ".xml")))
            {
                return (MapElements)serializer.Deserialize(fs);
            }
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
            KaboomResources.Textures["BombSheet"] = Content.Load<Texture2D>("BombSheet");
            KaboomResources.Textures["hud"] = Content.Load<Texture2D>("hud");
            KaboomResources.Textures["hud_active"] = Content.Load<Texture2D>("hud_active");
            KaboomResources.Textures["highlight"] = Content.Load<Texture2D>("HighLight");
            KaboomResources.Textures["highlight2"] = Content.Load<Texture2D>("HighLight2");


            KaboomResources.Fonts["default"] = Content.Load<SpriteFont>("defaultFont");
            KaboomResources.Levels["level1"] = this.LoadLevel("level1");
        }

        /// <summary>
        /// Initialise game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.map_ = new Map(this, this.spriteBatch_, KaboomResources.Levels[this.level_]);
            Viewport.Instance.Initialize(GraphicsDevice, this.map_);
            this.Components.Add(this.map_);
            this.hud_ = new Hud(this, this.spriteBatch_, new List<Hud.BombInfo>
                                                             {
                                                                 new Hud.BombInfo(Pattern.Type.Square,
                                                                                  new SpriteSheet(
                                                                                      KaboomResources.Textures[
                                                                                          "BombSheet"], new[] {9, 18},
                                                                                      2), 3),
                                                                 new Hud.BombInfo(Pattern.Type.Angle,
                                                                                  new SpriteSheet(
                                                                                      KaboomResources.Textures[
                                                                                          "BombSheet"], new[] {9, 18},
                                                                                      2), 5),
                                                                 new Hud.BombInfo(Pattern.Type.Ultimate,
                                                                                  new SpriteSheet(
                                                                                      KaboomResources.Textures[
                                                                                          "BombSheet"], new[] {9, 18},
                                                                                      2), 1)

                                                             });
            // TODO : This is a HUD Unitest

            this.Components.Add(this.hud_);
        }

        /// <summary>
        /// Update game and game components
        /// </summary>
        /// <param name="gameTime">Game clock</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Viewport.Instance.Update();
            ///////////////////////////////////////////////////////TODO: Not A TODO: Gesture test
            Action ret;
            while ((ret = this.em_.GetEvents()).ActionType != Action.Type.NoEvent)
            {
                switch (ret.ActionType)
                {
                    case Action.Type.NoEvent:
                        break;
                    case Action.Type.Drag:
                        Viewport.Instance.AdjustPos(this.map_, ref ret.DeltaX, ref ret.DeltaY);
                        
                        Camera.Instance.OffX += ret.DeltaX;
                        Camera.Instance.OffY += ret.DeltaY;
                        
                        break;
                    case Action.Type.ZoomIn:
                        Viewport.Instance.ZoomIn(ret.Pos);
                        break;

                    case Action.Type.ZoomOut:
                        Viewport.Instance.ZoomOut();
                        break;

                    case Action.Type.Tap:
                        {
                            var hudEvent = this.hud_.GetHudEvent(ret.Pos);
                            if (hudEvent != Hud.EHudAction.NoAction)
                            {
                                if (hudEvent == Hud.EHudAction.BombDetonation)
                                {
                                    this.map_.SetExplosion(new Point(7, 7)); //TODO : place true detonators
                                    if (currentBomb_.Coord.X != -1)
                                        map_.RemoveEntity(currentBomb_.Coord);
                                    currentBomb_.Coord.X = -1;
                                    currentBomb_.Coord.Y = -1;
                                }
                                else if (hudEvent == Hud.EHudAction.BombRotation)
                                {
                                    var pattern = hud_.SelectedBombType();
                                    if (pattern != Pattern.Type.NoPattern)
                                    {
                                        currentBomb_.Entity.NextOrientation();
                                        this.map_.AddNewEntity(currentBomb_.Entity, currentBomb_.Coord);
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    var pattern = hud_.SelectedBombType();
                                    if (pattern != Pattern.Type.NoPattern)
                                    {
                                        if (this.map_.GetCoordByPos(ret.Pos) == currentBomb_.Coord)
                                        {
                                            this.map_.AddNewEntity(currentBomb_.Entity.ToBomb(), currentBomb_.Coord);
                                            hud_.RemoveBombOfType(pattern);
                                            hud_.UnselectAll();

                                        }
                                        else
                                        {
                                            if (currentBomb_.Coord.X != -1)
                                                map_.RemoveEntity(currentBomb_.Coord);
                                            currentBomb_.Coord = this.map_.GetCoordByPos(ret.Pos);
                                            currentBomb_.Entity = new VirtualBomb(pattern,
                                                                                  new SpriteSheet(
                                                                                      KaboomResources.Textures[
                                                                                          "BombSheet"],
                                                                                      new[] { 8, 18 }, 2));
                                            this.map_.AddNewEntity(currentBomb_.Entity, currentBomb_.Coord);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                }
            }

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
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}