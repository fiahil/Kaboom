using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Kaboom.Sources
{
    class Map : DrawableGameComponent
    {
        private readonly Square[,] board_;
        private readonly SpriteBatch sb_;
        private readonly int sizeX_;
        private readonly int sizeY_;

        /// <summary>
        /// Initialize a new map
        /// </summary>
        /// <param name="g">Game instance</param>
        /// <param name="sb">Spritebatch used to draw textures</param>
        /// <param name="sizex">X map number of entities</param>
        /// <param name="sizey">Y map number of entities</param>
        public Map(Game g, SpriteBatch sb, int sizex, int sizey)
            : base(g)
        {
            this.sb_ = sb;
            this.board_ = new Square[sizex, sizey];
            this.sizeX_ = sizex;
            this.sizeY_ = sizey;

            for (var i = 0; i < this.sizeX_; i++)
            {
                for (var j = 0; j < this.sizeY_; j++)
                {
                    this.board_[i, j] = new Square(new Point(i, j));
                }
            }
        }

        /// <summary>
        /// Randomly fill the map with entities
        /// </summary>
        public void Randomize()
        {
            for (var i = 0; i < this.sizeX_; i++)
            {
                for (var j = 0; j < this.sizeY_; j++)
                {
                    this.board_[i, j].AddEntity(new UnitestEntity(0, KaboomResources.Textures["background1"]));
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Randomize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var item in this.board_)
            {
                item.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.sb_.Begin();
            foreach (var item in this.board_)
            {
                item.Draw(this.sb_, gameTime);
            }
            this.sb_.End();
        }
    }
}