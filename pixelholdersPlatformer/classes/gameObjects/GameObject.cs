using pixelholdersPlatformer.classes.Component;

namespace pixelholdersPlatformer.classes.gameObjects;

public class GameObject
{
    private List<Component.IComponent> _components;

    public float CoordX { get; set; }
    public float CoordY { get; set; }

    //in meter
    public float Width  { get; set; }
    public float Height { get; set; }


    public GameObject(float coordX, float coordY, float width, float height)
    {
        this.CoordX = coordX;
        this.CoordY = coordY;
        this.Width = width;
        this.Height = height;
    }

    public void Update()
    {
        foreach (var component in _components)
        {
            component.Update();
        }
    }
}
