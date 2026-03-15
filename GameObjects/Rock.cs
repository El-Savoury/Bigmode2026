using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Input;
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
    public class Rock : MovingTileObject
    {
        public Rock(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
            Direction = new Point(0, 0);
        }

        public override void LoadContent()
        {
            _sprite = new Sprite(_tilemap.Tileset.GetTileTexture(2));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void ResolveCollison(TileObject obj)
        {
            PushOutOfCollision(obj);
            if (Direction == Point.Zero && obj is MovingTileObject movingObj)
            {
                Direction = movingObj.Direction;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}








