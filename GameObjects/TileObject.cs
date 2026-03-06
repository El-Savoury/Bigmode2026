using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Collisions;
using MonogameLibrary.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public abstract class TileObject
    {
        #region Constants

        private const int SIZE = 64;

        #endregion Constants







        #region Members

        protected Sprite _sprite;

        protected Tilemap _tilemap;

        public Vector2 Position;
        public Point Index { get; set; }
        public RectF Bounds => new RectF(Position.X, Position.Y, SIZE - 1, SIZE-1);

        #endregion Members






        #region Init

        public TileObject(Tilemap tilemap, int xIndex, int yIndex)
        {
            Index = new Point(xIndex, yIndex);
            _tilemap = tilemap;
            Position = _tilemap.IndexToWorldPos(Index.X, Index.Y);
        }

        public abstract void LoadContent();

        #endregion Init







        #region Update

        public abstract void Update(GameTime gameTime);

        #endregion Update





        #region Collision

        public bool Collide(TileObject obj)
        {
            return Collision.I.RectVsRect(Bounds, obj.Bounds);
        }


        public abstract void ResolveCollison(TileObject obj);
        
        #endregion Collision





        #region Draw

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, Position);
        }

        #endregion Draw




        #region Utility

        public void Destroy()
        {
            TileObjectManager.I.DestroyObject(this);
        }

        #endregion utility
    }
}
