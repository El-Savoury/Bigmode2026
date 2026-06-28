using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Tilemaps;
using System;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class Rock : MovingTileObject
    {
        Sprite _sprite;

        public Rock(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
            Direction = new Point(0, 0);
        }


        public override void LoadContent()
        {
            _sprite = new Sprite(_tilemap.Tileset.GetTileTexture(2));
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}








