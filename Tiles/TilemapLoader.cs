using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonogameLibrary.Tilemaps;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Bigmode_Game_Jam_2026.Tiles
{
    public class TilemapLoader
    {
        #region rConstants

        const string EMPTY = "0";
        const string ICE = "1";
        const string WIN = "4";
        #endregion rConstants




        public Tilemap Load(Tileset tileset, Vector2 pos, int tileWidth, int tileHeight, int rows, int cols, string filePath)
        {

            Tilemap map = new Tilemap(tileset, pos, tileWidth, tileHeight, rows, cols);

            map.AddLayer("defaultLayer");
            map.AddTileType(TileType.Empty, 0);
            map.AddTileType(TileType.Ice, 1);
            map.AddTileType(TileType.Win, 4);


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
            string[] mapData = File.ReadAllLines($"Content/Files/Level1.txt");
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