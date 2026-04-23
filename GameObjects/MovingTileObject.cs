using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;

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

        protected int MOVE_COOLDOWN_TIME = 200;

        #endregion Constants






        #region Properties

        protected Timer _moveTimer = new Timer();

        protected State _currentState;
        protected State _previousState;

        public Point Direction { get; set; }

        #endregion Properties







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

            bool canMove = _currentState == State.Move && !_moveTimer.IsRunning;

            if (canMove)
            {
                _moveTimer.Start();
                Index = GetNextIndex(Direction);
            }

            if (_moveTimer.ElapsedTime > MOVE_COOLDOWN_TIME)
            {
                GetDirection(Index);
                Point nextIndex = GetNextIndex(Direction);

                if (_tilemap.GetTile(nextIndex, "defaultLayer").HasFlag(TileFlags.Solid))
                {
                    ReverseDirection();
                    Index = GetNextIndex(Direction);
                }
                else
                {
                    Index = nextIndex;
                }

                    _moveTimer.Reset();
            }

            Position = _tilemap.IndexToWorldPos(Index.X, Index.Y);

            _previousState = _currentState;
        }



        #endregion Update





        #region Util

        public Point GetNextIndex(Point direction)
        {
            return new Point(Index.X + direction.X, Index.Y + direction.Y);
        }


        protected void GetDirection(Point index)
        {
            Tile currentTile = _tilemap.GetTile(Index.X, Index.Y, "defaultLayer");

            switch (currentTile.Type)
            {
                case TileType.Arrow:
                    Direction = CardinalDirExtension.ConvertToPoint(currentTile.Rotation);

                    // TODO: Change the way rotation is incremeted on tiles
                    // so they dont have to be reinstantiated each time
                    currentTile.Rotation++;
                    _tilemap.SetTile("defaultLayer", currentTile, Index.X, Index.Y);
                    break;

                case TileType.Empty:
                    Direction = Point.Zero;
                    break;
            }
        }


        public void ReverseDirection()
        {
            Direction = new Point(-Direction.X, -Direction.Y);
        }

        #endregion Util
    }
}
