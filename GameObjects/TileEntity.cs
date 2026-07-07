using Microsoft.Xna.Framework;
using MonogameLibrary.Entities;
using MonogameLibrary.Tilemaps;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    // An object in a tilemap that has behaviour 
    public abstract class TileEntity : DynamicEntity
    {
        protected Tilemap _tilemap;
        public Point MapIndex { get; set; }


        public TileEntity(Tilemap tilemap, int xIndex, int yIndex)
        {
            _tilemap = tilemap;
            MapIndex = new Point(xIndex, yIndex);
            Position = _tilemap.IndexToWorldPos(MapIndex.X, MapIndex.Y);
        }
    }
}
