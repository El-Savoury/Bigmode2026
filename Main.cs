using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Screens;
using System;

namespace Bigmode_Game_Jam_2026
{
    public class Main : Core
    {
        // Window 
        private const string Title = "Bigmode";
        private const int WindowWidth = 1280;
        private const int WindowHeight = 720;
        private const bool IsFullscreen = false;

        // FPS
        private const int FixedFPS = 60;
        private const double FixedTimeStep = 1.0d / FixedFPS;

        // Screen resolution
        private const int NativeResWidth = 640;
        private const int NativeResHeight = 360;
        private Rectangle _renderDestination;

        public Main() : base(Title, WindowWidth, WindowHeight, IsFullscreen)
        {
            // Target fixed frame rate
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(FixedTimeStep);

            // Vsync
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();

            // Window resizing
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;
        }


        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Tileset texture atlas
            Texture2D tilesetTexture = Content.Load<Texture2D>("tileset");
            TextureAtlas atlas = new TextureAtlas(tilesetTexture);
            AssetManager.I.AddTextureAtlas("tilesetAtlas", atlas);

            // Cursor texture atlas
            Texture2D cursorTexture = Content.Load<Texture2D>("cursor");
            TextureAtlas cursorAtlas = TextureAtlas.FromGrid("cursorAtlas", cursorTexture, 80, 80);
            AssetManager.I.AddTextureAtlas("cursorAtlas", cursorAtlas);

            ScreenManager.I.RegisterScreen("gameplayScreen", new GameplayScreen(Graphics, NativeResWidth, NativeResHeight));
            ScreenManager.I.LoadScreens(Content);

            // Load startup screen
            ScreenManager.I.ActivateScreen("gameplayScreen");
            _renderDestination = CalcRenderDestination(ScreenManager.I.CurrentScreen.Width, ScreenManager.I.CurrentScreen.Height);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ScreenManager.I.UpdateScreens(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Screen screen = ScreenManager.I.CurrentScreen;

            if (screen != null)
            {
                RenderTarget2D screenRenderTarget = screen.DrawToRenderTarget(GraphicsDevice, SpriteBatch);
                GraphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin(SpriteSortMode.FrontToBack,
                                                         BlendState.AlphaBlend,
                                                         SamplerState.PointClamp,
                                                         DepthStencilState.Default,
                                                         RasterizerState.CullNone);

                SpriteBatch.Draw(screenRenderTarget, _renderDestination, Color.White);

                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }


        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            _renderDestination = CalcRenderDestination(ScreenManager.I.CurrentScreen.Width, ScreenManager.I.CurrentScreen.Height);
        }
    }
}
