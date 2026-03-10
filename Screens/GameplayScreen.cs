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
using System.Reflection;

namespace Bigmode_Game_Jam_2026
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    internal class GameplayScreen : Screen
    {
        private enum GameState
        {
            Edit,
            Play,
        }



        #region Constants

        const int MAP_SIZE = 10;
        const int TILE_WIDTH = 64;
        const int TILE_HEIGHT = 72;

        #endregion Constants





        #region Members

        private Tilemap _tilemap;
        private TileObject _currentObject;
        private Point _currentIndex = new Point(1, 1);
        private Point _previousIndex;
        private GameState _currentGameState = GameState.Play;

        #endregion Members






        #region Init

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

        #endregion Init






        #region Update

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


            if (_currentGameState == GameState.Edit)
            {
                _currentIndex = GetCurrentIndex();

                // Update currently held object
                if (_currentObject != null)
                {
                    UpdateCurrentObject(_currentIndex);
                }

                // Move objects
                if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.Space))
                {
                    if (_currentObject == null)
                    {
                        PickUpObject(_currentIndex);
                    }
                    else
                    {
                        DropObject(_currentIndex);
                    }
                }
            }


            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.E))
            {
                // Return current object to prev index
                if (_currentObject != null)
                {
                    DropObject(_previousIndex);
                }

                ToggleGameState();
            }

            TileObjectManager.I.Update(gameTime);
        }


        private void PickUpObject(Point index)
        {
            _currentObject = TileObjectManager.I.GetObject(index);
            _previousIndex = _currentIndex;

            // Destroy this object on current tile
            TileObjectManager.I.DestroyObject(_currentObject);
        }


        private void DropObject(Point index)
        {
            // Check if placement tile is a valid placement
            bool tileOccupied = TileObjectManager.I.GetObject(index) != null;
            ushort tileType = _tilemap.GetTileType("defaultLayer", index.X, index.Y);

            if (tileOccupied || tileType == TileType.Empty)
            {
                return;
            }

            UpdateCurrentObject(index);
            TileObjectManager.I.RegisterObject(_currentObject);
            _currentObject = null;
        }

        private void UpdateCurrentObject(Point index)
        {
            _currentObject.Position = _tilemap.IndexToWorldPos(index.X, index.Y);
            _currentObject.Index = index;
        }

        #endregion Update






        #region Draw

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

            if (_currentGameState == GameState.Edit)
            {
                if (_currentObject != null) { _currentObject.Draw(spriteBatch); }

                //DrawTileGrid(spriteBatch);

                // Draw cursor
                Vector2 cursorPos = _tilemap.IndexToWorldPos(_currentIndex.X, _currentIndex.Y);
                TextureRegion cursor = _tilemap.Tileset.GetTileTexture(8);
                cursor.Draw(spriteBatch, cursorPos, Color.White);             
            }

            spriteBatch.End();

            return mScreenTarget;
        }


        private void DrawTileGrid(SpriteBatch spriteBatch)
        {
            TextureRegion region = _tilemap.Tileset.GetTileTexture(9);

            for (int x = 1; x < _tilemap.Width - 1; x++)
            {
                for (int y = 1; y < _tilemap.Height - 1; y++)
                {
                    Vector2 pos = new Vector2(_tilemap.Position.X + x * TILE_WIDTH, _tilemap.Position.Y + y * TILE_WIDTH);
                    region.Draw(spriteBatch, pos, Color.White);
                }
            }
        }

        #endregion Draw







        #region Util

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
            index.X = Math.Clamp(index.X, 1, _tilemap.Width - 2);
            index.Y = Math.Clamp(index.Y, 1, _tilemap.Height - 2);

            return index;
        }


        private void Reset()
        {
            _currentObject = null;
            TileObjectManager.I.Clear();
            LoadMap();
        }


        private void ToggleGameState()
        {
            _currentGameState = _currentGameState == GameState.Edit ? GameState.Play : GameState.Edit;
        }

        #endregion Util
    }
}