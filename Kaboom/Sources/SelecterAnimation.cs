namespace Kaboom.Sources
{
    class SelecterAnimation
    {
        public bool Enabled { get; set; }
        public bool FadeAnimation { get; private set; }
        public float Size { get; set; }

        public SelecterAnimation()
        {
            this.FadeAnimation = false;
            this.Size = 0f;
            this.Enabled = false;
        }

        public bool IsFading()
        {
            return this.Size <= 0.6f && Size > 0.0f;
        }

        public void Reset()
        {
            this.Size = 0.0f;
        }

        public void Start()
        {
            this.FadeAnimation = true;
            this.Size = 0.0f;
        }

        public void Stop()
        {
            this.FadeAnimation = false;
            this.Size = 0.6f;
        }

        public void Update()
        {
            if (this.FadeAnimation)
            {
                if (this.Size < 0.6f)
                    this.Size += 0.1f;
            }
            else
            {
                if (this.Size > 0.0f)
                this.Size -= 0.1f;
            }
        }
    }
}
