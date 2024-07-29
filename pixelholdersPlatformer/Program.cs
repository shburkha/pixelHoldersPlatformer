using pixelholdersPlatformer.classes;

using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace pixelholdersPlatformer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SDL_Init(SDL_INIT_EVERYTHING);
            TTF_Init();
            Game game = new Game();
            game.StartGame();

        }
    }
}
