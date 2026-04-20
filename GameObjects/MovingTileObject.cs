using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
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

        //  private const float SPEED = 220f;

        #endregion Constants






        #region Members

        protected Timer _moveTimer = new Timer();
        protected int moveCooldownTime = 200;

        protected State _currentState;
        protected State _previousState;

        //protected bool _targetReached = false;

        public Point Direction { get; set; }

        #endregion Members







        #region Init

        public MovingTileObject(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
            Direction = Point.Zero;
        }

        #endregion Init






        #region Update

        public override void Update(GameTime gameTime)
        {
            _moveTimer.Update(gameTime);

            if (_currentState == State.Move && !_moveTimer.IsRunning)
            {
                _moveTimer.Start();
                Index = GetNextIndex(Direction);
            }

            if (_moveTimer.ElapsedTime > moveCooldownTime)
            {
                Index = GetNextIndex(Direction);
                _moveTimer.Reset();
            }

            Position = _tilemap.IndexToWorldPos(Index.X, Index.Y);
            

            //// If we are at target tile get our new direction
            //if (_targetReached)
            //{
            //    Index = _tilemap.WorldPosToIndex(Bounds.Centre);
            //    GetDirection(Index);
            //    _targetReached = false;

            //    // Get any updates from current tile
            //    Tile currentTile = _tilemap.GetTile(Index.X, Index.Y);
            //    ushort currentTileType = currentTile.Type;

            //    switch (currentTileType)
            //    {
            //        case TileType.Arrow:
            //            currentTile.Rotation++;
            //            _tilemap.SetTile("defaultLayer", currentTile, Index.X, Index.Y);
            //            break;
            //        case TileType.Empty:
            //            _currentState = State.Fall;
            //            Destroy();
            //            break;
            //    }

            _previousState = _currentState;

            // TODO: Handle collisions here?

            //// Move to next index
            //Point nextIndex = GetNextIndex(Direction);
            //Vector2 targetPos = _tilemap.IndexToWorldPos(nextIndex.X, nextIndex.Y);
            //MoveToPos(targetPos, gameTime);
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

            obj.Position = _tilemap.IndexToWorldPos(obj.Index.X, obj.Index.Y);
        }

        #endregion Collision






        #region Utility

        public Point GetNextIndex(Point direction)
        {
            return new Point(Index.X + direction.X, Index.Y + direction.Y);
        }


        //public void MoveToPos(Vector2 targetPos, GameTime gameTime)
        //{
        //    Position.X += Direction.X * SPEED * Utility.I.DeltaTime(gameTime);
        //    Position.Y += Direction.Y * SPEED * Utility.I.DeltaTime(gameTime);

        //    // Make sure we dont overshoot the next tile pos when moving
        //    if (Direction.X == 1 && Position.X >= targetPos.X ||
        //        Direction.Y == 1 && Position.Y >= targetPos.Y ||
        //        Direction.X == -1 && Position.X <= targetPos.X ||
        //        Direction.Y == -1 && Position.Y <= targetPos.Y)
        //    {
        //        Position = targetPos;
        //        _targetReached = true;
        //    }
        //}


        protected void GetDirection(Point index)
        {
            Tile currentTile = _tilemap.GetTile(Index.X, Index.Y);

            switch (currentTile.Type)
            {
                case TileType.Arrow:
                    Direction = CardinalDirExtension.ConvertToPoint(currentTile.Rotation);
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
