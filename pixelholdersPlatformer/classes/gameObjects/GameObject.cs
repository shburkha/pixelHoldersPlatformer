using pixelholdersPlatformer.classes.behaviours;
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
        foreach (IComponent component in Components)
        {
            switch (type)
            {
                case Component.Animatable:
                    if (component is AnimatableComponent)
                    {
                        return component as AnimatableComponent;
                    }
                    break;
                case Component.Audio:
                    if (component is AudioComponent)
                    {
                        return component as AudioComponent;
                    }
                    break;
                case Component.Collision:
                    if (component is CollisionComponent)
                    {
                        return component as CollisionComponent;
                    }
                    break;
                case Component.Movable:
                    if (component is MovableComponent)
                    {
                        return component as MovableComponent;
                    }
                    break;
                case Component.Physics:
                    if (component is PhysicsComponent)
                    {
                        return component as PhysicsComponent;
                    }
                    break;
                case Component.Rendering:
                    if (component is RenderingComponent)
                    {
                        return component as RenderingComponent;
                    }
                    break;
                case Component.Behaviour:
                    if (component is IBehaviour)
                    {
                        return component as IBehaviour;
                    }
                    break;

                default:
                    return null;
            }
        }
        return null;
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
    Animatable, Audio, Collision, Movable, Physics, Rendering, Behaviour
}