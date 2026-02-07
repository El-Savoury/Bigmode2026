using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.Graphics;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;

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
            ScreenManager.LoadAllScreens(Graphics, Content);
            ScreenManager.ActivateScreen(ScreenType.Gameplay);

            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ScreenManager.GetActiveScreen().Update(gameTime);

            // TODO: Add your update logic here

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


        public static ContentManager GetContentManager()
        {
            return Content;
        }

    }
}
