using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.Component;

public class RenderingComponent : IComponent
{
    public SDL_Rect BoundingBox;
    public RenderingComponent()
    {
        BoundingBox = new SDL_Rect();
    }

    public void Update()
    {
        
    }
};