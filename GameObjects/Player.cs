using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Tilemaps;
using System;
using System.Collections.Generic;


namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class Player : MovingTileObject
    {
        AnimatedSprite _animatedSprite;
        Spritesheet _spritesheet;


        public Player(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
        }

        public override void LoadContent()
        {
            _spritesheet = new Spritesheet(AssetManager.I.GetTextureAtlas("player"));

            // create spritesheet animation
            _spritesheet.AddAnimation("playerAnim", TimeSpan.FromMilliseconds(200), 0, 1, 2, 3);

            _animatedSprite = new AnimatedSprite(_spritesheet, "playerAnim");
        }


        public override void Update(GameTime gameTime)
        {
            _animatedSprite.Update(gameTime);

            if (_currentState == State.Move)
            {
                _animatedSprite.AnimationController.Play();
            }
            else
            {
                _animatedSprite.AnimationController.Stop();
            }

            base.Update(gameTime);
        }


        public override void ResolveCollison(TileObject obj)
        {
            PushOutOfCollision(obj);
            ReverseDirection();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            _animatedSprite.Draw(spriteBatch, Position);
        }
    }
}
