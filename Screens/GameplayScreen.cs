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
using System.Reflection.Metadata;
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

        const int MAP_WIDTH = 10;
        const int MAP_HEIGHT = 10;
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

        TileCursor _tileCursor;

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

            _tileCursor = new TileCursor(_tilemap, _currentIndex.X, _currentIndex.Y);
        }


        private void LoadMap(int levelNumber)
        {
            TextureRegion tileTextures = AssetManager.I.GetTextureAtlas("tileset").GetRegion("tileset");
            Tileset tileset = new Tileset("tileset", tileTextures, TILE_WIDTH, TILE_HEIGHT);

            Vector2 mapPos = new Vector2(GetScreenSize().Center.X - (TILE_WIDTH * MAP_WIDTH / 2), GetScreenSize().Center.Y - (TILE_HEIGHT * MAP_HEIGHT / 2));

            TilemapLoader _mapLoader = new TilemapLoader();

            string lvlFilePath = "level" + levelNumber;
            _tilemap = _mapLoader.Load(mapPos, tileset, TILE_WIDTH, TILE_HEIGHT, MAP_HEIGHT, MAP_WIDTH, lvlFilePath);
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

            // Handle edit mode
            if (_currentGameState == GameState.Edit)
            {
                _currentIndex = GetCurrentIndex();
                _tileCursor.Update(gameTime, _currentIndex);

                // Move objects
                if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.Space))
                {
                    if (!_tileCursor.IsHoldingObject)
                    {
                        _tileCursor.PickUpObject(_currentIndex);
                        _previousIndex = _currentIndex;
                    }
                    else
                    {
                        _tileCursor.DropObject(_currentIndex);
                    }
                }
            }
            else
            {
                TileObjectManager.I.Update(gameTime);
            }

            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.E))
            {
                // Return current object to prev index
                if (_tileCursor.IsHoldingObject)
                {
                    _tileCursor.DropObject(_previousIndex);
                }

                ToggleGameState();
            }


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
                _tileCursor.Draw(spriteBatch);
            }

            spriteBatch.End();

            return mScreenTarget;
        }

        #endregion Draw







        #region Util

        private Point GetCurrentIndex()
        {
            Point index = _currentIndex;

            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.D))
            {
                index.X++;
            }
            else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.A))
            {
                index.X--;
            }
            else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.W))
            {
                index.Y--;
            }
            else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.S))
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