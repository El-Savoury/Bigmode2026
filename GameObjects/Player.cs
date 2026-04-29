using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

        public Player(Tilemap map, int xIndex, int yIndex) : base(map, xIndex, yIndex)
        {
            _currentState = State.Idle;
        }


        public override void LoadContent()
        {
            Spritesheet _spritesheet = new Spritesheet(AssetManager.I.GetTextureAtlas("tileset"));

            // Create spritesheet animation
            bool isLooping = true;
            _spritesheet.AddAnimation("idleAnim", TimeSpan.FromMilliseconds(300), isLooping, 12, 13);
            _spritesheet.AddAnimation("moveAnim", TimeSpan.FromMilliseconds(300), isLooping, 14);
            _spritesheet.AddAnimation("fallAnim", TimeSpan.FromMilliseconds(200), !isLooping, 17, 18, 19, 16);

            _animatedSprite = new AnimatedSprite(_spritesheet, "idleAnim");
        }


        public override void Update(GameTime gameTime)
        {
            if (_currentState == State.Idle)
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

                if (Direction != Point.Zero)
                {
                    _currentState = State.Move;
                }
            }

            if (Direction == Point.Zero && _currentState != State.Idle)
            {
                _currentState = State.Fall;
            }

            UpdateAnimation(gameTime);
            base.Update(gameTime);
        }


        private void UpdateAnimation(GameTime gameTime)
        {
            if (_currentState != _previousState)
            {
                switch (_currentState)
                {
                    case State.Idle:
                        _animatedSprite.SetAnimation("idleAnim");
                        break;
                    case State.Move:
                        _animatedSprite.SetAnimation("moveAnim");
                        break;
                    case State.Fall:
                        _animatedSprite.SetAnimation("fallAnim");
                        break;
                }
            }

            // Flip sprite facing direction based on move direction
            if (Direction.X == -1)
            {
                _animatedSprite.Effects = SpriteEffects.FlipHorizontally;
            }
            else if (Direction.X == 1)
            {
                _animatedSprite.Effects = SpriteEffects.None;
            }

            _animatedSprite.Update(gameTime);
        }

        
        public override void Draw(SpriteBatch spriteBatch)
        {
            _animatedSprite.Draw(spriteBatch, Position);
        }
    }
}
