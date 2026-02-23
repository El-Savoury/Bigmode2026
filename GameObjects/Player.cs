using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Tilemaps;
using System;
using System.Collections.Generic;


namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class Player : TileObject
    {
        AnimatedSprite _sprite;
        Spritesheet _spritesheet;


        public Player(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
        }

        public override void LoadContent()
        {
            _spritesheet = new Spritesheet(AssetManager.I.GetTextureAtlas("player"));

            // create spritesheet animation
            _spritesheet.AddAnimation("playerAnim", TimeSpan.FromMilliseconds(200), 0, 1, 2, 3);

            _sprite = new AnimatedSprite(_spritesheet, "playerAnim");
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
            ReverseDirection();
            PushOutOfCollision(obj);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, Position);
        }
    }
}
