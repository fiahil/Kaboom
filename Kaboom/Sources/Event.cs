using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace Kaboom.Sources
{
    class Action
    {
        public enum Type
        {
            Drag,
            ZoomIn,
            ZoomOut,
            Tap,
            Pinch,
            NoEvent
        }

        public Type ActionType;
        public int DeltaX;
        public int DeltaY;
        public Vector2 Pos;
    }

    class Event
    {

        private Vector2 oldDrag_;
        private Vector2 Pinch_;

        /// <summary>
        /// Enable gestures
        /// </summary>
        public Event()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.DragComplete | GestureType.DoubleTap | 
                                         GestureType.Tap | GestureType.Pinch | GestureType.PinchComplete;
            this.oldDrag_ = Vector2.Zero;
            this.Pinch_ = Vector2.Zero;
        }

        /// <summary>
        /// Get one event and return the correspondant Action, that contain position and delta of gesture
        /// </summary>
        public Action GetEvents()
        {
            var ret = new Action
                {
                    ActionType = Action.Type.NoEvent,
                    DeltaX = 0,
                    DeltaY = 0,
                    Pos = Vector2.Zero
                };

            if (TouchPanel.IsGestureAvailable)
            {
                var g = TouchPanel.ReadGesture();

                if (g.GestureType == GestureType.DoubleTap)
                {
                    ret.ActionType = Viewport.Instance.IsZoomed ? Action.Type.ZoomOut : Action.Type.ZoomIn;
                    ret.Pos = g.Position;
                    return ret;
                }
                if (g.GestureType == GestureType.Pinch)
                {
                    ret.ActionType = Action.Type.Pinch;
                    
                    if (this.Pinch_ == Vector2.Zero)
                    {
                        this.Pinch_.X = (g.Position.X + g.Position2.X) / 2;
                        this.Pinch_.Y = (g.Position.Y + g.Position2.Y) / 2;
                    }
                    System.Diagnostics.Debug.Print("----------------------------------------------------");
                    System.Diagnostics.Debug.Print("g.Position : {0}, g.Position2 : {1}", g.Position, g.Position2);
                    System.Diagnostics.Debug.Print("g.Delta : {0}, g.Delta2 : {1}", g.Delta, g.Delta2);

                    ret.Pos = this.Pinch_;
                    ret.DeltaX = (int)((Math.Abs(g.Delta.X) + Math.Abs(g.Delta2.X)) * 0.2);
                    ret.DeltaY = (int)((Math.Abs(g.Delta.Y) + Math.Abs(g.Delta2.Y)) * 0.2);

                    ret.DeltaX = ret.DeltaY = (ret.DeltaX > ret.DeltaY) ? ret.DeltaY  : ret.DeltaX;

                    if (g.Position.Y > g.Position2.Y && g.Delta.Y < 0 && g.Delta2.Y > 0 ||
                        g.Position.Y < g.Position2.Y && g.Delta.Y > 0 && g.Delta2.Y < 0)
                    {
                        ret.DeltaX = -1 * ret.DeltaX;
                        ret.DeltaY = -1 * ret.DeltaY;
                    }
                    return ret;
                }

                if (g.GestureType == GestureType.Tap)
                {
                    ret.ActionType = Action.Type.Tap;
                    ret.Pos = g.Position;
                    return ret;
                }

                if (Viewport.Instance.IsZoomed && g.GestureType == GestureType.FreeDrag)
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
                if (g.GestureType == GestureType.PinchComplete)
                {
                    this.Pinch_ = Vector2.Zero;
                    //Viewport.Instance.ResetPinch();
                }
            }
            return ret;
        }

        #region Unitest
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
        #endregion
    }
}