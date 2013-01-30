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
    struct Action
    {
        public enum Type
        {
            Drag,
            ZoomIn,
            ZoomOut,
            Tap,
            NoEvent
        };

        public Type ActionType;
        public int DeltaX;
        public int DeltaY;
        public Vector2 Pos;
    }

    class Event
    {

        private Vector2 oldDrag_;
        public bool isZoomed_;

        /// <summary>
        /// Enable gestures
        /// </summary>
        public Event()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.DragComplete | GestureType.DoubleTap | GestureType.Tap;
            this.oldDrag_ = Vector2.Zero;
            this.isZoomed_ = false;
        }

        /// <summary>
        /// Get one event and return the correspondant Action, that contain position and delta of gesture
        /// </summary>
        public Action GetEvents()
        {
            Action ret;
            ret.ActionType = Action.Type.NoEvent;
            ret.DeltaX = 0;
            ret.DeltaY = 0;
            ret.Pos = Vector2.Zero;
            if (TouchPanel.IsGestureAvailable)
            {
                var g = TouchPanel.ReadGesture();

                if (g.GestureType == GestureType.DoubleTap)
                {
                    ret.ActionType = this.isZoomed_ ? Action.Type.ZoomOut : Action.Type.ZoomIn;
                    this.isZoomed_ = !this.isZoomed_;
                    return ret;
                }
                if (g.GestureType == GestureType.Tap)
                {
                    ret.ActionType = Action.Type.Tap;
                    ret.Pos = g.Position;
                    return ret;
                }
                if (this.isZoomed_ && g.GestureType == GestureType.FreeDrag)
                {
                    if (oldDrag_ != Vector2.Zero)
                    {
                        ret.ActionType = Action.Type.Drag;
                        ret.DeltaX = (int) (g.Position.X - oldDrag_.X);
                        ret.DeltaY = (int) (g.Position.Y - oldDrag_.Y);
                        this.oldDrag_ = g.Position;
                        return ret;
                    }
                    
                    this.oldDrag_ = g.Position;
                }
                if (g.GestureType == GestureType.DragComplete)
                    this.oldDrag_ = Vector2.Zero;
            }
            return ret;
        }

        /// <summary>
        /// Event Unitests
        /// </summary>
        public static void Unitest()
        {
            var e = new Event();

            while (true)
            {
                var ret = e.GetEvents();
                while (ret.ActionType == Action.Type.NoEvent)
                {
                    ret = e.GetEvents();
                    // Do an action to leave the loop
                }
                // Put your breakpoint here
            }
        }
    }
}