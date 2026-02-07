using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;
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
        public RectF Bounds => new RectF(Position.X, Position.Y, SIZE, SIZE);


        public TileObject(Tilemap tilemap, int xIndex, int yIndex)
        {
            Index = new Point(xIndex, yIndex);
            _tilemap = tilemap;
            Position = _tilemap.GetTileWorldPos(Index.X, Index.Y);
            Direction = new Point(1, 0);
        }

        public abstract void LoadContent();


        public Point GetNextIndex(Point direction)
        {
            return new Point(Index.X + direction.X, Index.Y + direction.Y);
        }


        public void MoveToNextIndex(GameTime gameTime)
        {
            Point nextIndex = GetNextIndex(Direction);

            // Check for collisions
            TileObject collisionObj = TileObjectManager.I.GetObject(nextIndex);

            if (collisionObj != null)
            {
                collisionObj.ResolveCollison(this);
                ResolveCollison(collisionObj);
                nextIndex = GetNextIndex(Direction);
            }

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
            if (_currentState == State.Fall) { return false; }
            return Collision.I.RectVsRect(Bounds, obj.Bounds);
        }


        public abstract void ResolveCollison(TileObject obj);


        public void PushOutOfCollision(TileObject obj)
        {
            RectF rect = obj.Bounds;

            float overlapX = Math.Min(Bounds.Right - rect.Left, rect.Right - Bounds.Left);
            float overlapY = Math.Min(Bounds.Bottom - rect.Top, rect.Bottom - Bounds.Top);

            if (overlapX < overlapY)
            {
                Position.X += Bounds.Centre.X > rect.Centre.X ? overlapX : -overlapX;

            }
            else
            {
                Position.Y += Bounds.Centre.Y > rect.Centre.Y ? overlapY : -overlapY;

            }

            obj.Position = _tilemap.GetTileWorldPos(obj.Index.X, obj.Index.Y);
        }



        public virtual void Update(GameTime gameTime)
        {
            MoveToNextIndex(gameTime);

            Tile currentTile = _tilemap.GetTile("defaultLayer", Index.X, Index.Y);
            ushort currentTileType = currentTile.Type;

            switch (currentTileType)
            {
                case (ushort)TileType.Ice:

                    break;

                case (ushort)TileType.Empty:
                    _currentState = State.Fall;
                    Destroy();
                    break;

                case (ushort)TileType.Win:
                    Direction = Point.Zero;
                    break;
            }
        }


        public void ReverseDirection()
        {
            Direction = new Point(-Direction.X, -Direction.Y);
        }


        public void Destroy()
        {
            TileObjectManager.I.DestroyObject(this);
        }

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
