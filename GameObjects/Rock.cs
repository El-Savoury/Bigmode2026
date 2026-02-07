using Bigmode_Game_Jam_2026.Tiles;
using Microsoft.Xna.Framework;
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
    public class Rock : TileObject
    {
        private Sprite _sprite;

        public Rock(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
        }

        public void LoadContent(TextureRegion region)
        {
            _sprite = new Sprite(region);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void ResolveCollison(TileObject obj)
        {
            // Allow player to affect player object collison
            if (Collide(obj))
            {
                RectF rect = obj.Bounds;
                float overlapX = Math.Min(Bounds.Right - rect.Left, rect.Right - Bounds.Left);
                float overlapY = Math.Min(Bounds.Bottom - rect.Top, rect.Bottom - Bounds.Top);

                if (overlapX < overlapY)
                {
                    Position.X += Bounds.Centre.X > rect.Centre.X ? overlapX : -overlapX;

                }
                else
                {
                    Position.Y += Bounds.Centre.Y > rect.Centre.Y ? overlapY : -overlapY;

                }



                if (obj is Player)
                {
                    return;
                }

                //Direction = new Point(-Direction.X, -Direction.Y);
                //obj.Position = _tilemap.GetTileWorldPos(obj.Index.X, obj.Index.Y);

            }
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, Position);
        }
    }
}
