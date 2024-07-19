using System;
using static SDL2.SDL;
using pixelholdersPlatformer.classes.gameObjects;

namespace pixelholdersPlatformer.classes.managers
{
    public class LevelManager
    {
        private int _currentLevel;
        private const int MaxLevel = 3; // Assuming 3 levels as per the description

        public LevelManager()
        {
            _currentLevel = 1;
            LoadCurrentLevel();
        }

        private void LoadCurrentLevel()
        {
            Console.WriteLine($"Loading Level {_currentLevel}.tmx");
        }

        public void AdvanceLevel()
        {
            if (_currentLevel < MaxLevel)
            {
                _currentLevel++;
                LoadCurrentLevel();
            }
            else
            {
                Console.WriteLine("Congratulations! You've completed all levels.");
                SDL_Delay(500);
                SDL_Quit();
            }
        }
    }
}