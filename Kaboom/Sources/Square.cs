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
        private Rectangle offset_;

        public Square(Rectangle offset)
        {
            this.entities_ = new SortedSet<IEntity>(new EntityComparer());
            this.offset_ = offset;
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
        public void Draw(SpriteBatch sb)
        {
            foreach (var item in this.entities_)
            {
                //item.Draw();
                if (item.Visibility == eVisibility.OPAQUE)
                    break;
            }
        }

        public static void Unitest()
        {
            Square sq = new Square(Rectangle.Empty);
            sq.addEntity(new UnitestEntity(0));
            sq.addEntity(new UnitestEntity(2));
            sq.addEntity(new UnitestEntity(5));
            sq.addEntity(new UnitestEntity(1));
            sq.addEntity(new UnitestEntity(2));

            // Put your breakpoint here
        }
    }
}
