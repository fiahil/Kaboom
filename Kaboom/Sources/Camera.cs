namespace Kaboom.Sources
{
    class Camera
    {
        public static Camera Instance = new Camera();

        /// <summary>
        /// Instanciate a new camera
        /// </summary>
        /// <param name="x">X start position of the camera</param>
        /// <param name="y">Y start position of the camera</param>
        /// <param name="dimx">X lenght of elements</param>
        /// <param name="dimy">Y lenght of elements</param>
        private Camera(int x = 10, int y = 10, int dimx = 10, int dimy = 10)
        {
            this.OffX = x;
            this.OffY = y;
            this.DimX = dimx;
            this.DimY = dimy;
        }
        
        public int OffX { get; set; }
        public int OffY { get; set; }
        public int DimX { get; set; }
        public int DimY { get; set; }
    }
}
