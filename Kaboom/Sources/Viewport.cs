﻿using System;
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
        private Point mapDimensions_;
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
        /// <param name="mapWidth">Map width</param>
        /// <param name="mapHeight">Map length</param>
        public void Initialize(GraphicsDevice graphicsDevice, Map map, int mapWidth, int mapHeight)
        {
            this.graphicsDevice_ = graphicsDevice;
            this.maxZoom_ = new int[2];
            this.mapDimensions_ = new Point(mapWidth, mapHeight);

            this.map_ = map;
            this.oldOrientation_ = this.GetOrientation();

            if (this.GetOrientation() == DisplayOrientation.Portrait)
            {
                maxZoom_[1] = this.graphicsDevice_.Viewport.Width / this.mapDimensions_.X;
                if (maxZoom_[1] >
                    (this.graphicsDevice_.Viewport.Height - (int) (0.1 * this.graphicsDevice_.Viewport.Height)) / this.mapDimensions_.Y)
                    maxZoom_[1] = (this.graphicsDevice_.Viewport.Height - (int) (0.1 * this.graphicsDevice_.Viewport.Width)) /
                                  this.mapDimensions_.Y;
                maxZoom_[0] = this.graphicsDevice_.Viewport.Height / this.mapDimensions_.X;
                if (maxZoom_[0] >
                    (this.graphicsDevice_.Viewport.Width - (int) (0.1 * this.graphicsDevice_.Viewport.Height)) / this.mapDimensions_.Y)
                    maxZoom_[0] = (this.graphicsDevice_.Viewport.Width - (int) (0.1 * this.graphicsDevice_.Viewport.Width)) /
                                  this.mapDimensions_.Y;
                Camera.Instance.DimY = maxZoom_[1];
                Camera.Instance.DimX = maxZoom_[1];
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[1] * this.mapDimensions_.X)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height - (int) (0.1 * this.graphicsDevice_.Viewport.Width) -
                                        (maxZoom_[1] * this.mapDimensions_.Y)) / 2 + (int) (0.1 * this.graphicsDevice_.Viewport.Width);
            }
            else
            {
                maxZoom_[0] = this.graphicsDevice_.Viewport.Width / this.mapDimensions_.X;
                if (maxZoom_[0] >
                    (this.graphicsDevice_.Viewport.Height - (int) (0.1 * this.graphicsDevice_.Viewport.Height)) / this.mapDimensions_.Y)
                    maxZoom_[0] = (this.graphicsDevice_.Viewport.Height - (int) (0.1 * this.graphicsDevice_.Viewport.Height)) /
                                  this.mapDimensions_.Y;
                maxZoom_[1] = this.graphicsDevice_.Viewport.Height / this.mapDimensions_.X;
                if (maxZoom_[1] >
                    (this.graphicsDevice_.Viewport.Width - (int) (0.1 * this.graphicsDevice_.Viewport.Width)) / this.mapDimensions_.Y)
                    maxZoom_[1] = (this.graphicsDevice_.Viewport.Width - (int) (0.1 * this.graphicsDevice_.Viewport.Height)) /
                                  this.mapDimensions_.Y;
                Camera.Instance.DimY = maxZoom_[0];
                Camera.Instance.DimX = maxZoom_[0];
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[0] * this.mapDimensions_.X)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height - (int) (0.1 * this.graphicsDevice_.Viewport.Height) -
                                        (maxZoom_[0] * this.mapDimensions_.Y)) / 2 + (int) (0.1 * this.graphicsDevice_.Viewport.Height);
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
                    Camera.Instance.OffY = -1 * (where.Y * 80) + (int) (0.1 * this.graphicsDevice_.Viewport.Width) +
                                           this.graphicsDevice_.Viewport.Height / 2;
                else
                    Camera.Instance.OffY = -1 * (where.Y * 80) +
                                           (int) (0.1 * this.graphicsDevice_.Viewport.Height) +
                                           this.graphicsDevice_.Viewport.Height / 2;

            }
            catch (Exception)
            {
                Camera.Instance.OffX = -1 * ((this.mapDimensions_.X * 80) / 2) + this.graphicsDevice_.Viewport.Width / 2;
                if (this.GetOrientation() == DisplayOrientation.Portrait)
                    Camera.Instance.OffY = -1 * ((this.mapDimensions_.Y * 80) / 2) +
                                           (int) (0.1 * this.graphicsDevice_.Viewport.Width) +
                                           this.graphicsDevice_.Viewport.Height / 2;
                else
                    Camera.Instance.OffY = -1 * ((this.mapDimensions_.Y * 80) / 2) +
                                           (int) (0.1 * this.graphicsDevice_.Viewport.Height) +
                                           this.graphicsDevice_.Viewport.Height / 2;
            }
            Camera.Instance.DimX = 80;
            Camera.Instance.DimY = 80;
            this.IsZoomed = true;
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
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[1] * this.mapDimensions_.X)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height -
                                        (int) (0.1 * this.graphicsDevice_.Viewport.Width) -
                                        (maxZoom_[1] * this.mapDimensions_.Y)) / 2 +
                                       (int) (0.1 * this.graphicsDevice_.Viewport.Width);
            }
            else
            {
                Camera.Instance.DimY = maxZoom_[0];
                Camera.Instance.DimX = maxZoom_[0];
                Camera.Instance.OffX = (this.graphicsDevice_.Viewport.Width - (maxZoom_[0] * this.mapDimensions_.X)) / 2;
                Camera.Instance.OffY = (this.graphicsDevice_.Viewport.Height -
                                        (int) (0.1 * this.graphicsDevice_.Viewport.Height) -
                                        (maxZoom_[0] * this.mapDimensions_.Y)) / 2 +
                                       (int) (0.1 * this.graphicsDevice_.Viewport.Height);
            }
            this.IsZoomed = false;
        }

        /// <summary>
        /// Getter for zoom status
        /// </summary>
        public bool IsZoomed { get; private set; }
    }
}