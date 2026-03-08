using Bigmode_Game_Jam_2026.GameObjects;
using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Input;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;
using System.Dynamic;

namespace Bigmode_Game_Jam_2026
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    internal class GameplayScreen : Screen
    {
        #region rMembers

        const int MAP_SIZE = 10;
        const int TILE_WIDTH = 64;
        const int TILE_HEIGHT = 72;

        private Tilemap _tilemap;

        private Point _currentIndex = Point.Zero;
        private TileObject _currentObject;

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
            LoadMap();
        }

        private void LoadMap()
        {
            TextureRegion tileTextures = AssetManager.I.GetTextureAtlas("atlas").GetRegion("tileset");
            Tileset tileset = new Tileset("tileset", tileTextures, TILE_WIDTH, TILE_HEIGHT);

            Vector2 mapPos = new Vector2(GetScreenSize().Center.X - (TILE_WIDTH * MAP_SIZE / 2), GetScreenSize().Center.Y - (TILE_WIDTH * MAP_SIZE / 2));

            TilemapLoader _mapLoader = new TilemapLoader();
            _tilemap = _mapLoader.Load(mapPos, tileset, TILE_WIDTH, TILE_WIDTH, 10, 10, "Level1");
        }

        #endregion rInitialisation






        #region rUpdate

        /// <summary>
        /// Update game screen
        /// </summary>
        /// <param name="gameTime">Frame time</param>
        public override void Update(GameTime gameTime)
        {
            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.R))
            {
                Reset();
            }

            _currentIndex = GetCurrentIndex();

            // Update currently held object
            if (_currentObject != null)
            {
                _currentObject.Position = _tilemap.IndexToWorldPos(_currentIndex.X, _currentIndex.Y);
                _currentObject.Index = _currentIndex;
            }

            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.Space))
            {
                if (_currentObject == null)
                {
                    PickUpObject();
                }
                else
                {
                    DropObject();
                }
            }

            TileObjectManager.I.Update(gameTime);
        }


        private void PickUpObject()
        {
            _currentObject = TileObjectManager.I.GetObject(_currentIndex);

            // Destroy this object on current tile
            TileObjectManager.I.DestroyObject(_currentObject);
        }


        private void DropObject()
        {
            // Check if placement tile is a valid placement
            bool tileOccupied = TileObjectManager.I.GetObject(_currentIndex) != null;
            ushort tileType = _tilemap.GetTileType("defaultLayer", _currentIndex.X, _currentIndex.Y);

            if (tileOccupied || tileType == TileType.Empty)
            {
                return;
            }

            TileObjectManager.I.RegisterObject(_currentObject);
            _currentObject = null;
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

            // Draw cursor
            Vector2 cursorPos = _tilemap.IndexToWorldPos(_currentIndex.X, _currentIndex.Y);
            Draw2D.I.DrawRect(spriteBatch, (int)cursorPos.X, (int)cursorPos.Y, TILE_WIDTH, TILE_WIDTH, Color.Green * 0.7f);

            if (_currentObject != null) { _currentObject.Draw(spriteBatch); }

            spriteBatch.End();

            return mScreenTarget;
        }

        #endregion rDraw







        #region rUtility

        private Point GetCurrentIndex()
        {
            Point index = _currentIndex;

            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.Right))
            {
                index.X++;
            }
            else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.Left))
            {
                index.X--;
            }
            else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.Up))
            {
                index.Y--;
            }
            else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.Down))
            {
                index.Y++;
            }

            // Clamp index to bounds of tilemap
            index.X = Math.Clamp(index.X, 0, _tilemap.Width - 1);
            index.Y = Math.Clamp(index.Y, 0, _tilemap.Height - 1);

            return index;
        }


        private void Reset()
        {
            _currentObject = null;
            TileObjectManager.I.Clear();
            LoadMap();
        }

        #endregion rUtility
    }
}