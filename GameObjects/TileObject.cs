using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System.Runtime.InteropServices;
using System.Threading;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public enum State
    {
        Idle,
        Move,
        Fall,
    }


    public abstract class TileObject
    {
        private const float SPEED = 220f;
        private const int SIZE = 64;

        protected Tilemap _tilemap;
        protected State _currentState = State.Move;

        public Vector2 Position;
        public Point Direction { get; set; }
        public Point Index { get; set; }
        public RectF Bounds => new RectF(Position.X, Position.Y, SIZE + 1, SIZE + 1);


        public TileObject(Tilemap tilemap, int xIndex, int yIndex)
        {
            Index = new Point(xIndex, yIndex);
            _tilemap = tilemap;
            Position = _tilemap.GetTileWorldPos(Index.X, Index.Y);
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

            Position.X += Direction.X * SPEED * Utility.I.DeltaTime(gameTime);
            Position.Y += Direction.Y * SPEED * Utility.I.DeltaTime(gameTime);

            // Make sure we dont overshoot the next tile pos when moving
            if (Direction.X == 1 && Position.X > targetPos.X ||
                Direction.Y == 1 && Position.Y > targetPos.Y ||
                Direction.X == -1 && Position.X < targetPos.X ||
                Direction.Y == -1 && Position.Y < targetPos.Y)
            {
                Position = targetPos;
                
                Index = _tilemap.GetIndexfromWorldPos(Position);

                GetDirection(Index);
            }
        }


        protected void GetDirection(Point index)
        {
            ushort currentTileType = _tilemap.GetTile("defaultLayer", Index.X, Index.Y).Type;

            switch (currentTileType)
            {
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


        public bool Collide(TileObject obj)
        {
            return (Collision.I.RectVsRect(Bounds, obj.Bounds));
        }

        public abstract void ResolveCollison(TileObject obj);


        public virtual void Update(GameTime gameTime)
        {
            Tile currentTile = _tilemap.GetTile("defaultLayer", Index.X, Index.Y);
            ushort currentTileType = currentTile.Type;

            MoveToNextIndex(gameTime);

            switch (currentTileType)
            {
                case (ushort)TileType.Ice:

                    break;
                case (ushort)TileType.Empty:

                    break;
                case (ushort)TileType.Win:
                    Direction = Point.Zero;
                    break;
            }
        }


        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
