using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public enum State
    {
        Idle,
        Move,
        Fall,
    }


    public abstract class MovingTileObject : TileObject
    {
        #region Constants

        private const float SPEED = 220f;

        #endregion Constants






        #region Members

        protected State _currentState = State.Move;
        protected bool _targetReached = false;
        public Point Direction { get; set; }

        #endregion Members







        #region Init

        protected MovingTileObject(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
            Direction = new Point(1, 0);
        }

        #endregion Init






        #region Update

        public override void Update(GameTime gameTime)
        {
            // If we are at target tile get our new direction
            if (_targetReached)
            {
                Index = _tilemap.GetIndexfromWorldPos(Position);
                GetDirection(Index);
                _targetReached = false;
            }


            // Move(targetPos, gameTime);



            Tile currentTile = _tilemap.GetTile("defaultLayer", Index.X, Index.Y);
            Enum currentTileType = currentTile.Type;

            switch (currentTileType)
            {
                case TileType.Ice:

                    break;

                case TileType.Empty:
                    _currentState = State.Fall;
                    Destroy();
                    break;

                case TileType.Win:
                    Direction = Point.Zero;
                    break;
            }
        }

        #endregion Update






        #region Collision

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

        #endregion Collision






        #region Utility

        public Point GetNextIndex(Point direction)
        {
            return new Point(Index.X + direction.X, Index.Y + direction.Y);
        }


        public void Move(GameTime gameTime)
        {
            // Use direction to get next target pos
            Point nextIndex = GetNextIndex(Direction);
            Vector2 targetPos = _tilemap.GetTileWorldPos(nextIndex.X, nextIndex.Y);

            Position.X += Direction.X * SPEED * Utility.I.DeltaTime(gameTime);
            Position.Y += Direction.Y * SPEED * Utility.I.DeltaTime(gameTime);

            // Make sure we dont overshoot the next tile pos when moving
            if (Direction.X == 1 && Position.X >= targetPos.X ||
                Direction.Y == 1 && Position.Y >= targetPos.Y ||
                Direction.X == -1 && Position.X <= targetPos.X ||
                Direction.Y == -1 && Position.Y <= targetPos.Y)
            {
                Position = targetPos;
                _targetReached = true;
            }
        }


        protected void GetDirection(Point index)
        {
            Enum currentTileType = _tilemap.GetTile("defaultLayer", Index.X, Index.Y).Type;

            switch (currentTileType)
            {
                case TileType.Empty:
                    Direction = Point.Zero;
                    break;
                case TileType.Down:
                    Direction = new Point(0, 1);
                    break;
                case TileType.Up:
                    Direction = new Point(0, -1);
                    break;
                case TileType.Left:
                    Direction = new Point(-1, 0);
                    break;
                case TileType.Right:
                    Direction = new Point(1, 0);
                    break;
                case TileType.Win:
                    Direction = Point.Zero;
                    break;

            }
        }


        public void ReverseDirection()
        {
            Direction = new Point(-Direction.X, -Direction.Y);
        }

        #endregion Utility
    }
}
