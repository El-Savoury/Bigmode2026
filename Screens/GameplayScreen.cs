using Bigmode_Game_Jam_2026.GameObjects;
using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Tilemaps;

namespace Bigmode_Game_Jam_2026
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    internal class GameplayScreen : Screen
    {
        #region rMembers

        const int TILE_SIZE = 64;

        Tileset _tileset;
        Tilemap _tilemap;

        #endregion rMembers






        #region rInitialisation

        /// <summary>
        /// Game screen constructor
        /// </summary>
        /// <param name="graphics">Graphics device</param>
        public GameplayScreen(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        /// <summary>
        /// Load content required for gameplay
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            int mapSize = 10;
            Texture2D texture = content.Load<Texture2D>("tileset");
            TextureAtlas atlas = new TextureAtlas(texture);
            atlas.AddRegion("tileset", 0, 0, texture.Width, texture.Height);
            _tileset = new Tileset(atlas.GetRegion("tileset"), TILE_SIZE, TILE_SIZE);

            // Load map
            Vector2 mapPos = new Vector2(GetScreenSize().Center.X - (TILE_SIZE * mapSize / 2), GetScreenSize().Center.Y - (TILE_SIZE * mapSize / 2));

            TilemapLoader _mapLoader = new TilemapLoader();
            _tilemap = _mapLoader.Load(mapPos, TILE_SIZE, TILE_SIZE, 10, 10, "Level1");
        }

        #endregion rInitialisation






        #region rUpdate

        /// <summary>
        /// Update game screen
        /// </summary>
        /// <param name="gameTime">Frame time</param>
        public override void Update(GameTime gameTime)
        {
            TileObjectManager.I.Update(gameTime);
        }

        #endregion rUpdate






        #region rDraw

        /// <summary>
        /// Draw game screen to render target
        /// </summary>
        /// <param name="info">Info needed to draw</param>
        /// <returns>Render target with game screen drawn on it</returns>
        public override RenderTarget2D DrawToRenderTarget(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(mScreenTarget);
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            _tilemap.Draw(spriteBatch);
            TileObjectManager.I.Draw(spriteBatch);

            spriteBatch.End();

            return mScreenTarget;
        }

        #endregion rDraw







        #region rUtility

        #endregion rUtility
    }
}