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

            public BombInfo(Pattern.Type type, SpriteSheet sprite, int quantity, string name)
            {
                Type = type;
                Sprite = sprite;
                Quantity = quantity;
                Activated = false;
                Name = name;
            }
        }

        private readonly SpriteBatch sb_;
        private int width_;
        private int height_;
        private bool isActive_;
        private int currentPos_;
        private readonly List<BombInfo> bombSet_;
        public GameProgressInfos GameInfos { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">Main game parameter</param>
        /// <param name="sb">Main spriteBatch to display hud elements</param>
        /// <param name="bombSet">Different bomb type using during this game</param>
        public Hud(Game game, SpriteBatch sb, List<BombInfo> bombSet)
            : base(game)
        {
            sb_ = sb;
            height_ = 0;
            width_ = 0;
            currentPos_ = 0;
            bombSet_ = bombSet;
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
            foreach (var bombInfo in bombSet_.Where(bombInfo => bombInfo.Type == type))
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
        /// <param name="number">the number which will be add</param>
        /// <returns>return true if the specified type is in the set or else false</returns>
        public bool RemoveBombOfType(Pattern.Type type, int number = 1)
        {
            foreach (var bombInfo in bombSet_.Where(bombInfo => bombInfo.Type == type))
            {
                bombInfo.Quantity -= number;
                if (bombInfo.Quantity < 0)
                    bombInfo.Quantity = 0;
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
            return isActive_ ? bombSet_[currentPos_].Type : Pattern.Type.NoPattern;
        }

        public string SelectedBombName()
        {
            return isActive_ ? bombSet_[currentPos_].Name : "";            
        }

        /// <summary>
        /// Update the hud bomb annimation
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var bombInfo in bombSet_.Where(bombInfo => bombInfo.Activated))
            {
                bombInfo.Sprite.Update(gameTime);
            }
        }

        /// <summary>
        /// Unselect all selected bomb
        /// </summary>
        public void UnselectAll()
        {
            foreach (var bombInfo in bombSet_.Where(bombInfo => bombInfo.Activated))
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

            if (isActive_)
                this.sb_.Draw(KaboomResources.Textures["hud_active"],
                              new Rectangle((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2), 0,
                                            this.width_, this.height_),
                              new Rectangle(0, 0, 780, 125), Color.White);
            else
                this.sb_.Draw(KaboomResources.Textures["hud"],
                              new Rectangle((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2), 0,
                                            this.width_, this.height_),
                              new Rectangle(0, 0, 780, 125), Color.White);

            var padding = 4;

            foreach (var bombInfo in bombSet_)
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

            var scoreS = GameInfos.Score.ToString(CultureInfo.InvariantCulture) + " Points";
            this.sb_.DrawString(KaboomResources.Fonts["default"],
                                scoreS,
                                new Vector2(
                                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                                    (int)((((780.0 - (150.0 + (14 * scoreS.Length))) / 780.0) * this.width_)),
                                    (int)(((70.0 / 125.0) * this.height_) / 2)), Color.White);

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
                if (i < this.bombSet_.Count)
                {
                    if (bomb.Contains(new Point((int)pos.X, (int)pos.Y)))
                    {
                        if (bombSet_[i].Quantity <= 0)
                            return EHudAction.BombSelection;
                        if (i != currentPos_ || this.bombSet_[i].Activated == false)
                        {
                            this.bombSet_[currentPos_].Activated = false;
                            this.bombSet_[currentPos_].Sprite.ResetCurrentAnim();
                            this.bombSet_[i].Activated = true;
                            isActive_ = true;
                        }
                        else
                        {
                            this.bombSet_[i].Activated = false;
                            this.bombSet_[i].Sprite.ResetCurrentAnim();
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
            if (rotation.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudAction.BombRotation;
            return EHudAction.NoAction;
        }

        #region Unitest
        /// <summary>
        /// Event Unitests
        /// </summary>
        public static void Unitest()
        {

        }
        #endregion
    }
}