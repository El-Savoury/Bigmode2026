using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Maths;
using MonogameLibrary.Tilemaps;
using MonogameLibrary.Utilities;
using System;
using System.Collections.Generic;


namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class Player : TileObject
    {
        AnimatedSprite _sprite;

        public Player(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
        }

        public void LoadContent(ContentManager content)
        {
            Texture2D texture = content.Load<Texture2D>("Sprites/player");
            TimeSpan delay = TimeSpan.FromMilliseconds(200);

            List<AnimationFrame> frames = new List<AnimationFrame>()
            {
                new AnimationFrame(new TextureRegion(texture, 0,0, 64,64),delay),
                new AnimationFrame(new TextureRegion(texture, 64,0, 64,64),delay),
                new AnimationFrame(new TextureRegion(texture, 126,0, 64,64),delay),
                new AnimationFrame(new TextureRegion(texture, 190,0, 64,64),delay),
            };

            Animation anim = new Animation(frames, false, false, true);
            _sprite = new AnimatedSprite(anim);
        }

        public override void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);

            if (_currentState == State.Move)
            {
                _sprite.AnimationController.Play();
            }
            else
            {
                _sprite.AnimationController.Stop();
            }

            base.Update(gameTime);
        }


        public override void ResolveCollison(TileObject obj)
        {
            if (Collide(obj))
            {
                obj.Direction = Direction;
                Direction = new Point(-Direction.X, -Direction.Y);

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

                obj.Position = _tilemap.GetTileWorldPos(obj.Index.X, obj.Index.Y);
            }           
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, Position);
        }
    }
}
