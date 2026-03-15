using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Assets;
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
        bool collide = false;

        public Column(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
        }


        public override void LoadContent()
        {
            _sprite = new Sprite(_tilemap.Tileset.GetTileTexture(3));

            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            collide = false;
        }


        public override void ResolveCollison(TileObject obj)
        {
            collide = true;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Colour = collide ? Color.Red : Color.White;
            base.Draw(spriteBatch);
        }
    }
}
