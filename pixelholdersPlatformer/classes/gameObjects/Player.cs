using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;

namespace pixelholdersPlatformer.gameObjects;

public class Player : GameObject
{
    public Player(float coordX, float coordY, float width, float height) : base(coordX, coordY, width, height)
    {
        this.CoordX = 50;
        this.CoordY = 50;
        this.Components.Add(new MovableComponent(this));
        this.Components.Add(new PhysicsComponent(this));
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

    public void SetDeltaTime(double dt)
    {
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).DeltaT = dt;
    }
}