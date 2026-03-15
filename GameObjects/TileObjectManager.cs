using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Utilities;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class TileObjectManager : Singleton<TileObjectManager>
    {
        List<TileObject> _tileObjects = new List<TileObject>();


        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _tileObjects.Count; i++)
            {
                _tileObjects[i].Update(gameTime);
            }

            for (int i = 0; i < _tileObjects.Count; i++)
            {
                // Check all objects against each other for collisions
                for (int j = 0; j < _tileObjects.Count; j++)
                {
                    if (_tileObjects[i] != _tileObjects[j] && _tileObjects[i].Collide(_tileObjects[j]))
                    {
                        _tileObjects[i].ResolveCollison(_tileObjects[j]);
                        _tileObjects[j].ResolveCollison(_tileObjects[i]);
                    }
                }
            }
        }


        public void Draw(SpriteBatch spritebatch)
        {
            foreach (TileObject tileObject in _tileObjects)
            {
                tileObject.Draw(spritebatch);
            }
        }


        public TileObject GetObject(Point index)
        {
            for (int i = 0; i < _tileObjects.Count; i++)
            {
                if (_tileObjects[i].Index == index) return _tileObjects[i];
            }
            return null;
        }


        public void RegisterObject(TileObject tileObject)
        {
            _tileObjects.Add(tileObject);
            tileObject.LoadContent();
        }


        public void RemoveObject(TileObject tileObject)
        {
            _tileObjects.Remove(tileObject);
        }


        public void DestroyObject(TileObject tileObject)
        {
            RemoveObject(tileObject);
            tileObject = null;
        }

        public void Clear()
        {
            _tileObjects.Clear();
        }

        public void SetPos(ref TileObject obj, Vector2 pos)
        {
            int index = _tileObjects.IndexOf(obj);
            _tileObjects[index].Position = pos;
        }
    }
}
