using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Kaboom.Sources
{
    
    internal class Hud : DrawableGameComponent
    {
        public enum EHudAction
        {
            NoAction,
            BombDetonation,
            BombSelection,
            BombRotation
        }

        public enum EHudEndAction
        {
            NoAction,
            Reload,
            Menu,
            Ladder,
            Score,
            Next
        }

        public class GameProgressInfos
        {
            public int Round { get; set; }
            public int Score { get; set; }

            public GameProgressInfos(int r, int s)
            {
                Round = r;
                Score = s;
            }
        }


        public class BombInfo
        {
            public Pattern.Type Type;
            public SpriteSheet Sprite;
            public int Quantity;
            public bool Activated;
            public string Name;

            public BombInfo(Pattern.Type type, int quantity, string name)
            {
                Type = type;
                Sprite = KaboomResources.Sprites[name].Clone();
                Quantity = quantity;
                Activated = false;
                Name = name;
            }

            public BombInfo Clone()
            {
                return new BombInfo(this.Type, this.Quantity, this.Name);
            }
        }

        private readonly SpriteBatch sb_;
        private int width_;
        private int height_;
        private int heightEnd_;
        private int widthEnd_;
        private bool isActive_;
        private int currentPos_;
        private List<BombInfo> bombSet_;
        private readonly List<BombInfo> bombSetRef_;

        public List<BombInfo> BombSet
        {
            get
            {
                return bombSet_;
            }
            set
            {
                foreach (var bombInfo in value)
                {
                    var i = this.bombSetRef_.FindIndex(val => bombInfo.Type == val.Type);
                    if (i != -1)
                        this.bombSetRef_[i].Quantity += bombInfo.Quantity;
                    else
                        this.bombSetRef_.Add(bombInfo);
                }
                this.bombSet_ = this.bombSetRef_.Select(bomb => bomb.Clone()).ToList();
                this.isActive_ = false;
                this.currentPos_ = 0;
            }
        }

        public GameProgressInfos GameInfos { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">Main game parameter</param>
        /// <param name="sb">Main spriteBatch to display hud elements</param>
        public Hud(Game game, SpriteBatch sb)
            : base(game)
        {
            bombSet_ = new List<BombInfo>();
            bombSetRef_ = new List<BombInfo>();
            sb_ = sb;
            height_ = 0;
            width_ = 0;
            currentPos_ = 0;
            isActive_ = false;
            GameInfos = new GameProgressInfos(0, 0);
        }

        /// <summary>
        /// Initialize hud elelment
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            if (this.Game.Window.CurrentOrientation == DisplayOrientation.Portrait)
                this.widthEnd_ = (int)(this.GraphicsDevice.Viewport.Height * 0.9);
            else
                this.widthEnd_ = (int)(this.GraphicsDevice.Viewport.Width * 0.9);

            this.heightEnd_ = (int)(450.0 / 700.0 * this.widthEnd_);
            if (this.Game.Window.CurrentOrientation == DisplayOrientation.Portrait)
                this.height_ = (int)(this.Game.GraphicsDevice.Viewport.Width * 0.15);
            else
                this.height_ = (int)(this.Game.GraphicsDevice.Viewport.Height * 0.15);
            this.width_ = (int)(780.0 / 125.0 * this.height_);
        }

        /// <summary>
        /// Add the specified number of the specified bomb to the set
        /// </summary>
        /// <param name="type">the type of the targeted bomb</param>
        /// <param name="number">the number which will be add</param>
        /// <returns>return true if the specified type is in the set or else falses</returns>
        public bool AddBombOfType(Pattern.Type type, int number = 1)
        {
            foreach (var bombInfo in BombSet.Where(bombInfo => bombInfo.Type == type))
            {
                bombInfo.Quantity += number;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove the specified number of the specified bomb to the set
        /// </summary>
        /// <param name="type">the type of the targeted bomb</param>
        /// <param name="number">the number which will be sub</param>
        /// <returns>return true if the specified type is in the set or else false</returns>
        public bool RemoveBombOfType(Pattern.Type type, int number = 1)
        {
            foreach (var bombInfo in BombSet.Where(bombInfo => bombInfo.Type == type))
            {
                bombInfo.Quantity -= number;
                if (bombInfo.Quantity < 0)
                {
                    bombInfo.Quantity = 0;
                }
                else if (bombInfo.Quantity == 0)
                    this.UnselectAll();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the type of the corrently selected bomb or return NoPattern
        /// </summary>
        /// <returns></returns>
        public Pattern.Type SelectedBombType()
        {
            return isActive_ ? BombSet[currentPos_].Type : Pattern.Type.NoPattern;
        }

        /// <summary>
        /// Return current bomb name
        /// </summary>
        /// <returns></returns>
        public string SelectedBombName()
        {
            return isActive_ ? BombSet[currentPos_].Name : "";            
        }

        /// <summary>
        /// Update the hud bomb annimation
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var bombInfo in BombSet.Where(bombInfo => bombInfo.Activated))
            {
                bombInfo.Sprite.Update(gameTime);
            }
        }

        /// <summary>
        /// Unselect all selected bomb
        /// </summary>
        public void UnselectAll()
        {
            foreach (var bombInfo in BombSet.Where(bombInfo => bombInfo.Activated))
            {
                bombInfo.Activated = false;
                bombInfo.Sprite.ResetCurrentAnim();
            }
            isActive_ = false;
        }

        /// <summary>
        /// Display the hud
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.sb_.Begin();

            this.sb_.Draw(isActive_ ? KaboomResources.Textures["hud_active"] : KaboomResources.Textures["hud"],
                          new Rectangle((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2), 0,
                                        this.width_, this.height_),
                          new Rectangle(0, 0, 780, 125), Color.White);

            var padding = 4;

            foreach (var bombInfo in BombSet)
            {
                bombInfo.Sprite.Draw(this.sb_, gameTime, new Rectangle(
                                                             ((this.Game.GraphicsDevice.Viewport.Width / 2) -
                                                              (this.width_ / 2)) +
                                                             (int) (((padding) / 780.0) * this.width_),
                                                             (int) ((81.0 / 125.0) * this.height_ * 0.2),
                                                             (int) ((61.0 / 780.0) * this.width_),
                                                             (int) ((61.0 / 780.0) * this.width_)));

                this.sb_.DrawString(KaboomResources.Fonts["default"],
                                    bombInfo.Quantity.ToString(CultureInfo.InvariantCulture),
                                    new Vector2(((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                                                (int) (((padding) / 780.0) * this.width_) +
                                                (int) (((61.0 / 780.0) * this.width_) / 2),
                                                (int) (((81.0 / 125.0) * this.height_) / 2)), Color.White);

                padding += 57;
            }

            var scoreS = GameInfos.Score.ToString(CultureInfo.InvariantCulture);
            this.sb_.DrawString(KaboomResources.Fonts["default"],
                                scoreS,
                                new Vector2(
                                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                                    (int)((((780.0 - (150.0 + (14 * scoreS.Length))) / 780.0) * this.width_)),
                                    (int)(((67.0 / 125.0) * this.height_) / 2)), Color.White);

            padding = 0;
            if (GameInfos.Round < 10)
                padding = 5;
            this.sb_.DrawString(KaboomResources.Fonts["default"],
                                GameInfos.Round.ToString(CultureInfo.InvariantCulture),
                                new Vector2(
                                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                                    (int) ((((780.0 - (77.0 - padding)) / 780.0) * this.width_)),
                                    (int) (((100.0 / 125.0) * this.height_) / 2)), Color.White);
            this.sb_.End();
        }

        public void DrawLadder(GameTime gameTime, List<Ladder.LadderEntry> ladder)
        {
            this.sb_.Begin();
            // Image en fonction du nombre de point (pour etoile)
            this.sb_.Draw(KaboomResources.Textures["ladderScreen"],
                                   new Rectangle((this.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2),
                                                 (this.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2),
                                                 this.widthEnd_, this.heightEnd_), Color.White);

            var padding = 0;

            foreach (var ladderEntry in ladder)
            {
                if (padding == (100 * 5))
                    break;

                var scoreS = ladderEntry.Name;
                var toPadd = 14 - scoreS.Length;
                if (toPadd > 0)
                    for (var i = 0; i < toPadd; i++)
                    {
                        scoreS += " ";
                    }
                scoreS += " " + ladderEntry.Score.ToString(CultureInfo.InvariantCulture);
                this.sb_.DrawString(KaboomResources.Fonts["default"],
                                scoreS,
                                new Vector2(
                                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2))
                                    + (int)((((270.0) / 700.0) * this.widthEnd_)),
                                    ((this.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2))
                                    + (int)((((260.0 + padding) / 450.0) * this.heightEnd_) / 2)), Color.White);
                padding += 100;
            }

            this.sb_.End();
        }

        public void DrawEnd(GameTime gametime)
        {
                this.sb_.Begin();
                this.sb_.Draw(KaboomResources.Textures["endScreen"],
                                       new Rectangle((this.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2),
                                                     (this.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2),
                                                     this.widthEnd_, this.heightEnd_), Color.White);
                var scoreS = GameInfos.Score.ToString(CultureInfo.InvariantCulture);
                this.sb_.DrawString(KaboomResources.Fonts["default"],
                                             scoreS,
                                             new Vector2(
                                                 ((this.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2))
                                                 + (int)((((240.0 + (14 * scoreS.Length)) / 700) * this.widthEnd_)),
                                                 ((this.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2))
                                                 + (int)((325.0 / 450.0) * this.heightEnd_)), Color.White);
                this.sb_.End();
        }

        public EHudEndAction GetHudEndEvent(Vector2 pos)
        {
            // case = 109 w 82 h

            // 192 w 373 h
            var menu =
               new Rectangle(
                   ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2)) +
                   (int)((192.0 / 700.0) * this.widthEnd_), ((this.Game.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2)) +
                   (int)((373.0 / 450.0) * this.heightEnd_), (int)((109.0 / 700.0) * this.widthEnd_), (int)((82.0 / 450.0) * this.heightEnd_));
            if (menu.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudEndAction.Menu;

            var reload =
                          new Rectangle(
                   ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2)) +
                   (int)(((192.0 + 109.0) / 700.0) * this.widthEnd_), ((this.Game.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2)) +
                   (int)((373.0 / 450.0) * this.heightEnd_), (int)((109.0 / 700.0) * this.widthEnd_), (int)((82.0 / 450.0) * this.heightEnd_));
            if (reload.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudEndAction.Reload;

            var next =
                          new Rectangle(
                   ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2)) +
                   (int)(((192.0 + 109.0 + 109.0) / 700.0) * this.widthEnd_), ((this.Game.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2)) +
                   (int)((373.0 / 450.0) * this.heightEnd_), (int)((109.0 / 700.0) * this.widthEnd_), (int)((82.0 / 450.0) * this.heightEnd_));
            if (next.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudEndAction.Next;
 
            var ladder =
                          new Rectangle(
                   ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2)) +
                   (int)(((402) / 700.0) * this.widthEnd_), ((this.Game.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2)) +
                   (int)((305.0 / 450.0) * this.heightEnd_), (int)((109.0 / 700.0) * this.widthEnd_), (int)((82.0 / 450.0) * this.heightEnd_));
            if (ladder.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudEndAction.Ladder;

            // square 130w 51h
            // pos 388w 59h
            var score =
                         new Rectangle(
                  ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.widthEnd_ / 2)) +
                  (int)(((388) / 700.0) * this.widthEnd_), ((this.Game.GraphicsDevice.Viewport.Height / 2) - (this.heightEnd_ / 2)) +
                  (int)((59 / 450.0) * this.heightEnd_), (int)((130.0 / 700.0) * this.widthEnd_), (int)((51.0 / 450.0) * this.heightEnd_));
            if (score.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudEndAction.Score;

            return EHudEndAction.NoAction;

        }


        /// <summary>
        /// Detect if the corrent tapped position is on a hud element or not
        /// </summary>
        /// <param name="pos">Position of the current tap</param>
        /// <returns></returns>
        public EHudAction GetHudEvent(Vector2 pos)
        {
            var detonator =
                new Rectangle(
                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                    (int)((652.0 / 780.0) * this.width_), 0, (int)((128.0 / 780.0) * this.width_), this.height_);
            if (detonator.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudAction.BombDetonation;

            var padding = 4;
            for (var i = 0; i < 5; ++i, padding += 57)
            {
                var bomb =
                    new Rectangle(
                        ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                        (int)(((padding) / 780.0) * this.width_), 0, (int)((61.0 / 780.0) * this.width_),
                        (int)((81.0 / 125.0) * this.height_));
                if (i < this.BombSet.Count)
                {
                    if (bomb.Contains(new Point((int)pos.X, (int)pos.Y)))
                    {
                        if (BombSet[i].Quantity <= 0)
                            return EHudAction.BombSelection;
                        if (i != currentPos_ || this.BombSet[i].Activated == false)
                        {
                            this.BombSet[currentPos_].Activated = false;
                            this.BombSet[currentPos_].Sprite.ResetCurrentAnim();
                            this.BombSet[i].Activated = true;
                            isActive_ = true;
                        }
                        else
                        {
                            this.BombSet[i].Activated = false;
                            this.BombSet[i].Sprite.ResetCurrentAnim();
                            isActive_ = false;
                        }
                        this.currentPos_ = i;
                        return EHudAction.BombSelection;
                    }
                }
            }

            var rotation =
                new Rectangle(
                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                    (int)((296.0 / 780.0) * this.width_), 0, (int)((109.0 / 780.0) * this.width_),
                    (int)((119.0 / 125.0) * this.height_));
            return rotation.Contains(new Point((int)pos.X, (int)pos.Y)) ? EHudAction.BombRotation : EHudAction.NoAction;
        }

        /// <summary>
        /// Reset current bombset
        /// </summary>
        public void ResetBombset()
        {
            this.bombSet_ = this.bombSetRef_.Select(bomb => bomb.Clone()).ToList();
        }
    }
}