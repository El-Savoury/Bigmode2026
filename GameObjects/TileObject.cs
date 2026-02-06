using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;


namespace Bigmode_Game_Jam_2026.GameObjects
{
    public enum State
    {
        Idle,
        Move,
        Fall
    }


    public class TileObject
    {
        private AnimatedSprite _sprite;
        private Tilemap _tilemap;
        private Vector2 _position;
        private State _currentState = State.Move;
        private float _speed = 200f;

        public Point Direction { get; set; }
        public Point Index { get; set; }

        public RectF Bounds => new RectF(_position, 64, 64);


        public TileObject(Tilemap tilemap, int xIndex, int yIndex)
        {
            Index = new Point(xIndex, yIndex);

            //_sprite = sprite;
            _tilemap = tilemap;
            _position = _tilemap.GetTileWorldPos(Index.X, Index.Y);

            Direction = new Point(1, 0);
        }

        public Point GetNextIndex(Point direction)
        {
            return new Point(Index.X + direction.X, Index.Y + direction.Y);
        }



        public void MoveToNextIndex(GameTime gameTime)
        {
            Point nextIndex = GetNextIndex(Direction);
            Vector2 targetPos = _tilemap.GetTileWorldPos(nextIndex.X, nextIndex.Y);

            _position.X += Direction.X * _speed * Utility.I.DeltaTime(gameTime);
            _position.Y += Direction.Y * _speed * Utility.I.DeltaTime(gameTime);

            // Make sure we dont overshoot the next tile pos when moving
            if (Direction.X == 1 && _position.X > targetPos.X ||
                Direction.Y == 1 && _position.Y > targetPos.Y ||
                Direction.X == -1 && _position.X < targetPos.X ||
                Direction.Y == -1 && _position.Y < targetPos.Y)

            {
                _position = targetPos;

                // On reaching next tile
                GetDirection();
            }
        }


        private void GetDirection()
        {
            Index = _tilemap.GetIndexfromWorldPos(_position);
            ushort currentTileType = _tilemap.GetTile("defaultLayer", Index.X, Index.Y).Type;

            switch (currentTileType)
            {
                case (ushort)TileType.Ice:

                    break;
                case (ushort)TileType.Empty:
                    Direction = Point.Zero;
                    break;
                case (ushort)TileType.Down:
                    Direction = new Point(0, 1);

                    break;
                case (ushort)TileType.Up:
                    Direction = new Point(0, -1);

                    break;
                case (ushort)TileType.Left:
                    Direction = new Point(-1, 0);

                    break;
                case (ushort)TileType.Right:
                    Direction = new Point(1, 0);

                    break;
                case (ushort)TileType.Win:
                    Direction = Point.Zero;
                    break;
            }
        }

        public void Collide(TileObject obj)
        {
            if (Collision.I.RectVsRect(Bounds, obj.Bounds))
            {
                obj.Direction = Direction;
                Direction = new Point(-Direction.X, -Direction.Y);
            }
        }


        public void Update(GameTime gameTime)
        {
            MoveToNextIndex(gameTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            Draw2D.I.DrawRect(spriteBatch, new RectF(_position.X, _position.Y, 64, 64), Color.Red * 0.5f);
        }
    }
}
