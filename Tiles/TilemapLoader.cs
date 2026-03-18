using Bigmode_Game_Jam_2026.GameObjects;
using Microsoft.Xna.Framework;
using MonogameLibrary.Tilemaps;
using System.IO;
using MonogameLibrary.Utilities;

namespace Bigmode_Game_Jam_2026.Tiles
{
    public class TilemapLoader
    {
        #region rConstants

        const string EMPTY = "0";
        const string ICE = "1";
        const string WIN = "9";
        const string UP = "8";
        const string DOWN = "2";
        const string LEFT = "4";
        const string RIGHT = "6";
        const string PLAYER = "P";
        const string COLUMN = "C";
        const string ROCK = "R";

        #endregion rConstants


        public TilemapLoader()
        {
        }


        public Tilemap Load(Vector2 pos, Tileset tileset, int tileWidth, int tileHeight, int rows, int cols, string filePath)
        {
            Tilemap map = new Tilemap(tileset, pos, tileWidth, tileHeight, rows, cols);

            map.AddLayer("defaultLayer");
            map.AddTileType(TileType.Empty, 0);
            map.AddTileType(TileType.Ice, 1);
            map.AddTileType(TileType.Win, 9);
            map.AddTileType(TileType.Arrow, 5);          

            string[,] tileData = GetTileData(filePath);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Vector2 tilePos = new Vector2(pos.X + x * tileWidth, pos.Y + y * tileHeight);

                    switch (tileData[x, y])
                    {
                        case EMPTY:
                            map.SetTile("defaultLayer", new Tile(TileType.Empty), x, y);
                            break;

                        case ICE:
                            map.SetTile("defaultLayer", new Tile(TileType.Ice), x, y);
                            break;

                        case WIN:
                            map.SetTile("defaultLayer", new Tile(TileType.Win), x, y);
                            break;

                        case UP:
                            map.SetTile("defaultLayer", new Tile(TileType.Arrow, CardinalDir.Up), x, y);
                            break;

                        case DOWN:
                            map.SetTile("defaultLayer", new Tile(TileType.Arrow, CardinalDir.Down), x, y);
                            break;

                        case LEFT:
                            map.SetTile("defaultLayer", new Tile(TileType.Arrow, CardinalDir.Left), x, y);
                            break;

                        case RIGHT:
                            map.SetTile("defaultLayer", new Tile(TileType.Arrow, CardinalDir.Right), x, y);
                            break;

                        case PLAYER:
                            TileObjectManager.I.RegisterObject(new Player(map, x, y));
                            map.SetTile("defaultLayer", new Tile(TileType.Ice), x, y);
                            break;

                        case ROCK:
                            TileObjectManager.I.RegisterObject(new Rock(map, x, y));
                            map.SetTile("defaultLayer", new Tile(TileType.Ice), x, y);
                            break;

                        case COLUMN:
                            TileObjectManager.I.RegisterObject(new Column(map, x, y));
                            map.SetTile("defaultLayer", new Tile(TileType.Ice), x, y);
                            break;

                        default:
                            map.SetTile("defaultLayer", new Tile(TileType.Empty), x, y);
                            break;
                    }
                }

            }

            return map;
        }



        /// <summary>
        /// Load tilemap data from .txt file path
        /// </summary>
        /// <returns></returns>
        private static string[,] GetTileData(string filePath)
        {
            string[] mapData = File.ReadAllLines($"Content/Files/{filePath}.txt");
            var width = mapData[0].Length;
            var height = mapData.Length;

            string[,] tileData = new string[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    tileData[x, y] = mapData[y][x].ToString();
            }

            return tileData;
        }
    }
}