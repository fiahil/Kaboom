using System;
using Kaboom.Serializer;
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

            //TODO import from serializer
            var r = new Random(777);
            var me = new MapElements(15, 15);

                for (var i = 0; i < 15; i++)
                {
                    for (var j = 0; j < 15; j++)
                    {
                        me.Board[i][j].Entities.Add(new EntityProxy
                            {
                                TileIdentifier = "background1",
                                TileFramePerAnim = new[] {1},
                                TileTotalAnim = 1,
                                TileFrameSpeed = 1,
                                ZIndex = 1
                            });

                        if (i == 6 && j == 7)
                        {
                            me.Board[i][j].Entities.Add(new BombProxy
                                {
                                    TileIdentifier = "BombSheet",
                                    TileFramePerAnim = new[] {8, 18},
                                    TileTotalAnim = 2,
                                    TileFrameSpeed = 20,
                                    Type = 0
                                });
                        }
                        if ((i == 7 || i == 6) && j == 7)
                            continue;

                        if (r.Next(2) == 0)
                        {
                            me.Board[i][j].Entities.Add(new BlockProxy
                                {
                                    Destroyable = true,
                                    TileIdentifier = "background2",
                                    TileFramePerAnim = new[] {1, 2},
                                    TileTotalAnim = 2,
                                    TileFrameSpeed = 2
                                });
                        }
                        else
                        {
                            me.Board[i][j].Entities.Add(new BlockProxy
                                {
                                    Destroyable = false,
                                    TileIdentifier = "background3",
                                    TileFramePerAnim = new[] {1},
                                    TileTotalAnim = 1,
                                    TileFrameSpeed = 1
                                });
                        }
                    }
                }
                //ENDOF TODO

            this.map_ = new Map(this, this.spriteBatch_, me);
            Viewport.Instance.Initialize(GraphicsDevice, this.map_);
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
                        {
                            var hudEvent = this.hud_.GetHudEvent(ret.Pos);
                            if (hudEvent != Hud.EHudAction.NoAction)
                            {
                                if (hudEvent == Hud.EHudAction.Detonator)
                                {
                                    this.map_.SetExplosion(new Point(7, 7)); //TODO : place true detonators
                                }
                            }
                            else
                            {
                                try
                                {
                                    var coord = this.map_.GetCoordByPos(ret.Pos);
                                    var entity = new Bomb(new[]
                                                            {
                                                                Pattern.Type.Angle,
                                                                Pattern.Type.Square,
                                                                Pattern.Type.Line,
                                                                Pattern.Type.BigSquare,
                                                                Pattern.Type.H,
                                                                Pattern.Type.X,
                                                                Pattern.Type.Ultimate
                                                            }[new Random().Next(7)], new SpriteSheet(KaboomResources.Textures["BombSheet"], new[] {8, 18}, 2));
                                    this.map_.AddNewEntity(entity, coord);
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