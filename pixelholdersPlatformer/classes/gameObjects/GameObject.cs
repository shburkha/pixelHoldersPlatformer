using pixelholdersPlatformer.classes.Component;

namespace pixelholdersPlatformer.classes.gameObjects;

public class GameObject
{
    public List<IComponent> Components { get; private set; }

    public float CoordX { get; set; }
    public float CoordY { get; set; }

    //in gameunits
    public float Width  { get; set; }
    public float Height { get; set; }


    public GameObject(float coordX, float coordY, float width, float height)
    {
        Components = new List<IComponent>();
        this.CoordX = coordX;
        this.CoordY = coordY;
        this.Width = width;
        this.Height = height;
    }

    public void AddComponent(IComponent component)
    {
        Components.Add(component);
    }

    public void Update()
    {
        foreach (var component in Components)
        {
            component.Update();
        }
    }
}
