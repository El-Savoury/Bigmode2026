using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class TileCursor : TileObject
    {
        #region Constants

        const int POS_OFFSET = 8;

        #endregion Constants





        #region Properties

        private TileObject _heldObject;
        private AnimatedSprite _animatedSprite;
        public bool IsHoldingObject => _heldObject != null;

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
            Index = currentIndex;
            Position = _tilemap.IndexToWorldPos(currentIndex.X, currentIndex.Y);

            // Update currently held object
            if (_heldObject != null)
            {
                UpdateCurrentObject(Index);
            }

            _animatedSprite.Update(gameTime);
        }

        #endregion Update





        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_heldObject != null) { _heldObject.Draw(spriteBatch); }

            Vector2 drawPos = new Vector2(Position.X - POS_OFFSET, Position.Y - POS_OFFSET);
            _animatedSprite.Draw(spriteBatch, drawPos);
        }

        #endregion Draw






        #region Util

        public void PickUpObject(Point index)
        {
            TileObject obj = TileObjectManager.I.GetObject(index);

            if (obj is Player || obj == null) { return; }

            _heldObject = obj;

            // Destroy this object on current tile
            TileObjectManager.I.DestroyObject(_heldObject);

            _animatedSprite.AnimationController.Stop();
            _animatedSprite.SetCurrentFrame(1);
        }


        public void DropObject(Point index)
        {
            // Check if placement tile is a valid placement
            bool tileOccupied = TileObjectManager.I.GetObject(index) != null;
            ushort tileType = _tilemap.GetTileType(index.X, index.Y, "defaultLayer");

            if (tileOccupied || tileType == TileType.Empty)
            {
                return;
            }

            UpdateCurrentObject(index);
            TileObjectManager.I.RegisterObject(_heldObject);
            _heldObject = null;

            _animatedSprite.AnimationController.Play(0);
        }


        private void UpdateCurrentObject(Point index)
        {
            _heldObject.Position = _tilemap.IndexToWorldPos(index.X, index.Y);
            _heldObject.Index = index;
        }

        #endregion Util
    }
}
