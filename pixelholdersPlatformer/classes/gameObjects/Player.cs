using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;

namespace pixelholdersPlatformer.gameObjects;

public class Player : GameObject
{

    private float startPosX;
    private float startPosY;
    public Player(float coordX, float coordY, float width, float height) : base(coordX, coordY, width, height)
    {
        this.Components.Add(new MovableComponent(this));
        this.Components.Add(new PhysicsComponent(this));
        this.Components.Add(new CollisionComponent(this));
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).HasGravity = true;
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).CanMove = true;
        startPosX = coordX;
        startPosY = coordY;

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
        CoordX = startPosX;
        CoordY = startPosY;
    }

    public void SetDeltaTime(double dt)
    {
        ((PhysicsComponent)Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).DeltaT = dt;
    }
}