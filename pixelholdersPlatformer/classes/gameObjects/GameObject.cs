using pixelholdersPlatformer.classes.Component;

namespace pixelholdersPlatformer.classes.gameObjects;

public class GameObject
{
    private List<Component.IComponent> _components;

    public void Update()
    {
        foreach (var component in _components)
        {
            component.Update();
        }
    }
}