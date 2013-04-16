using System;
using System.IO;
using System.Xml.Serialization;
using Android.Content;
using Kaboom.Serializer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

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
        private readonly Ladder ladder_;
        private bool lose_;
        private bool explosionMode_;
        private readonly ScoreManager score_ = ScoreManager.Instance;
        private bool isTuto_;
        private readonly List<string> mapName_;
        private readonly List<string> tutoName_;
        private bool onHelp_;


        /// <summary>
        /// Create the game instance
        /// </summary>
        public MainGame(string level)
        {
            currentBomb_ = new CurrentElement();
            this.graphics_ = new GraphicsDeviceManager(this)
                                 {
                                     IsFullScreen = true,
                                     SupportedOrientations = DisplayOrientation.LandscapeLeft,
                                     PreferMultiSampling = true
                                 };
            this.level_ = level;
            ended_ = false;
            lose_ = false;
            this.em_ = new Event();
            this.ladder_ = new Ladder();


            #region mapNameInit

            mapName_ = new List<string>
                           {
                               "NumbaWan",
                               "DidUCheckTuto",
                               "It's Something",
                               "Versus",
                               "CombisTheG",
                               "InTheRedCorner",
                               "TheBreach",
                               "OppositeForces",
                               "XFactor",
                               "ChooseYourSide",
                               "DynamiteWarehouse",
                               "FaceToFace",
                               "OneStepAway",
                               "FindYourWayOut",
                               "Corporate",
                               "Unreachable",
                               "Tetris",
                               "Life",
                               "Invasion",
                               "A-Maze-Me"
                           };
            tutoName_ = new List<string>
                            {
                                "TutoNormalBomb",
                                "TutoLineBomb",
                                "TutoConeBomb",
                                "TutoXBomb",
                                "TutoCheckpointBS",
                                "TutoHBomb",
                                "TutoUltimateBomb",
                                "TutoBonusTNT"
                            };

            #endregion

            isTuto_ = tutoName_.Contains(this.level_);

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
            KaboomResources.Textures["endScreen2"] = Content.Load<Texture2D>("endScreen2");
            KaboomResources.Textures["endScreen3"] = Content.Load<Texture2D>("endScreen3");
            KaboomResources.Textures["failScreen"] = Content.Load<Texture2D>("failScreen");
            KaboomResources.Textures["ladderScreen"] = Content.Load<Texture2D>("ladderScreen");
            KaboomResources.Textures["help"] = Content.Load<Texture2D>("question-mark");
            if (isTuto_)
                KaboomResources.Textures["Tuto"] = Content.Load<Texture2D>(level_);
            KaboomResources.Textures["helpScreen"] = Content.Load<Texture2D>("helpScreen");

            KaboomResources.Sprites["BombUltimate"] = new SpriteSheet(KaboomResources.Textures["BombSheetUltimate"], new[] { 14, 14 }, 2, 20);
            KaboomResources.Sprites["BombSquare"] = new SpriteSheet(KaboomResources.Textures["BombSheetSquare"], new[] { 14, 14 }, 2, 20);
            KaboomResources.Sprites["BombAngle"] = new SpriteSheet(KaboomResources.Textures["BombSheetAngle"], new[] { 14, 14 }, 2, 20);
            KaboomResources.Sprites["BombBigSquare"] = new SpriteSheet(KaboomResources.Textures["BombSheetBigSquare"], new[] { 14, 14 }, 2, 20);
            KaboomResources.Sprites["BombLine"] = new SpriteSheet(KaboomResources.Textures["BombSheetLine"], new[] { 14, 14 }, 2, 20);
            KaboomResources.Sprites["BombSheetTNT"] = new SpriteSheet(KaboomResources.Textures["BombSheetTNT"], new[] { 1, 14 }, 2, 20);
            KaboomResources.Sprites["BombH"] = new SpriteSheet(KaboomResources.Textures["BombSheetH"], new[] { 14, 14 }, 2, 20);
            KaboomResources.Sprites["BombX"] = new SpriteSheet(KaboomResources.Textures["BombSheetX"], new[] { 14, 14 }, 2, 20);
            KaboomResources.Sprites["background2"] = new SpriteSheet(KaboomResources.Textures["background2"], new[] { 1, 15 }, 2, 20);
            KaboomResources.Sprites["background3"] = new SpriteSheet(KaboomResources.Textures["background3"], new[] { 1, 8 }, 2, 20);
            KaboomResources.Sprites["Ground"] = new SpriteSheet(KaboomResources.Textures["background1"], new[] { 1 }, 1);
            KaboomResources.Sprites["checkpoint"] = new SpriteSheet(KaboomResources.Textures["checkpoint"], new[] { 1, 14 }, 2);
            KaboomResources.Sprites["goal"] = new SpriteSheet(KaboomResources.Textures["goal"], new[] { 14, 14 }, 2);

            KaboomResources.Fonts["default"] = Content.Load<SpriteFont>("defaultFont");
            KaboomResources.Fonts["end"] = Content.Load<SpriteFont>("endFont");

            KaboomResources.Effects["DropBomb"] = Content.Load<SoundEffect>("Drop");
            KaboomResources.Effects["Explode"] = Content.Load<SoundEffect>("Explosion");
            KaboomResources.Effects["Detonate"] = Content.Load<SoundEffect>("Detonate");

            KaboomResources.Musics["InGame"] = Content.Load<Song>("InGame");

            KaboomResources.Level = LoadLevel(level_);
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
            score_.Restart(10);

            this.map_ = new Map(this, this.spriteBatch_, KaboomResources.Level);
            this.ladder_.AddEntry(KaboomResources.Level.Score.Score1, "King");
            this.ladder_.AddEntry(KaboomResources.Level.Score.Score2, "Prince");
            this.ladder_.AddEntry(KaboomResources.Level.Score.Score3, "Champion");
            this.hud_.GameInfos.Round = KaboomResources.Level.Score.Turn;
            this.map_.EndGameManager += ManageEndGame;
            Viewport.Instance.Initialize(GraphicsDevice, this.map_);

            this.Components.Add(this.map_);
            this.Components.Add(this.hud_);

            MediaPlayer.Play(KaboomResources.Musics["InGame"]);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
        }


        private void UpdateEnd(Action ret)
        {
             if (ret.ActionType == Action.Type.Tap)
             {
                 var hudEvent = this.hud_.GetHudEndEvent(ret.Pos);
                 switch (hudEvent)
                 {
                     case Hud.EHudEndAction.Menu:
                         MediaPlayer.IsRepeating = false;
                         MediaPlayer.Stop();
                         Activity.Finish();
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
                         MediaPlayer.IsRepeating = false;
                         MediaPlayer.Stop();
                         Activity.Finish();
                         Activity.StartActivity(new Intent(Activity, typeof (MainActivity)).PutExtra("level",
                                                                                                     this.level_));
                         break;
                     case Hud.EHudEndAction.Next:
                         MediaPlayer.IsRepeating = false;
                         MediaPlayer.Stop();
                         Activity.Finish();
                         int idx;
                         if ((idx = mapName_.IndexOf(level_)) != -1)
                         {
                             idx++;
                             if (idx >= mapName_.Count)
                                 Activity.StartActivity(new Intent(Activity, typeof (MenuActivity)));
                             else
                                 Activity.StartActivity(new Intent(Activity, typeof (MainActivity)).PutExtra("level",
                                                                                                             mapName_[
                                                                                                                 idx]));
                         }
                         else
                         {
                             idx = tutoName_.IndexOf(level_) + 1;
                             if (idx >= tutoName_.Count)
                                 Activity.StartActivity(new Intent(Activity, typeof (MainActivity)).PutExtra("level",
                                                                                                             mapName_[0]));
                             else
                                 Activity.StartActivity(new Intent(Activity, typeof (MainActivity)).PutExtra("level",
                                                                                                             tutoName_[
                                                                                                                 idx]));
                         }
                         break;
                 }
             }
        }


        private void UpdateSplash(Action ret)
        {
            if (ret.ActionType == Action.Type.Tap)
            {
                if (isTuto_)
                    isTuto_ = false; 
                else
                    onHelp_ = false;
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

            if (explosionMode_)
            {
                if (map_.NbExplosions == 0)
                {
                    explosionMode_ = false;
                    score_.EndOfTurn(hud_.TotalBombsNumber());
                }
            }
            else if (!ended_)
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
                else if (isTuto_ || onHelp_)
                {
                    UpdateSplash(ret);
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

                                        if (currentBomb_.Coord.X != -1)
                                        {
                                            this.map_.AddNewEntity(currentBomb_.Entity.ToBomb(),
                                                                       currentBomb_.Coord);
                                            currentBomb_.Coord.X = -1;
                                            currentBomb_.Coord.Y = -1;
                                        }

                                        map_.ActivateDetonators();

                                        hud_.GameInfos.Round -= 1;
                                        if (hud_.GameInfos.Round <= 0)
                                            hud_.GameInfos.Round = 0;

                                        this.hud_.ResetBombset();
                                        this.hud_.UnselectAll();
                                        explosionMode_ = true;
                                    }
                                    else if (hudEvent == Hud.EHudAction.BombLock)
                                    {
                                        if (currentBomb_.Coord.X != -1)
                                        {
                                            var pattern = hud_.SelectedBombType();

                                            this.map_.AddNewEntity(currentBomb_.Entity.ToBomb(),
                                                                   currentBomb_.Coord);
                                            hud_.RemoveBombOfType(pattern);
                                            currentBomb_.Coord.X = -1;
                                            currentBomb_.Coord.Y = -1;
                                            KaboomResources.Effects["DropBomb"].Play();
                                        }
                                    }
                                    else if (hudEvent == Hud.EHudAction.Help)
                                    {
                                        onHelp_ = true;
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

                                        if (this.map_.GetCoordByPos(ret.Pos) == currentBomb_.Coord)
                                        {
                                            if (pattern != Pattern.Type.NoPattern && currentBomb_.Coord.X != -1)
                                            {
                                                currentBomb_.Entity.NextOrientation();
                                                this.map_.AddNewEntity(currentBomb_.Entity, currentBomb_.Coord);
                                            }
                                        }
                                        else if (pattern != Pattern.Type.NoPattern)
                                        {
                                            if (currentBomb_.Coord.X != -1)
                                                map_.RemoveEntity(currentBomb_.Coord);
                                            currentBomb_.Coord = this.map_.GetCoordByPos(ret.Pos);
                                            currentBomb_.Entity = new VirtualBomb(pattern,
                                                                                  KaboomResources.Sprites[
                                                                                      hud_.SelectedBombName()].Clone
                                                                                      ());
                                            if (
                                                !(this.map_.AddNewEntity(currentBomb_.Entity, currentBomb_.Coord)))
                                            {
                                                currentBomb_.Coord.X = -1;
                                                currentBomb_.Coord.Y = -1;
                                            }
                                        }
                                    }
// ReSharper disable EmptyGeneralCatchClause
                                    catch
// ReSharper restore EmptyGeneralCatchClause
                                    {
                                    }
                                }

                                break;
                            }
                    }
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Stop();
                this.Exit();
            }
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

            if (isTuto_)
            {
                this.spriteBatch_.Begin();
                this.spriteBatch_.Draw(KaboomResources.Textures["Tuto"],
                                       new Rectangle((int)(this.graphics_.PreferredBackBufferWidth * 0.05),
                                                     (int)(this.graphics_.PreferredBackBufferHeight * 0.05),
                                                     (int)(this.graphics_.PreferredBackBufferWidth * 0.9),
                                                     (int)(this.graphics_.PreferredBackBufferHeight * 0.9)),
                                       KaboomResources.Textures["Tuto"].Bounds, Color.White);
                this.spriteBatch_.End();
            }
            else if (onHelp_)
            {
                this.spriteBatch_.Begin();
                this.spriteBatch_.Draw(KaboomResources.Textures["helpScreen"],
                                       new Rectangle((int)(this.graphics_.PreferredBackBufferWidth * 0.05),
                                                     (int)(this.graphics_.PreferredBackBufferHeight * 0.05),
                                                     (int)(this.graphics_.PreferredBackBufferWidth * 0.9),
                                                     (int)(this.graphics_.PreferredBackBufferHeight * 0.9)),
                                       KaboomResources.Textures["helpScreen"].Bounds, Color.White);
                this.spriteBatch_.End();
            }

            if (ended_)
            {
                if (ladder_.IsDisplay)
                    hud_.DrawLadder(gameTime, ladder_.GetLadder());
                else
                    hud_.DrawEnd(gameTime, lose_, ladder_.GetIdxOf("Player"));
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
                this.ladder_.AddEntry(this.hud_.GameInfos.Score.Score, "Player");
            ended_ = true;
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