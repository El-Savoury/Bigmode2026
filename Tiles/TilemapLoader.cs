using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
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
        const string WIN = "9";
        const string UP = "8";
        const string DOWN = "2";
        const string LEFT = "4";
        const string RIGHT = "6";

        const int TILE_TEXTURE_WIDTH = 64;
        const int TILE_TEXTURE_HEIGHT = 72;

        #endregion rConstants


        #region rMembers

        private Tileset _tileset;

        #endregion rMembers


        public TilemapLoader()
        {
            // Load tileset
            Texture2D texture = Main.Content.Load<Texture2D>("tileset");
            TextureRegion region = new TextureRegion(texture, 0, 0, texture.Width, texture.Height);
            _tileset = new Tileset(region, TILE_TEXTURE_WIDTH, TILE_TEXTURE_HEIGHT);
        }


        public Tilemap Load(Vector2 pos, int tileWidth, int tileHeight, int rows, int cols, string filePath)
        {

            Tilemap map = new Tilemap(_tileset, pos, tileWidth, tileHeight, rows, cols);

            map.AddLayer("defaultLayer");
            map.AddTileType(TileType.Empty, 0);
            map.AddTileType(TileType.Ice, 1);
            map.AddTileType(TileType.Win, 2);
            map.AddTileType(TileType.Up, 5);
            map.AddTileType(TileType.Down, 4);
            map.AddTileType(TileType.Left, 6);
            map.AddTileType(TileType.Right, 7);

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
                            map.SetTile("defaultLayer", new Tile(TileType.Up), x, y);
                            break;

                        case DOWN:
                            map.SetTile("defaultLayer", new Tile(TileType.Down), x, y);
                            break;

                        case LEFT:
                            map.SetTile("defaultLayer", new Tile(TileType.Left), x, y);
                            break;

                        case RIGHT:
                            map.SetTile("defaultLayer", new Tile(TileType.Right), x, y);
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