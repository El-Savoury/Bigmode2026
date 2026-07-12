using Bigmode_Game_Jam_2026.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.Screens;
using MonogameLibrary.Input;
using MonogameLibrary.Tilemaps;
using System;


namespace Bigmode_Game_Jam_2026
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    public class GameplayScreen : Screen
    {
        private enum GameState
        {
            Edit,
            Play,
        }



        #region Constants

        private const int MapWidth = 10;
        private const int MapHeight = 10;

        private const int TotalLevels = 3;

        #endregion Constants





        #region Members

        private Tilemap _tilemap;
        private TileCursor _tileCursor;
        private TileEntity _currentObject;
        private Point _currentIndex = new Point(1, 1);
        private Point _previousIndex;

        private int _currentLvlNum = 1;
        private GameState _currentGameState = GameState.Play;

        #endregion Members






        #region Init

        /// <summary>
        /// Game screen constructor
        /// </summary>
        /// <param name="graphics">Graphics device</param>
        public GameplayScreen(GraphicsDeviceManager graphics, int width, int height) : base(graphics, width, height)
        {
        }


        /// <summary>
        /// Load content required for gameplay
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            LoadTilemap(content);

            _tileCursor = new TileCursor(_tilemap, _currentIndex.X, _currentIndex.Y);
        }


        private void LoadTilemap(ContentManager content)
        {
            Tileset tileset = Tileset.FromFile(content, "FilesXML/tilesetDefinition.xml");

            int mapX = Bounds.Center.X - (tileset.TileWidth * MapWidth / 2);
            int mapY = Bounds.Center.Y - (tileset.TileHeight * MapHeight / 2);

            _tilemap = new Tilemap(tileset, new Vector2(mapX, mapY), MapWidth, MapHeight);
            _tilemap.AddLayer("defaultLayer");

            // TODO: Load correct level by using file name string + level number
            _tilemap.LoadLevelFromFile(Main.Content, "FilesXML/level1.xml");
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
                if (_currentLvlNum > 1) { _currentLvlNum--; }
                Reset();
            }
            if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.NumPad3))
            {
                if (_currentLvlNum < TotalLevels) { _currentLvlNum++; }
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
                        _tileCursor.PickUpObject(_currentIndex, "objectLayer");
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
                _tilemap.Update(gameTime);
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
            graphicsDevice.SetRenderTarget(_renderTarget);
            graphicsDevice.Clear(Color.Red);

            spriteBatch.Begin();

            _tilemap.Draw(spriteBatch);

            if (_currentGameState == GameState.Edit)
            {
                _tileCursor.Draw(spriteBatch);
            }

            spriteBatch.End();

            return _renderTarget;
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
            index.X = Math.Clamp(index.X, 1, _tilemap.Columns - 2);
            index.Y = Math.Clamp(index.Y, 1, _tilemap.Rows - 2);

            return index;
        }


        private void Reset()
        {
            _currentObject = null;
            LoadTilemap(Main.Content);
        }


        private void ToggleGameState()
        {
            _currentGameState = _currentGameState == GameState.Edit ? GameState.Play : GameState.Edit;
        }

        #endregion Util
    }
}