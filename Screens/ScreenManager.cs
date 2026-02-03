using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Bigmode_Game_Jam_2026
{
    /// <summary>
    /// Enumerator to store each screen.
    /// </summary>
    enum ScreenType
    {
        Title,
        Gameplay,
        None
    }

    /// <summary>
    /// Class to manage all screens.
    /// </summary>
    static class ScreenManager
    {
        #region rMembers

        static Dictionary<ScreenType, Screen> mScreens = new Dictionary<ScreenType, Screen>();
        static ScreenType mActiveScreen = ScreenType.None;

        #endregion rMembers






        #region rInitialise

        /// <summary>
        /// Load all screens but don't activate them.
        /// </summary>
        /// <param name="deviceManager">Graphics device</param>
        public static void LoadAllScreens(GraphicsDeviceManager deviceManager)
        {
            mScreens.Clear();

            LoadScreen(ScreenType.Gameplay, new GameplayScreen(deviceManager));
        }


        /// <summary>
        /// Load specific type of screen.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="screen"></param>
        private static void LoadScreen(ScreenType type, Screen screen)
        {
            mScreens.Add(type, screen);
            screen.LoadContent();
        }

        #endregion rInitialise 






        #region rUtility

        /// <summary>
        /// Get a screen of a certain type.
        /// </summary>
        /// <param name="type">Screen type you want to find</param>
        /// <returns>Screen of specified type, null if type does not exist</returns>
        public static Screen GetScreen(ScreenType type)
        {
            if (mScreens.ContainsKey(type))
            {
                return mScreens[type];
            }

            return null;
        }


        /// <summary>
        /// Get the currently active screen.
        /// </summary>
        /// <returns>Active screen reference, null if there is no active screen</returns>
        public static Screen GetActiveScreen()
        {
            if (mScreens.ContainsKey(mActiveScreen))
            {
                return mScreens[mActiveScreen];
            }

            return null;
        }


        /// <summary>
        /// Activate a screen of certain type.
        /// </summary>
        /// <param name="type">Screen type you want to activate</param>
        public static void ActivateScreen(ScreenType type)
        {
            if (!mScreens.ContainsKey(type))
            {
                return;
            }

            if (mScreens.ContainsKey(mActiveScreen))
            {
                mScreens[mActiveScreen].OnDeactivate();
            }

            mActiveScreen = type;
            mScreens[type].OnActivate();
        }

        #endregion rUtility
    }
}