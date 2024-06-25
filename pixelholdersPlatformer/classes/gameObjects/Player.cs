using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;

namespace pixelholdersPlatformer.gameObjects;

public class Player : GameObject
{
    public Player(float coordX, float coordY, float width, float height) : base(coordX, coordY, width, height)
    {
        this.Components.Add(new MovableComponent(this));
        this.Components.Add(new PhysicsComponent(this));
        this.Components.Add(new CollisionComponent(this));
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).IsFallable = true;


    }

    public void MovePlayer(float amountX, float amountY)
    { 
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).SetVelocity(amountX, amountY);
    }
    public void MovePlayerX(float amountX)
    {
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).SetVelocityX(amountX);
    }
    public void MovePlayerY(float amountY)
    {
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).SetVelocityY(amountY);
    }

    public void ResetPlayerPosition()
    {
        CoordX = 50;
        CoordY = 50;
    }

}