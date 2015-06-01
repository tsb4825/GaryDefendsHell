using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Objects
{
    public static class PreLevelMaps
    {
        private static Dictionary<int, string> PreMapTextures;
        private static int _level;

        static PreLevelMaps()
        {
            PreMapTextures = new Dictionary<int, string>
            {
                {1, "StoryBoardExample"}
            };
        }

        public static bool DoesPreLevelExist(int level)
        {
            return PreMapTextures.ContainsKey(level);
        }

        public static string GetPreLevelTexture()
        {
            if (PreMapTextures.ContainsKey(_level))
            {
                return PreMapTextures[_level];
            }

            throw new Exception("Level Does Not Have PreLevel Story");
        }

        public static void SetLevel(int level)
        {
            _level = level;
        }

        public static int GetLevel()
        {
            return _level;
        }
    }
}
