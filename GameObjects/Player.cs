using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.Assets;
using MonogameLibrary.Graphics;
using MonogameLibrary.Input;
using MonogameLibrary.Tilemaps;
using System;

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
            if (Direction == Point.Zero)
            {
                if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.D))
                {
                    Direction = new Point(1, 0);
                }
                else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.A))
                {
                    Direction = new Point(-1, 0);
                }
                else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.W))
                {
                    Direction = new Point(0, -1);
                }
                else if (InputManager.I.KeyboardInput.IsKeyPressed(Keys.S))
                {
                    Direction = new Point(0, 1);
                }
            }

            UpdateAnimation(gameTime);
            base.Update(gameTime);
        }


        private void UpdateAnimation(GameTime gameTime)
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
