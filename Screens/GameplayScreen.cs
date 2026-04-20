using Bigmode_Game_Jam_2026.GameObjects;
using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.Assets;
using MonogameLibrary.Entities;
using MonogameLibrary.Graphics;
using MonogameLibrary.Input;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        const int TILE_HEIGHT = 64;

        #endregion Constants





        #region Members

        private Tilemap _tilemap;
        private TileObject _currentObject;
        private Point _currentIndex = new Point(1, 1);
        private Point _previousIndex;
        private GameState _currentGameState = GameState.Play;

        int _levelNum = 1;
        const int TOTAL_LEVELS = 3;

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
            LoadMap(_levelNum);
        }


        private void LoadMap(int levelNumber)
        {
            TextureRegion tileTextures = AssetManager.I.GetTextureAtlas("tileset").GetRegion("tileset");
            Tileset tileset = new Tileset("tileset", tileTextures, TILE_WIDTH, TILE_HEIGHT);

            Vector2 mapPos = new Vector2(GetScreenSize().Center.X - (TILE_WIDTH * MAP_SIZE / 2), GetScreenSize().Center.Y - (TILE_HEIGHT * MAP_SIZE / 2));

            TilemapLoader _mapLoader = new TilemapLoader();

            string lvlFilePath = "level" + levelNumber;
            _tilemap = _mapLoader.Load(mapPos, tileset, TILE_WIDTH, TILE_HEIGHT, 10, 10, lvlFilePath);
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

            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.NumPad1))
            {
                if (_levelNum > 1) { _levelNum--; }
                Reset();
            }
            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.NumPad3))
            {
                if (_levelNum < TOTAL_LEVELS) { _levelNum++; }
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
            TileObject obj = TileObjectManager.I.GetObject(index);

            if (obj is Player || obj is Column) { return; }

            _currentObject = obj;
            _previousIndex = _currentIndex;

            // Destroy this object on current tile
            TileObjectManager.I.DestroyObject(_currentObject);
        }


        private void DropObject(Point index)
        {
            // Check if placement tile is a valid placement
            bool tileOccupied = TileObjectManager.I.GetObject(index) != null;
            ushort tileType = _tilemap.GetTileType(index.X, index.Y, "defaultLayer");

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

                // Draw cursor
                Vector2 cursorPos = _tilemap.IndexToWorldPos(_currentIndex.X, _currentIndex.Y);
                TextureRegion cursor = _tilemap.Tileset.GetTileTexture(8);
                cursor.Draw(spriteBatch, cursorPos, Color.White);
            }

            spriteBatch.End();

            return mScreenTarget;
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
            LoadMap(_levelNum);
        }


        private void ToggleGameState()
        {
            _currentGameState = _currentGameState == GameState.Edit ? GameState.Play : GameState.Edit;
        }

        #endregion Util
    }
}