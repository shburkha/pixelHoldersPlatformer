using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;

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
                            ((CollisionComponent)gameObject.Components.Where(t => t.GetType().Name == "CollisionComponent").First()).Collide();
                        }
                    }
                }
            }
        }
    }


    private bool isColliding(GameObject gameObject, GameObject other)
    {

        if (((CollisionComponent)gameObject.Components.Where(t => t.GetType().Name == "CollisionComponent").First()).IsCollidable && ((CollisionComponent)other.Components.Where(t => t.GetType().Name == "CollisionComponent").First()).IsCollidable)
        {

            if (gameObject.CoordX + gameObject.Width >= other.CoordX &&
            gameObject.CoordX <= other.CoordX + other.Width &&
            gameObject.CoordY + gameObject.Height >= other.CoordY &&
            gameObject.CoordY <= other.CoordY + other.Height)
            {

                return true;
            }
        }

        return false;
    }

}