using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Tilemaps.TilemapObjects;
using MonogameLibrary.Utilities;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public enum State
    {
        Idle,
        Move,
        Fall,
    }


    public abstract class MovingTileObject : TilemapObject
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
                MapIndex = GetNextIndex(Direction);
            }

            if (_moveTimer.ElapsedTime > MOVE_COOLDOWN_TIME)
            {
                GetDirection(MapIndex);
                Point nextIndex = GetNextIndex(Direction);

                TileTemplate nextTile = _tilemap.GetTileInfo(nextIndex, "defaultLayer");

                if (nextTile.IsSolid || _tilemap.GetTile(nextIndex, "defaultLayer").HasFlag(TileFlags.Occupied))
                {
                    ReverseDirection();
                    MapIndex = GetNextIndex(Direction);
                }
                else
                {
                    MapIndex = nextIndex;

                    // TODO: Set the tile we moved to as occupied
                    //_tilemap.GetTile(Index, "defaultLayer").AddFlag(TileFlags.Occupied);
                }

                _moveTimer.Reset();
            }

            Position = _tilemap.IndexToWorldPos(MapIndex.X, MapIndex.Y);

            _previousState = _currentState;
        }



        #endregion Update





        #region Util

        public Point GetNextIndex(Point direction)
        {
            return new Point(MapIndex.X + direction.X, MapIndex.Y + direction.Y);
        }


        protected void GetDirection(Point index)
        {
            Tile currentTile = _tilemap.GetTile(MapIndex.X, MapIndex.Y, "defaultLayer");

            switch (currentTile.TileType)
            {
                case TileType.Arrow:
                    Direction = CardinalDirExtension.ToPoint(currentTile.Rotation);

                    // TODO: Change the way rotation is incremented on tiles
                    // so they dont have to be reinstantiated each time
                    currentTile.Rotation++;
                    _tilemap.SetTile("defaultLayer", currentTile, MapIndex.X, MapIndex.Y);
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
