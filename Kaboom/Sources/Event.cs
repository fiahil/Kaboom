using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

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

        /// <summary>
        /// Enable gestures
        /// </summary>
        public Event()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.DragComplete | GestureType.DoubleTap | GestureType.Tap;
            this.oldDrag_ = Vector2.Zero;
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