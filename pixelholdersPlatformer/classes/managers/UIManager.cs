using pixelholdersPlatformer.classes.gameObjects;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace pixelholdersPlatformer.classes.managers;

public class UIManager
{
    private const String _fontPath = "assets/fonts/slkscr.ttf";
    public nint Font = TTF_OpenFont(_fontPath, 24);

    private Dictionary<Scene, List<TextElementSchema>> _textElementsByScene = new Dictionary<Scene, List<TextElementSchema>>();

    private List<TextElement> _textElements = new List<TextElement>();

    public Scene CurrentScene;

    public UIManager()
    {
        _textElementsByScene.Add(Scene.MainMenu, new List<TextElementSchema>([
            new TextElementSchema( 5, 5, 10, 3, "Platformer"), 
            new TextElementSchema( 5, 9, 5, 2, "Start", true),
            new TextElementSchema( 5, 10.5f, 5, 2, "Options", true),
            new TextElementSchema( 5, 12, 5, 2, "Exit", true)
            ]));
        _textElementsByScene.Add(Scene.Game, new List<TextElementSchema>());

        ChangeScene(Scene.MainMenu);
    }

    public void CreateTextElement(float coordX, float coordY, float width, float height, String text, bool isClickable = false)
    {
        _textElements.Add(new TextElement(coordX, coordY, width, height, text, isClickable));
    }

    public void ChangeScene(Scene scene)
    {
        //if (scene == CurrentScene) { return; }

        CurrentScene = scene;
        _textElements.Clear();
        foreach (var schema in _textElementsByScene[CurrentScene])
        {
            _textElements.Add(new TextElement(schema));
        }
    }

    public List<TextElement> GetCurrentSceneTextElements()
    {
        return _textElements;
    }
}

public struct TextElementSchema
{
    public float CoordX;
    public float CoordY;
    public float Width;
    public float Height;
    public String Text;
    public bool IsClickable;

    public TextElementSchema(float x, float y, float w, float h, String text, bool isClickable = false)
    {
        CoordX = x; CoordY = y; Width = w; Height = h; Text = text;
        IsClickable = isClickable;
    }
}

public enum Scene
{
    MainMenu, Game, Settings, GameOver, Win
}