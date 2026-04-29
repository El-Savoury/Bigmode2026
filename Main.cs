using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Utilities;
using System;
using System.Reflection.Metadata;

namespace Bigmode_Game_Jam_2026
{
    public class Main : Core
    {
        private const string TITLE = "Bigmode";
        private const int SCREEN_WIDTH = 1280;
        private const int SCREEN_HEIGHT = 720;
        private const bool FULLSCREEN = false;


        public Main() : base(TITLE, SCREEN_WIDTH, SCREEN_HEIGHT, FULLSCREEN)
        {
            // 60 FPS
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0d / 60d);

            // Vsync
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();
        }


        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Tileset texture atlas
            Texture2D tilesetTexture = Content.Load<Texture2D>("tileset");
            TextureAtlas tilesetAtlas = TextureAtlas.FromGrid("tileset", tilesetTexture, 64, 64);
            tilesetAtlas.AddRegion("tileset", 0, 0, tilesetTexture.Width, tilesetTexture.Height);
            AssetManager.I.AddTextureAtlas("tileset", tilesetAtlas);

            // Cursor texture atlas
            Texture2D cursorTexture = Content.Load<Texture2D>("cursor");
            TextureAtlas cursorAtlas = TextureAtlas.FromGrid("cursorAtlas", cursorTexture, 80, 80);
            AssetManager.I.AddTextureAtlas("cursorAtlas", cursorAtlas);

            ScreenManager.LoadAllScreens(Graphics, Content);
            ScreenManager.ActivateScreen(ScreenType.Gameplay);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ScreenManager.GetActiveScreen().Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw active screen.
            Screen screen = ScreenManager.GetActiveScreen();

            if (screen != null)
            {
                RenderTarget2D screenTargetRef = screen.DrawToRenderTarget(GraphicsDevice, SpriteBatch);
                GraphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin(SpriteSortMode.FrontToBack,
                                                         BlendState.AlphaBlend,
                                                         SamplerState.PointClamp,
                                                         DepthStencilState.Default,
                                                         RasterizerState.CullNone);

                Draw2D.I.DrawTexture(SpriteBatch, screenTargetRef, Vector2.Zero);

                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
