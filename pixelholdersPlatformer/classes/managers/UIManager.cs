using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace pixelholdersPlatformer.classes.managers;

public class UIManager
{
    private const String _fontPath = "assets/fonts/slkscr.ttf";
    private nint _font = TTF_OpenFont(_fontPath, 12);

    public UIManager()
    {

    }
}