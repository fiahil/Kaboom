﻿using System.Globalization;
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

        public class BombInfo
        {
            public Pattern.Type Type;
            public SpriteSheet Sprite;
            public int Quantity;
            public bool Activated;

            public BombInfo(Pattern.Type type, SpriteSheet sprite, int quantity)
            {
                Type = type;
                Sprite = sprite;
                Quantity = quantity;
                Activated = false;
            }
        }

        private readonly SpriteBatch sb_;
        private int width_;
        private int height_;
        private bool isActive_;
        private int currentPos_;
        private readonly List<BombInfo> bombSet_;

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
        public bool AddBombOfType(Pattern.Type type, int number)
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
        public bool RemoveBombOfType(Pattern.Type type, int number)
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
            if (isActive_)
                return bombSet_[currentPos_].Type;
            return Pattern.Type.NoPattern;
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
        /// Display the hud
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.sb_.Begin();


            // au momment de l'activation on set une variable interne a la class pour eviter les boucle inutile
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
                                                             (int)(((padding) / 780.0) * this.width_),
                                                             (int)((81.0 / 125.0) * this.height_ * 0.2),
                                                             (int)((61.0 / 780.0) * this.width_),
                                                             (int)((61.0 / 780.0) * this.width_)));

                this.sb_.DrawString(KaboomResources.Fonts["default"],
                                    bombInfo.Quantity.ToString(CultureInfo.InvariantCulture),
                                    new Vector2(((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                                                (int)(((padding) / 780.0) * this.width_) +
                                                (int)(((61.0 / 780.0) * this.width_) / 2),
                                                (int)(((81.0 / 125.0) * this.height_) / 2)), Color.White);

                padding += 57;
            }

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