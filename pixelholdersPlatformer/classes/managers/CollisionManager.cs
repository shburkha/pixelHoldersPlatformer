using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.gameObjects;
using System.Numerics;

namespace pixelholdersPlatformer.classes.managers;

public class CollisionManager
{
    private List<GameObject> gameObjects;

    public void SetGameObjects(List<GameObject> gameObjects)
    {
        this.gameObjects = gameObjects;
    }

    public CollisionManager()
    {
        
    }


    private float overlapX;
    private float overlapY;
    public void HandleCollision()
    { 
        foreach(GameObject gameObject in gameObjects) 
        {
            
            if (gameObject.Components.Where(t => t.GetType().Name == "CollisionComponent").Count() != 0)
            {
                foreach (GameObject other in gameObjects)
                { 
                    if(!other.Equals(gameObject) && other.Components.Where(t => t.GetType().Name == "CollisionComponent").Count() != 0)
                    {
                        if (isColliding(gameObject, other))
                        {
                              ((CollisionComponent)gameObject.Components.Where(t => t.GetType().Name == "CollisionComponent").First()).Collide(overlapX, overlapY);

                        }
                        overlapX = 0;
                        overlapY = 0;
                    }
                }
            }
        }
    }

    
    private bool isColliding(GameObject gameObject, GameObject other)
    {

        if (((CollisionComponent)gameObject.Components.Where(t => t.GetType().Name == "CollisionComponent").First()).IsCollidable && ((CollisionComponent)other.Components.Where(t => t.GetType().Name == "CollisionComponent").First()).IsCollidable)
        {
            Vector2 velocity = ((PhysicsComponent)gameObject.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).Velocity;

            if (gameObject.CoordX + gameObject.Width + velocity.X > other.CoordX &&
                gameObject.CoordX + velocity.X <= other.CoordX + other.Width &&
                gameObject.CoordY + gameObject.Height + velocity.Y > other.CoordY &&
                gameObject.CoordY < other.CoordY + other.Height)
            {

                float player_bottom = gameObject.CoordY + gameObject.Height;
                float tiles_bottom = other.CoordY + other.Height;
                float player_right = gameObject.CoordX + gameObject.Width;
                float tiles_right = other.CoordX + other.Width;

                float top_collision = tiles_bottom - gameObject.CoordY;
                float bottom_collision = player_bottom - other.CoordY;
                float right_collision = player_right - other.CoordX;
                float left_collision = tiles_right - gameObject.CoordX;

                float[] collisions = [top_collision, bottom_collision, right_collision, left_collision];

                if (collisions.Min() == bottom_collision)
                {
                    overlapY = gameObject.CoordY + gameObject.Height - other.CoordY;
                }
                if (collisions.Min() == top_collision)
                {
                    overlapY = -1 * (other.CoordY + other.Height - gameObject.CoordY);
                }
                if (collisions.Min() == right_collision)
                {
                    overlapX = gameObject.CoordX + gameObject.Width - other.CoordX;
                }
                if (collisions.Min() == left_collision)
                {
                    overlapX = -1 * (other.CoordX + other.Width - gameObject.CoordX);
                }

                if (gameObject.GetType().Name == "SpecialTile" && other.GetType().Name == "Player")
                {
                    ((SpecialTile)gameObject).SpecialCollision();
                }
                else if (other.GetType().Name == "SpecialTile" && gameObject.GetType().Name == "Player")
                {
                    ((SpecialTile)other).SpecialCollision();
                }

                //overlapX = other.CoordX + other.Width - gameObject.CoordX;
                //overlapY = gameObject.CoordY + gameObject.Height - other.CoordY;

                return true;
            }
        }

        return false;
    }

}