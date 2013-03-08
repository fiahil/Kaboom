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

    class MainGame : Game
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

        private readonly GraphicsDeviceManager graphics_;
        private SpriteBatch spriteBatch_;
        private readonly string level_;
        private readonly Event em_;
        private Map map_;
        private Hud hud_;
        private readonly CurrentElement currentBomb_;

        /// <summary>
        /// Create the game instance
        /// </summary>
        public MainGame(string level)
        {
            currentBomb_ = new CurrentElement();
            this.graphics_ = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = true,
                    SupportedOrientations = DisplayOrientation.LandscapeLeft
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
            KaboomResources.Textures["BombSheetSquare"] = Content.Load<Texture2D>("BombSheetSquare");
            KaboomResources.Textures["BombSheetLine"] = Content.Load<Texture2D>("BombSheetLine");
            KaboomResources.Textures["BombSheetAngle"] = Content.Load<Texture2D>("BombSheetAngle");
            KaboomResources.Textures["BombSheetH"] = Content.Load<Texture2D>("BombSheetH");
            KaboomResources.Textures["BombSheetX"] = Content.Load<Texture2D>("BombSheetX");
            KaboomResources.Textures["BombSheetTNT"] = Content.Load<Texture2D>("BombSheetTNT");
            KaboomResources.Textures["BombSheetUltimate"] = Content.Load<Texture2D>("BombSheetUltimate");
            KaboomResources.Textures["BombSheetBigSquare"] = Content.Load<Texture2D>("BombSheetBigSquare");
            KaboomResources.Textures["hud"] = Content.Load<Texture2D>("hud");
            KaboomResources.Textures["hud_active"] = Content.Load<Texture2D>("hud_active");
            KaboomResources.Textures["highlight"] = Content.Load<Texture2D>("HighLight");
            KaboomResources.Textures["highlight2"] = Content.Load<Texture2D>("HighLight2");
            KaboomResources.Textures["goal"] = Content.Load<Texture2D>("GoalSheet");
            KaboomResources.Textures["checkpoint"] = Content.Load<Texture2D>("CheckPoint");

            KaboomResources.Sprites["Bomb"] = new SpriteSheet(KaboomResources.Textures["BombSheet"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombUltimate"] = new SpriteSheet(KaboomResources.Textures["BombSheetUltimate"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombSquare"] = new SpriteSheet(KaboomResources.Textures["BombSheetSquare"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombAngle"] = new SpriteSheet(KaboomResources.Textures["BombSheetAngle"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombBigSquare"] = new SpriteSheet(KaboomResources.Textures["BombSheetBigSquare"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombLine"] = new SpriteSheet(KaboomResources.Textures["BombSheetLine"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombTNT"] = new SpriteSheet(KaboomResources.Textures["BombSheetTNT"], new[] { 1, 25 }, 2, 20);
            KaboomResources.Sprites["BombH"] = new SpriteSheet(KaboomResources.Textures["BombSheetH"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombX"] = new SpriteSheet(KaboomResources.Textures["BombSheetX"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["DestructibleBlock"] = new SpriteSheet(KaboomResources.Textures["background2"], new[] { 1, 2 }, 2, 2);
            KaboomResources.Sprites["UndestructibleBlock"] = new SpriteSheet(KaboomResources.Textures["background3"], new[] { 1 }, 1, 1);
            KaboomResources.Sprites["Ground"] = new SpriteSheet(KaboomResources.Textures["background1"], new[] {1}, 1);

            KaboomResources.Fonts["default"] = Content.Load<SpriteFont>("defaultFont");
            KaboomResources.Levels["level1"] = LoadLevel("level1");
            KaboomResources.Levels["level2"] = LoadLevel("level2");
        }

        /// <summary>
        /// Initialise game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.map_ = new Map(this, this.spriteBatch_, KaboomResources.Levels[this.level_]);
            this.map_.EndGameManager += ManageEndGame;
            Viewport.Instance.Initialize(GraphicsDevice, this.map_);
            this.Components.Add(this.map_);
            this.hud_ = new Hud(this, this.spriteBatch_, new List<Hud.BombInfo>
                                                             {
                                                                 new Hud.BombInfo(Pattern.Type.Square,
                                                                                  KaboomResources.Sprites["BombSquare"].
                                                                                      Clone()
                                                                                  as SpriteSheet, 3, "BombSquare"),
                                                                 new Hud.BombInfo(Pattern.Type.Line,

                                                                                  KaboomResources.Sprites["BombLine"].
                                                                                      Clone()
                                                                                  as SpriteSheet, 5, "BombLine"),
                                                                 new Hud.BombInfo(Pattern.Type.Angle,
                                                                                  KaboomResources.Sprites["BombAngle"].
                                                                                      Clone()
                                                                                  as SpriteSheet, 5, "BombAngle"),
                                                                 new Hud.BombInfo(Pattern.Type.Ultimate,
                                                                                  KaboomResources.Sprites["BombUltimate"
                                                                                      ].Clone()
                                                                                  as SpriteSheet, 5, "BombUltimate"),
                                                                 new Hud.BombInfo(Pattern.Type.X,
                                                                                  KaboomResources.Sprites["BombX"
                                                                                      ].Clone()
                                                                                  as SpriteSheet, 5, "BombX")

                                                             });

            // TODO : This is a HUD Unitest
            // TODO : get the round number in map object
            hud_.GameInfos.Round = 10;
            hud_.GameInfos.Score = 0;
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
                                    map_.ActivateDetonators();
                                    if (currentBomb_.Coord.X != -1)
                                        map_.RemoveEntity(currentBomb_.Coord);
                                    currentBomb_.Coord.X = -1;
                                    currentBomb_.Coord.Y = -1;
                                    hud_.GameInfos.Round -= 1;
                                    if (hud_.GameInfos.Round <= 0)
                                        hud_.GameInfos.Round = 0;

                                    // TODO : calcul du score
                                    hud_.GameInfos.Score += 9000;

                                }
                                else if (hudEvent == Hud.EHudAction.BombRotation)
                                {
                                    var pattern = hud_.SelectedBombType();
                                    if (pattern != Pattern.Type.NoPattern && currentBomb_.Coord.X != -1)
                                    {
                                        currentBomb_.Entity.NextOrientation();
                                        this.map_.AddNewEntity(currentBomb_.Entity, currentBomb_.Coord);
                                    }
                                }
                                else if (hudEvent == Hud.EHudAction.BombSelection)
                                {
                                    if (currentBomb_.Coord.X != -1)
                                        map_.RemoveEntity(currentBomb_.Coord);
                                    currentBomb_.Coord.X = -1;
                                    currentBomb_.Coord.Y = -1;
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
                                            currentBomb_.Coord.X = -1;
                                            currentBomb_.Coord.Y = -1;
                                        }
                                        else
                                        {
                                            if (currentBomb_.Coord.X != -1)
                                                map_.RemoveEntity(currentBomb_.Coord);
                                            currentBomb_.Coord = this.map_.GetCoordByPos(ret.Pos);
                                            currentBomb_.Entity = new VirtualBomb(pattern,
                                                                                  KaboomResources.Sprites[hud_.SelectedBombName()].Clone() as SpriteSheet);
                                            if (!(this.map_.AddNewEntity(currentBomb_.Entity, currentBomb_.Coord)))
                                            {
                                                currentBomb_.Coord.X = -1;
                                                currentBomb_.Coord.Y = -1;
                                            }
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

        public void ManageEndGame(object sender, EventArgs ea)
        {
            // TODO : Manage end game here
            this.Exit();
        }
    }
}