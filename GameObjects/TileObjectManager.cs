using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.Graphics;
using MonogameLibrary.Utilities;
using System.Collections.Generic;

namespace Bigmode_Game_Jam_2026.GameObjects
{
    public class TileObjectManager : Singleton<TileObjectManager>
    {
        List<TileObject> _tileObjects = new List<TileObject>();


        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _tileObjects.Count; i++)
            {
                //for (int j = 0; j < _tileObjects.Count; j++)
                //{
                //    if (_tileObjects[i] != _tileObjects[j])
                //    {
                //        _tileObjects[i].ResolveCollison(_tileObjects[j]);
                //        _tileObjects[j].ResolveCollison(_tileObjects[i]);
                //    }
                //}

                _tileObjects[i].Update(gameTime);
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
    }
}
