using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System.Reflection.PortableExecutable;


namespace Bigmode_Game_Jam_2026
{
    public class TileObject
    {
        private Tilemap _tilemap;
        private AnimatedSprite _sprite;
        private Vector2 _position;

        private Point _direction = new Point(0, 0);
        private float _speed = 200f;

        public Point CurrentIndex { get; set; }


        public TileObject(Tilemap tilemap, int xIndex, int yIndex)
        {
            CurrentIndex = new Point(xIndex, yIndex);

            //_sprite = sprite;
            _tilemap = tilemap;
            _position = _tilemap.GetTileWorldPos(CurrentIndex.X, CurrentIndex.Y);
        }

        public Point GetNextIndex(Point direction)
        {
            return new Point(CurrentIndex.X + direction.X, CurrentIndex.Y + direction.Y);
        }


        public void MoveToNextIndex(GameTime gameTime, Point direction)
        {
            Point nextIndex = GetNextIndex(direction);

            if (nextIndex.X > _tilemap.Width || nextIndex.X < 0 ||
               nextIndex.Y > _tilemap.Height || nextIndex.Y < 0)
            {
                return;
            }

            ushort nextTileType = _tilemap.GetTile("defaultLayer", nextIndex.X, nextIndex.Y).Type;

            if (nextTileType != (ushort)TileType.Empty)
            {
                Vector2 targetIndexPos = _tilemap.GetTileWorldPos(nextIndex.X, nextIndex.Y);

                if ((direction.X == 1 && _position.X < targetIndexPos.X) || (direction.Y == 1 && _position.Y < targetIndexPos.Y))
                {
                    _position.X += direction.X * _speed * Utility.I.DeltaTime(gameTime);
                    _position.Y += direction.Y * _speed * Utility.I.DeltaTime(gameTime);
                }

                CurrentIndex = _tilemap.GetIndexfromWorldPos(_position);
            }

        }


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Draw2D.I.DrawRect(spriteBatch, new RectF(_position.X, _position.Y, 64, 64), Color.Red);
        }
    }
}
