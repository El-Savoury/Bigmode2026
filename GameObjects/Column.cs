using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class Column : TileObject
    {
        private Sprite _sprite;

        public Column(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
            Direction = Point.Zero;
        }

        public override void LoadContent()
        {
            _sprite = new Sprite(_tilemap.Tileset.GetTileTexture(3));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void ResolveCollison(TileObject obj)
        {
            //if (Collide(obj))
            //{
            //    obj.ReverseDirection();
            //    obj.Position = _tilemap.GetTileWorldPos(obj.Index.X, obj.Index.Y);
            //}
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, Position);
        }
    }
}
