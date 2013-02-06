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
        private Hud hud_;
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
            KaboomResources.Textures["BombSheet"] = Content.Load<Texture2D>("BombSheet");
            KaboomResources.Textures["hud"] = Content.Load<Texture2D>("hud");
            KaboomResources.Fonts["default"] = Content.Load<SpriteFont>("defaultFont");
        }

        /// <summary>
        /// Initialise game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.players_.Add(new Player(this.map_, "Player 1", true));
            this.players_.Add(new Player(this.map_, "Player 2"));

            this.map_ = new Map(this, this.spriteBatch_, 40, 20);
            Viewport.Instance.Initialize(GraphicsDevice, this.map_, 40, 20); // TODO map size property
            this.Components.Add(this.map_);
            this.hud_ = new Hud(this, this.spriteBatch_);
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
                        var hudEvent = this.hud_.GetHudEvent(ret.Pos);
                        if (hudEvent != Hud.EHudAction.NoAction)
                        {
                            if (hudEvent == Hud.EHudAction.Detonator)
                            {
                                this.map_.SetExplosion(new Point(0, 0));
                            }
                        }
                        else
                        {
                            foreach (var player in this.players_)
                            {
                                if (player.TurnToPlay)
                                {
                                    try
                                    {
                                        if (player.Name == "Player 1")
                                        {
                                            var coord = this.map_.GetCoordByPos(ret.Pos);
                                            var entity = new Bomb(Pattern.Type.Square,
                                                                  this.map_.GetMaxZIndexOnCoord(coord) + 1,
                                                                  new SpriteSheet(
                                                                      KaboomResources.Textures["BombSheet"],
                                                                      new[] {8, 18}, 2));
                                            //var entity = new StaticEntity(this.map_.GetMaxZIndexOnCoord(coord) + 1,
                                            //                               new SpriteSheet(
                                            //                                   KaboomResources.Textures["background2"],
                                            //                                   new[] {1}), EVisibility.Transparent);
                                            this.gameplay_.OnNewEntity(entity);
                                            this.map_.AddNewEntity(entity, coord);
                                            player.TurnToPlay = false;
                                        }
                                        else
                                        {
                                            var coord = this.map_.GetCoordByPos(ret.Pos);
                                            var entity = new Bomb(Pattern.Type.Square,
                                                                  this.map_.GetMaxZIndexOnCoord(coord) + 1,
                                                                  new SpriteSheet(
                                                                      KaboomResources.Textures["BombSheet"],
                                                                      new[] {8, 18}, 2));
                                            //var entity = new StaticEntity(this.map_.GetMaxZIndexOnCoord(coord) + 1,
                                            //                               new SpriteSheet(
                                            //                                   KaboomResources.Textures["background3"],
                                            //                                   new[] {1}), EVisibility.Transparent);
                                            //this.gameplay_.OnNewEntity(entity);
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
                        }
                        break;
                }
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
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}