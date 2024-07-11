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
        Components.Add(new RenderingComponent());
    }

    public void AddComponent(IComponent component)
    {
        Components.Add(component);
    }

    public IComponent GetComponent(Component type)
    {
        switch (type)
        {
            case Component.Animatable:
                return Components.Where(t => t is AnimatableComponent) as AnimatableComponent;
            case Component.Audio:
                return Components.Where(t => t is AudioComponent) as AudioComponent;
            case Component.Collision:
                return Components.Where(t => t is CollisionComponent) as CollisionComponent;
            case Component.Movable:
                return Components.Where(t => t is MovableComponent) as MovableComponent;
            case Component.Physics:
                return Components.Where(t => t is PhysicsComponent) as PhysicsComponent;
            case Component.Rendering:
                return Components.Where(t => t is RenderingComponent) as RenderingComponent;
            default:
                return null;
        }
    }

    public void Update()
    {
        foreach (var component in Components)
        {
            component.Update();
        }
    }
}

public enum Component
{
    Animatable, Audio, Collision, Movable, Physics, Rendering
}