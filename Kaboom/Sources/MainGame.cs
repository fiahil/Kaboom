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
    class MainGame : Game
    {
        private readonly GraphicsDeviceManager graphics_;
        private SpriteBatch spriteBatch_;
        private readonly Event em_;
        private Map map_;

        /// <summary>
        /// Create the game instance
        /// </summary>
        public MainGame()
        {
            this.graphics_ = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = true
                };
            em_ = new Event();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Load heavy content and resources
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch_ = new SpriteBatch(GraphicsDevice);

            KaboomResources.Textures["background1"] = Content.Load<Texture2D>("background1");
            KaboomResources.Textures["background2"] = Content.Load<Texture2D>("background2");
            KaboomResources.Textures["background3"] = Content.Load<Texture2D>("background3");
            KaboomResources.Textures["pony"] = Content.Load<Texture2D>("pony");
        }

        /// <summary>
        /// Initialise game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.map_ = new Map(this, this.spriteBatch_, 20, 20);
            this.Components.Add(this.map_);
        }

        /// <summary>
        /// Update game and game components
        /// </summary>
        /// <param name="gameTime">Game clock</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ///////////////////////////////////////////////////////TODO: Gesture test
            var ret = this.em_.GetEvents();
            while (ret.ActionType != Action.Type.NoEvent)
            {
                switch (ret.ActionType)
                {
                    case Action.Type.NoEvent:
                        break;
                    case Action.Type.Drag:
                        Camera.Instance.OffX += ret.DeltaX;
                        Camera.Instance.OffY += ret.DeltaY;
                        break;
                    case Action.Type.ZoomIn:
                        Camera.Instance.DimX = Camera.Instance.DimX - 30;
                        Camera.Instance.DimY = Camera.Instance.DimY - 30;
                        break;
                    case Action.Type.ZoomOut:
                        Camera.Instance.DimX = Camera.Instance.DimX + 30;
                        Camera.Instance.DimY = Camera.Instance.DimY + 30;
                        break;
                    case Action.Type.Tap:
                        this.map_.AddNewEntity(new UnitestEntity(2, KaboomResources.Textures["pony"], EVisibility.Transparent), this.map_.GetCoordByPos(ret.Pos));
                        break;
                }
                ret = this.em_.GetEvents();                 
            }
        }

        /// <summary>
        /// Draw game components
        /// </summary>
        /// <param name="gameTime">Game clock</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);

            base.Draw(gameTime);
        }
    }
}