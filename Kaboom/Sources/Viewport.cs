using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Viewport Manager class
    /// Handle zoom & orientation
    /// </summary>
    class Viewport
    {
        private GraphicsDevice graphicsDevice_;
        private int[] maxZoom_;
        private Map map_;
        private DisplayOrientation oldOrientation_;

        public static Viewport Instance = new Viewport();

        /// <summary>
        /// Construct an empty Viewport
        /// </summary>
        private Viewport()
        {
            Instance = this;
            this.IsZoomed = false;
        }

        /// <summary>
        /// Initialize Viewport instance
        /// </summary>
        /// <param name="graphicsDevice">Graphic device used to fetch Viewport</param>
        /// <param name="map">Map</param>
        public void Initialize(GraphicsDevice graphicsDevice, Map map)
        {
            this.graphicsDevice_ = graphicsDevice;
            this.maxZoom_ = new int[2];

            this.map_ = map;
            this.oldOrientation_ = this.GetOrientation();

            if (this.GetOrientation() == DisplayOrientation.Portrait)
            {
                maxZoom_[1] = this.graphicsDevice_.Viewport.Width / this.map_.SizeX;
                if (maxZoom_[1] >
                    (this.graphicsDevice_.Viewport.Height - (int) (0.15 * this.graphicsDevice_.Viewport.Height)) / this.map_.SizeY)
                    maxZoom_[1] = (this.graphicsDevice_.Viewport.Height - (int) (0.15 * this.graphicsDevice_.Viewport.Width)) /
                                  this.map_.SizeY;
                maxZoom_[0] = this.graphicsDevice_.Viewport.Height / this.map_.SizeX;
                if (maxZoom_[0] >
                    (this.graphicsDevice_.Viewport.Width - (int) (0.15 * this.graphicsDevice_.Viewport.Height)) / this.map_.SizeY)
                    maxZoom_[0] = (this.graphicsDevice_.Viewport.Width - (int) (0.15 * this.graphicsDevice_.Viewport.Width)) /
                                  this.map_.SizeY;
                Camera.Instance.DimY = maxZoom_[1];
                Camera.Instance.DimX = maxZoom_[1];
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[1] * this.map_.SizeX)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height - (int) (0.15 * this.graphicsDevice_.Viewport.Width) -
                                        (maxZoom_[1] * this.map_.SizeY)) / 2 + (int) (0.15 * this.graphicsDevice_.Viewport.Width);
            }
            else
            {
                maxZoom_[0] = this.graphicsDevice_.Viewport.Width / this.map_.SizeX;
                if (maxZoom_[0] >
                    (this.graphicsDevice_.Viewport.Height - (int) (0.15 * this.graphicsDevice_.Viewport.Height)) / this.map_.SizeY)
                    maxZoom_[0] = (this.graphicsDevice_.Viewport.Height - (int) (0.15 * this.graphicsDevice_.Viewport.Height)) /
                                  this.map_.SizeY;
                maxZoom_[1] = this.graphicsDevice_.Viewport.Height / this.map_.SizeX;
                if (maxZoom_[1] >
                    (this.graphicsDevice_.Viewport.Width - (int) (0.15 * this.graphicsDevice_.Viewport.Width)) / this.map_.SizeY)
                    maxZoom_[1] = (this.graphicsDevice_.Viewport.Width - (int) (0.15 * this.graphicsDevice_.Viewport.Height)) /
                                  this.map_.SizeY;
                Camera.Instance.DimY = maxZoom_[0];
                Camera.Instance.DimX = maxZoom_[0];
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[0] * this.map_.SizeX)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height - (int) (0.15 * this.graphicsDevice_.Viewport.Height) -
                                        (maxZoom_[0] * this.map_.SizeY)) / 2 + (int) (0.15 * this.graphicsDevice_.Viewport.Height);
            }
        }

        /// <summary>
        /// Fetch the current orientation
        /// </summary>
        /// <returns>Current display orientation</returns>
        private DisplayOrientation GetOrientation()
        {
            return this.graphicsDevice_.Viewport.Height > this.graphicsDevice_.Viewport.Width ? DisplayOrientation.Portrait : DisplayOrientation.LandscapeLeft;
        }

        /// <summary>
        /// Update viewport orientation
        /// </summary>
        public void Update()
        {
            if (this.oldOrientation_ != this.GetOrientation())
            {
                this.ZoomOut();
                this.oldOrientation_ = this.GetOrientation();
            }
        }

        /// <summary>
        /// Zoom on the map. Unlock drag gesture
        /// </summary>
        /// <param name="pos">Location of the zoom</param>
        public void ZoomIn(Vector2 pos)
        {
            try
            {
                var where = this.map_.GetCoordByPos(pos);
                Camera.Instance.OffX = -1 * (where.X * 80) + this.graphicsDevice_.Viewport.Width / 2;
                if (this.GetOrientation() == DisplayOrientation.Portrait)
                    Camera.Instance.OffY = -1 * (where.Y * 80) + (int) (0.15 * this.graphicsDevice_.Viewport.Width) +
                                           this.graphicsDevice_.Viewport.Height / 2;
                else
                    Camera.Instance.OffY = -1 * (where.Y * 80) +
                                           (int) (0.15 * this.graphicsDevice_.Viewport.Height) +
                                           this.graphicsDevice_.Viewport.Height / 2;

            }
            catch (Exception)
            {
                Camera.Instance.OffX = -1 * ((this.map_.SizeX * 80) / 2) + this.graphicsDevice_.Viewport.Width / 2;
                if (this.GetOrientation() == DisplayOrientation.Portrait)
                    Camera.Instance.OffY = -1 * ((this.map_.SizeY * 80) / 2) +
                                           (int) (0.15 * this.graphicsDevice_.Viewport.Width) +
                                           this.graphicsDevice_.Viewport.Height / 2;
                else
                    Camera.Instance.OffY = -1 * ((this.map_.SizeY * 80) / 2) +
                                           (int) (0.15 * this.graphicsDevice_.Viewport.Height) +
                                           this.graphicsDevice_.Viewport.Height / 2;
            }
            Camera.Instance.DimX = 80;
            Camera.Instance.DimY = 80;
            this.IsZoomed = true;
        }

        /// <summary>
        /// Adjust the delta of the drag if the focus is out of range
        /// </summary>
        /// <param name="map">the map object of the game</param>
        /// <param name="deltaX">delta of the move on X axe</param>
        /// <param name="deltaY">delta of the move on Y axe</param>
        public void AdjustPos(Map map, ref int deltaX, ref int deltaY)
        {
            if (!IsZoomed)
            {
                return;
            }
            var vision = new Rectangle(80, 80, graphicsDevice_.Viewport.Width - 160, graphicsDevice_.Viewport.Height - 160);

            if (deltaY > 0)
            {
                // Top pas dans le square ok 
                if (map.LineIsContainHorizontaly(vision, 0))
                    deltaY = 0;
            }
            else
            {
                // Bot pas dans le square
                if (map.LineIsContainHorizontaly(vision, map.SizeY - 1))
                    deltaY = 0;
            }

            if (deltaX > 0)
            {
                // Left pas dans le square ok 
                if (map.LineIsContainVerticaly(vision, 0))
                    deltaX = 0;
            }
            else
            {
                // Right pas dans le square
                if (map.LineIsContainVerticaly(vision, map.SizeX - 1))
                    deltaX = 0;
            }
        }

        /// <summary>
        /// Un-Zoom. Lock drag gesture
        /// </summary>
        public void ZoomOut()
        {
            if (this.GetOrientation() == DisplayOrientation.Portrait)
            {
                Camera.Instance.DimY = maxZoom_[1];
                Camera.Instance.DimX = maxZoom_[1];
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[1] * this.map_.SizeX)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height -
                                        (int) (0.15 * this.graphicsDevice_.Viewport.Width) -
                                        (maxZoom_[1] * this.map_.SizeY)) / 2 +
                                       (int) (0.15 * this.graphicsDevice_.Viewport.Width);
            }
            else
            {
                Camera.Instance.DimY = maxZoom_[0];
                Camera.Instance.DimX = maxZoom_[0];
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[0] * this.map_.SizeX)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height -
                                        (int) (0.15 * this.graphicsDevice_.Viewport.Height) -
                                        (maxZoom_[0] * this.map_.SizeY)) / 2 +
                                       (int) (0.15 * this.graphicsDevice_.Viewport.Height);
            }
            this.IsZoomed = false;
        }

        /// <summary>
        /// Getter for zoom status
        /// </summary>
        public bool IsZoomed { get; private set; }
    }
}
