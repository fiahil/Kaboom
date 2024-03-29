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
            NoEvent,
            DragComplete
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
            TouchPanel.EnabledGestures = GestureType.Tap;
            this.oldDrag_ = Vector2.Zero;
            this.Pinch_ = Vector2.Zero;
        }

        /// <summary>
        /// Get one event and return the corresponding Action, that contain position and delta of gesture
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

              
                if (g.GestureType == GestureType.Pinch)
                {
                    ret.ActionType = Action.Type.Pinch;
                    
                    if (this.Pinch_ == Vector2.Zero)
                    {
                        this.Pinch_.X = (g.Position.X + g.Position2.X) / 2;
                        this.Pinch_.Y = (g.Position.Y + g.Position2.Y) / 2;
                    }

                    ret.Pos = this.Pinch_;
                    ret.DeltaX = (int)((Math.Abs(g.Delta.X) + Math.Abs(g.Delta2.X)));
                    ret.DeltaY = (int)((Math.Abs(g.Delta.Y) + Math.Abs(g.Delta2.Y)));

                    var dist = Math.Sqrt(Math.Pow(ret.DeltaX, 2) + Math.Pow(ret.DeltaY, 2)) * 0.2;

                    ret.DeltaX = ret.DeltaY = (int)dist;
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

                if (g.GestureType == GestureType.FreeDrag)
                {
                    if (oldDrag_ != Vector2.Zero)
                    {
                        ret.ActionType = Action.Type.Drag;
                        ret.DeltaX = (int) (g.Position.X - oldDrag_.X);
                        ret.DeltaY = (int) (g.Position.Y - oldDrag_.Y);
                        ret.Pos = g.Position;
                        this.oldDrag_ = g.Position;
                        return ret;
                    }
                    
                    this.oldDrag_ = g.Position;
                    ret.Pos = g.Position;
                }
                if (g.GestureType == GestureType.DragComplete)
                {
                    this.oldDrag_ = Vector2.Zero;
                    ret.ActionType = Action.Type.DragComplete;
                }
                if (g.GestureType == GestureType.PinchComplete)
                {
                    this.Pinch_ = Vector2.Zero;
                    Viewport.Instance.ResetPinch();
                }
            }
			var touchCollection = TouchPanel.GetState();
			foreach (var elt in touchCollection)
			{
				if (elt.State == TouchLocationState.Pressed)
				{
					ret.Pos = elt.Position;
					ret.ActionType = Action.Type.Drag;
				}
				if (elt.State == TouchLocationState.Released)
				{
					ret.Pos = elt.Position;
					ret.ActionType = Action.Type.DragComplete;
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