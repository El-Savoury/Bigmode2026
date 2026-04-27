using System;

namespace MonogameLibrary.Tilemaps
{
    [Flags]
    public enum TileFlags : byte
    {
        None = 0,
        Occupied = 1 << 3,
    }
}
