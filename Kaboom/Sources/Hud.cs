using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    class Hud : DrawableGameComponent
    {
        public enum EHudAction
        {
            Bomb1,
            Bomb2,
            Bomb3,
            Bomb4,
            Bomb5,
            SelectedBomb,
            Detonator,
            NoAction
        }

        private readonly SpriteBatch sb_;
        private int width_;
        private int height_;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">Main game parameter</param>
        /// <param name="sb">Main spriteBatch to display hud elements</param>
        public Hud(Game game, SpriteBatch sb)
            : base(game)
        {
            sb_ = sb;
            height_ = 0;
            width_ = 0;
        }

        /// <summary>
        /// Initialize hud elelment
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (this.Game.Window.CurrentOrientation == DisplayOrientation.Portrait)
                this.height_ = (int)(this.Game.GraphicsDevice.Viewport.Width * 0.15);
            else
                this.height_ = (int)(this.Game.GraphicsDevice.Viewport.Height * 0.15);

            this.width_ = (int)(780.0 / 125.0 * this.height_);
        }

        /// <summary>
        /// Display the hud
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.sb_.Begin();
            this.sb_.Draw(KaboomResources.Textures["hud"],
                          new Rectangle((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2), 0,
                                        this.width_, this.height_),
                          new Rectangle(0, 0, 780, 125), Color.White);
            this.sb_.End();
        }

        /// <summary>
        /// Detect if the corrent tapped position is on a hud element or not
        /// </summary>
        /// <param name="pos">Position of the current tap</param>
        /// <returns></returns>
        public EHudAction GetHudEvent(Vector2 pos)
        {
            var detonator =
                new Rectangle(
                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                    (int)((652.0 / 780.0) * this.width_), 0, (int)((128.0 / 780.0) * this.width_), this.height_);
            if (detonator.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudAction.Detonator;
            var padding = 4;
            for (var i = 0; i < 5; ++i, padding += 57)
            {
                var bomb =
                    new Rectangle(
                        ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                        (int)(((padding) / 780.0) * this.width_), 0, (int)((61.0 / 780.0) * this.width_),
                        (int)((81.0 / 125.0) * this.height_));
                if (bomb.Contains(new Point((int)pos.X, (int)pos.Y)))
                    return (EHudAction)(i);
            }
            var selectedBomb =
                new Rectangle(
                    ((this.Game.GraphicsDevice.Viewport.Width / 2) - (this.width_ / 2)) +
                    (int)((296.0 / 780.0) * this.width_), 0, (int)((109.0 / 780.0) * this.width_),
                    (int)((119.0 / 125.0) * this.height_));
            if (selectedBomb.Contains(new Point((int)pos.X, (int)pos.Y)))
                return EHudAction.SelectedBomb;
            return EHudAction.NoAction;
        }
    }
}
