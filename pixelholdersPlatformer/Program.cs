using pixelholdersPlatformer.classes;

using static SDL2.SDL;


namespace pixelholdersPlatformer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SDL_Init(SDL_INIT_EVERYTHING);

            Game game = new Game();
            game.StartGame();

        }
    }
}
