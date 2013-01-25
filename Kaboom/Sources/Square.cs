using System;
using System.Collections.Generic;
using System.Text;

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
    class Square
    {
        private SortedSet<IEntity> entities_;
        private Rectangle base_;

        public Square(Rectangle baseLoc)
        {
            this.entities_ = new SortedSet<IEntity>(new EntityComparer());
            this.base_ = baseLoc;
        }

        /// <summary>
        /// Add an entity to the square
        /// </summary>
        /// <param name="e">The entity to add</param>
        public void addEntity(IEntity e)
        {
            this.entities_.Add(e);
        }

        /// <summary>
        /// Call entities' update
        /// </summary>
        public void Update()
        {
        }

        /// <summary>
        /// Draw entities using SpriteBatch. Should be called between sb.begin() & sb.end()
        /// </summary>
        /// <param name="sb">SpriteBatch used to render testures</param>
        public void Draw(SpriteBatch sb, GameTime t)
        {
            foreach (var item in this.entities_)
            {
                item.Draw(sb, t, this.base_);
                if (item.Visibility == eVisibility.OPAQUE)
                    break;
            }
        }

        public static void Unitest()
        {
            Square sq = new Square(Rectangle.Empty);
            sq.addEntity(new UnitestEntity(0, null));
            sq.addEntity(new UnitestEntity(2, null));
            sq.addEntity(new UnitestEntity(5, null));
            sq.addEntity(new UnitestEntity(1, null));
            sq.addEntity(new UnitestEntity(2, null));

            // Put your breakpoint here
        }
    }
}
