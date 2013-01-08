using System;
using System.Collections.Generic;
using System.Text;

namespace Kaboom.Sources
{
    class Camera
    {
        public static Camera Instance = new Camera();

        private int posX;
        private int posY;
        private int sizeX;
        private int sizeY;

        Camera(int x = 10, int y = 10, int dimx = 10, int dimy = 10)
        {
            this.posX = x;
            this.posY = y;
            this.sizeX = dimx;
            this.sizeY = dimy;
        }

        public int OffX { get { return this.posX; } set { this.posX = value; } }
        public int OffY { get { return this.posY; } set { this.posY = value; } }
        public int DimX { get { return this.sizeX; } set { this.sizeX = value; } }
        public int DimY { get { return this.sizeY; } set { this.sizeY = value; } }
    }
}
