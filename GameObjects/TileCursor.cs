using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Tilemaps.TilemapObjects;
using MonogameLibrary.Utilities;
using System;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class TileCursor : TilemapObject
    {
        #region Constants

        const int POS_OFFSET = 8;

        #endregion Constants





        #region Properties

        private TilemapObject _currentObject;
        private AnimatedSprite _animatedSprite;
        public bool IsHoldingObject => _currentObject != null;

        #endregion Properties





        #region Init

        public TileCursor(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
            LoadContent();
        }


        public override void LoadContent()
        {
            Spritesheet sheet = new Spritesheet(AssetManager.I.GetTextureAtlas("cursorAtlas"));
            sheet.AddAnimation("cursorAnim", TimeSpan.FromMilliseconds(300), true, 0, 1);
            sheet.AddAnimation("cursorAnimClosed", TimeSpan.FromMilliseconds(300), true, 1);
            _animatedSprite = new AnimatedSprite(sheet, "cursorAnim");
        }

        #endregion Init





        #region Update

        public override void Update(GameTime gameTime)
        {
        }


        public void Update(GameTime gameTime, Point currentIndex)
        {
            MapIndex = currentIndex;
            Position = _tilemap.IndexToWorldPos(currentIndex.X, currentIndex.Y);

            // Update currently held object
            if (_currentObject != null)
            {
                UpdateCurrentObject(MapIndex);
            }

            _animatedSprite.Update(gameTime);
        }

        #endregion Update





        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_currentObject != null) { _currentObject.Draw(spriteBatch); }

            Vector2 drawPos = new Vector2(Position.X - POS_OFFSET, Position.Y - POS_OFFSET);
            _animatedSprite.Draw(spriteBatch, drawPos);
        }

        #endregion Draw






        #region Util

        public void PickUpObject(Point index, string layer)
        {
        //    _currentObject = _tilemap.GetObject(index, layer);
        //    _tilemap.GetLayer(layer).RemoveObject(_currentObject);

            //TileObject obj = TileObjectManager.I.GetObject(index);

            //if (obj is Player || obj == null) { return; }

            //_currentObject = obj;

            //// Destroy this object on current tile
            //TileObjectManager.I.DestroyObject(_currentObject);

            //// Make tile not occupied
            //// TODO: Work out why we need to set a new tile
            //Tile tile = _tilemap.GetTile(index, "defaultLayer");
            //tile.RemoveFlag(TileFlags.Occupied);
            //_tilemap.SetTile("defaultLayer", tile, index.X, index.Y);

            _animatedSprite.AnimationController.Stop();
            _animatedSprite.SetCurrentFrame(1);
        }


        public void DropObject(Point index)
        {
            // Check if placement tile is a valid placement
            // bool tileOccupied = _tilemap.GetLayer("objectLayer").GetObject(index) != null;
            //int tileID = _tilemap.GetTile(index, "defaultLayer").TilesetID;

            //if (tileOccupied || _tilemap.GetTileInfo() == TileType.Empty)
            //{
            //    return;
            //}

            //UpdateCurrentObject(index);
            //TileObjectManager.I.RegisterObject(_currentObject);
            //_currentObject = null;

            //Tile tile = _tilemap.GetTile(index, "defaultLayer");
            
            //_tilemap.SetTile("defaultLayer", tile.TilesetID, index.X, index.Y);

            //_animatedSprite.AnimationController.Play(0);
        }


        private void UpdateCurrentObject(Point index)
        {
            _currentObject.Position = _tilemap.IndexToWorldPos(index.X, index.Y);
            _currentObject.MapIndex = index;
        }

        #endregion Util
    }
}
