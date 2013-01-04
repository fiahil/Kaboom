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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public MainGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            KaboomResources.textures["background1"] = Content.Load<Texture2D>("background1");
            KaboomResources.textures["background2"] = Content.Load<Texture2D>("background2");
            KaboomResources.textures["background3"] = Content.Load<Texture2D>("background3");
            KaboomResources.textures["pony"] = Content.Load<Texture2D>("pony");
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Components.Add(new Map(this, this.spriteBatch, 20, 40));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);
            base.Draw(gameTime);
        }
    }
}