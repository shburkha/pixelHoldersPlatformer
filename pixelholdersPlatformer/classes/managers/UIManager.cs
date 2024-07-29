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

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }

    public UIManager()
    {
        _textElementsByScene.Add(Scene.MainMenu, new List<TextElementSchema>([
            new TextElementSchema( 5, 5, 10, 3, "Platformer"), 
            new TextElementSchema( 5, 9, 5, 2, "Start", true),
            new TextElementSchema( 5, 10.5f, 5, 2, "Options", true),
            new TextElementSchema( 15, 12, 5, 2, "Exit", true)
            ]));
        _textElementsByScene.Add(Scene.Game, new List<TextElementSchema>([]));
        _textElementsByScene.Add(Scene.GameOver, new List<TextElementSchema>([
            new TextElementSchema( 5, 5, 10, 3, "Game Over"),
            new TextElementSchema( 5, 9, 5, 2, "Play Again", true),
            new TextElementSchema( 5, 12, 5, 2, "Main Menu", true),
            new TextElementSchema( 15, 12, 5, 2, "Exit", true)
            ]));
        _textElementsByScene.Add(Scene.Win, new List<TextElementSchema>([
            new TextElementSchema( 5, 5, 10, 3, "You Win!"),
            new TextElementSchema( 5, 9, 5, 2, "Play Again", true),
            new TextElementSchema( 5, 12, 5, 2, "Main Menu", true),
            new TextElementSchema( 15, 12, 5, 2, "Exit", true)
            ]));
        _textElementsByScene.Add(Scene.Settings, new List<TextElementSchema>([
            new TextElementSchema( 5, 5, 10, 3, "Options"),
            new TextElementSchema( 5, 9, 5, 2, "Fullscreen:"),
            new TextElementSchema( 10.5f, 9, 3, 2, "Off", true),
            new TextElementSchema( 5, 12, 5, 2, "Main Menu", true)
            ]));

        ChangeScene(Scene.MainMenu);
    }

    public void CreateTextElement(float coordX, float coordY, float width, float height, String text, bool isClickable = false)
    {
        _textElements.Add(new TextElement(coordX, coordY, width, height, text, isClickable));
    }

    public void ToggleFullscreenStatus()
    {
        switch (_textElementsByScene[Scene.Settings][2].Text)
        {
            case "On":
                _textElementsByScene[Scene.Settings][2] = new TextElementSchema(10.5f, 9, 3, 2, "Off", true);
                _textElements[2].SetText("Off");
                break;
            case "Off":
                _textElementsByScene[Scene.Settings][2] = new TextElementSchema(10.5f, 9, 3, 2, "On", true);
                _textElements[2].SetText("On");
                break;
        }
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

    public void SetText(String text)
    {
        Text = text;
    }
}

public enum Scene
{
    MainMenu, Game, Settings, GameOver, Win
}