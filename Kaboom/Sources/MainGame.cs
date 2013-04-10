using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
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
        private bool ended_;
        private Ladder ladder_;
        private bool lose_;
        private bool explosionMode_;
        private ScoreManager score_ = ScoreManager.Instance;

        private List<string> mapName_;
        private List<string> tutoName_;

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
            ended_ = false;
            lose_ = false;
            this.em_ = new Event();
            this.ladder_ = new Ladder(); // TODO : test
            this.ladder_.AddEntry(15000, "King");
            this.ladder_.AddEntry(10000, "Prince");
            this.ladder_.AddEntry(5000, "Champion");

            #region mapNameInit

            mapName_ = new List<string>()
                           {
                               "A-Maze-Me",
                               "CombisTheG",
                               "Corporate",
                               "ChooseYourSide",
                               "DidUCheckTuto",
                               "DynamiteWarehouse",
                               "FaceToFace",
                               "FindYourWayOut",
                               "InTheRedCorner",
                               "Invasion",
                               "It's Something",
                               "Life",
                               "NumbaWan",
                               "OneStepAway",
                               "OppositeForces",
                               "Tetris",
                               "TheBreach",
                               "Unreachable",
                               "Versus",
                               "XFactor"
                           };
            tutoName_ = new List<string>()
                            {
                                "TutoNormalBomb",
                                "TutoLineBomb",
                                "TutoConeBomb",
                                "TutoXBomb",
                                "TutoCheckPointBS",
                                "TutoHBomb",
                                "TutoUltimateBomb",
                                "TutoBonusTNT"
                            };

            #endregion

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

            #region Load

            KaboomResources.Textures["background1"] = Content.Load<Texture2D>("background1");
            KaboomResources.Textures["background2"] = Content.Load<Texture2D>("background2");
            KaboomResources.Textures["background3"] = Content.Load<Texture2D>("background3");
            KaboomResources.Textures["background4"] = Content.Load<Texture2D>("background4");
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
            KaboomResources.Textures["endScreen"] = Content.Load<Texture2D>("endScreen");
            KaboomResources.Textures["failScreen"] = Content.Load<Texture2D>("failScreen");
            KaboomResources.Textures["ladderScreen"] = Content.Load<Texture2D>("ladderScreen");

            KaboomResources.Sprites["Bomb"] = new SpriteSheet(KaboomResources.Textures["BombSheet"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombUltimate"] = new SpriteSheet(KaboomResources.Textures["BombSheetUltimate"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombSquare"] = new SpriteSheet(KaboomResources.Textures["BombSheetSquare"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombAngle"] = new SpriteSheet(KaboomResources.Textures["BombSheetAngle"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombBigSquare"] = new SpriteSheet(KaboomResources.Textures["BombSheetBigSquare"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombLine"] = new SpriteSheet(KaboomResources.Textures["BombSheetLine"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombTNT"] = new SpriteSheet(KaboomResources.Textures["BombSheetTNT"], new[] { 1, 25 }, 2, 20);
            KaboomResources.Sprites["BombH"] = new SpriteSheet(KaboomResources.Textures["BombSheetH"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["BombX"] = new SpriteSheet(KaboomResources.Textures["BombSheetX"], new[] { 8, 18 }, 2, 20);
            KaboomResources.Sprites["background2"] = new SpriteSheet(KaboomResources.Textures["background2"], new[] { 1, 15 }, 2, 20);
            KaboomResources.Sprites["background3"] = new SpriteSheet(KaboomResources.Textures["background3"], new[] { 1, 8 }, 2, 20);
            KaboomResources.Sprites["Ground"] = new SpriteSheet(KaboomResources.Textures["background1"], new[] { 1 }, 1);
            KaboomResources.Sprites["checkpoint"] = new SpriteSheet(KaboomResources.Textures["checkpoint"], new[] { 1, 14 }, 2);
            KaboomResources.Sprites["goal"] = new SpriteSheet(KaboomResources.Textures["goal"], new[] { 14, 14 }, 2);

            KaboomResources.Fonts["default"] = Content.Load<SpriteFont>("defaultFont");
            KaboomResources.Fonts["end"] = Content.Load<SpriteFont>("endFont");

            KaboomResources.Levels["TutoNormalBomb"] = LoadLevel("normalBombTuto");
            KaboomResources.Levels["TutoLineBomb"] = LoadLevel("LineBombTuto");
            KaboomResources.Levels["TutoConeBomb"] = LoadLevel("ConeBombTuto");
            KaboomResources.Levels["TutoXBomb"] = LoadLevel("CrossBombTuto");
            KaboomResources.Levels["TutoCheckPointBS"] = LoadLevel("CheckpointBS");
            KaboomResources.Levels["TutoHBomb"] = LoadLevel("HBombsCombinationTuto");
            KaboomResources.Levels["TutoUltimateBomb"] = LoadLevel("UltimateBombTuto");
            KaboomResources.Levels["TutoBonusTNT"] = LoadLevel("TNTBonusTuto");

            KaboomResources.Levels["A-Maze-Me"] = LoadLevel("A-Maze-Me");
            KaboomResources.Levels["CombisTheG"] = LoadLevel("CombisTheG");
            KaboomResources.Levels["Corporate"] = LoadLevel("Corporate");
            KaboomResources.Levels["ChooseYourSide"] = LoadLevel("ChooseYourSide");
            KaboomResources.Levels["DidUCheckTuto"] = LoadLevel("DidUCheckTuto");

            KaboomResources.Levels["DynamiteWarehouse"] = LoadLevel("DynamiteWarehouse");
            KaboomResources.Levels["FaceToFace"] = LoadLevel("FaceToFace");
            KaboomResources.Levels["FindYourWayOut"] = LoadLevel("FindYourWayOut");
            KaboomResources.Levels["InTheRedCorner"] = LoadLevel("InTheRedCorner");
            KaboomResources.Levels["Invasion"] = LoadLevel("Invasion");

            KaboomResources.Levels["It's Something"] = LoadLevel("It's Something");
            KaboomResources.Levels["Life"] = LoadLevel("Life");
            KaboomResources.Levels["NumbaWan"] = LoadLevel("NumbaWan");
            KaboomResources.Levels["OneStepAway"] = LoadLevel("OneStepAway");
            KaboomResources.Levels["OppositeForces"] = LoadLevel("OppositeForces");

            KaboomResources.Levels["Tetris"] = LoadLevel("Tetris");
            KaboomResources.Levels["TheBreach"] = LoadLevel("TheBreach");
            KaboomResources.Levels["Unreachable"] = LoadLevel("Unreachable");
            KaboomResources.Levels["Versus"] = LoadLevel("Versus");
            KaboomResources.Levels["XFactor"] = LoadLevel("XFactor");

            #endregion

        }

        /// <summary>
        /// Initialise game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.hud_ = new Hud(this, this.spriteBatch_);
            hud_.GameInfos.Round = 10;
            score_.Restart(10); // mettre le bombre de tour au depart

            this.map_ = new Map(this, this.spriteBatch_, KaboomResources.Levels[this.level_]);
            this.map_.EndGameManager += ManageEndGame;
            Viewport.Instance.Initialize(GraphicsDevice, this.map_);

            this.Components.Add(this.map_);
            this.Components.Add(this.hud_);
        }


        private void UpdateEnd(Action ret)
        {
             if (ret.ActionType == Action.Type.Tap)
             {
                 var hudEvent = this.hud_.GetHudEndEvent(ret.Pos);
                 switch (hudEvent)
                 {
                     case Hud.EHudEndAction.Menu:
                         Activity.StartActivity(new Intent(Activity, typeof (MenuActivity)));
                         break;
                     case Hud.EHudEndAction.Ladder:
                         if (!lose_)
                             ladder_.IsDisplay = true;
                         break;
                     case Hud.EHudEndAction.Score:
                         if (!lose_)
                             ladder_.IsDisplay = false;
                         break;
                     case Hud.EHudEndAction.Reload:
                         Activity.Finish();
                         Activity.StartActivity(new Intent(Activity, typeof (MainActivity)).PutExtra("level",
                                                                                                     this.level_));
                         break;
                     case Hud.EHudEndAction.Next:
                         Activity.Finish();
                         int idx;
                         if ((idx = mapName_.IndexOf(level_)) != -1)
                         {
                             idx++;
                             if (idx >= mapName_.Count)
                                 idx = 0;
                             Activity.StartActivity(new Intent(Activity, typeof (MainActivity)).PutExtra("level",
                                                                                                         mapName_[idx]));
                         }
                         else
                         {
                             idx = tutoName_.IndexOf(level_) + 1;
                             if (idx >= tutoName_.Count)
                                 idx = 0;
                             Activity.StartActivity(new Intent(Activity, typeof (MainActivity)).PutExtra("level",
                                                                                                         tutoName_[idx]));
                         }
                         break;
                 }
             }
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

            if (explosionMode_)
            {
                if (map_.NbExplosions == 0)
                {
                    explosionMode_ = false;
                    score_.EndOfTurn(hud_.TotalBombsNumber());
                }
            }
            else
            {
                if (hud_.GameInfos.Round <= 0)
                {
                    ended_ = true;
                    lose_ = true;
                }
            }
            Action ret;
            while ((ret = this.em_.GetEvents()).ActionType != Action.Type.NoEvent)
            {
                if (ended_)
                {
                    UpdateEnd(ret);
                }
                else
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
                        case Action.Type.Pinch:
                            {
                                Viewport.Instance.HandlePinch(ret);
                                break;
                            }
                        case Action.Type.Tap:
                            {
                                var hudEvent = this.hud_.GetHudEvent(ret.Pos);

                                if (explosionMode_) continue;
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

                                        this.hud_.ResetBombset();
                                        this.hud_.UnselectAll();
                                        explosionMode_ = true;
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
                                                this.map_.AddNewEntity(currentBomb_.Entity.ToBomb(),
                                                                       currentBomb_.Coord);
                                                hud_.RemoveBombOfType(pattern);
                                                //hud_.UnselectAll(); Dot not unselect bomb after a put
                                                currentBomb_.Coord.X = -1;
                                                currentBomb_.Coord.Y = -1;
                                            }
                                            else
                                            {
                                                if (currentBomb_.Coord.X != -1)
                                                    map_.RemoveEntity(currentBomb_.Coord);
                                                currentBomb_.Coord = this.map_.GetCoordByPos(ret.Pos);
                                                currentBomb_.Entity = new VirtualBomb(pattern,
                                                                                      KaboomResources.Sprites[hud_.SelectedBombName()].Clone());
                                                if (
                                                    !(this.map_.AddNewEntity(currentBomb_.Entity, currentBomb_.Coord)))
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

                                break;
                            }
                    }
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
            this.spriteBatch_.Begin();
            this.spriteBatch_.Draw(KaboomResources.Textures["background4"], new Rectangle(0, 0, this.graphics_.PreferredBackBufferWidth, this.graphics_.PreferredBackBufferHeight), KaboomResources.Textures["background4"].Bounds, Color.White);
            this.spriteBatch_.End();

            base.Draw(gameTime);

            if (ended_)
            {
                if (ladder_.IsDisplay)
                    hud_.DrawLadder(gameTime, ladder_.GetLadder());
                else
                    hud_.DrawEnd(gameTime, lose_);
            }
        }

        /// <summary>
        /// Handle the end of the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        public void ManageEndGame(object sender, EventArgs ea)
        {
            score_.EndReached(hud_.RemainingTurns);
            if (ended_ == false)
                this.ladder_.AddEntry(this.hud_.GameInfos.Score.Score, "Mon Score");
            // TODO : Manage end game here
            ended_ = true;
            //this.Exit();
        }

        /// <summary>
        /// Activate Bombset in checkpoint
        /// </summary>
        public void BombSet(List<Hud.BombInfo> value)
        {
            this.hud_.BombSet = value;
        }
    }
}