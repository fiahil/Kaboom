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
        private Square[,] board_;
        private SpriteBatch sb_;
        private int sizeX_;
        private int sizeY_;

        public Map(Game g, SpriteBatch sb, int sizex, int sizey)
            : base(g)
        {
            this.sb_ = sb;
            this.board_ = new Square[sizex, sizey];
            this.sizeX_ = sizex;
            this.sizeY_ = sizey;

            for (int i = 0; i < this.sizeX_; i++)
            {
                for (int j = 0; j < this.sizeY_; j++)
                {
                    this.board_[i, j] = new Square(new Rectangle(i, j, 0, 0));
                }
            }
        }

        public void Randomize()
        {
            Random r = new Random();

            for (int i = 0; i < this.sizeX_; i++)
            {
                for (int j = 0; j < this.sizeY_; j++)
                {
                    switch (r.Next(3))
                    {
                        case 0:
                            this.board_[i, j].addEntity(new UnitestEntity(0, KaboomResources.textures["background1"]));
                            break;

                        case 1:
                            this.board_[i, j].addEntity(new UnitestEntity(0, KaboomResources.textures["background2"]));
                            break;

                        case 2:
                            this.board_[i, j].addEntity(new UnitestEntity(0, KaboomResources.textures["background3"]));
                            break;

                        default:
                            this.board_[i, j].addEntity(new UnitestEntity(0, KaboomResources.textures["background1"]));
                            break;
                    }

                    if (r.Next(2) == 0)
                    {
                        this.board_[i, j].addEntity(new UnitestEntity(1, KaboomResources.textures["pony"], eVisibility.TRANSPARENT));
                    }
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Randomize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var item in this.board_)
            {
                item.Update();
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