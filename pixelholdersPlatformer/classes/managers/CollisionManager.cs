using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.gameObjects;
using System.Numerics;

namespace pixelholdersPlatformer.classes.managers;

public class CollisionManager
{
    private List<GameObject> _gameObjects;

    public void SetGameObjects(List<GameObject> gameObjects)
    {
        this._gameObjects = gameObjects;
    }

    public CollisionManager()
    {
        
    }


    private float overlapX;
    private float overlapY;
    public void HandleCollision()
    { 
        foreach(GameObject gameObject in _gameObjects) 
        {
            
            if (gameObject.GetComponent(gameObjects.Component.Collision) != null)
            {
                foreach (GameObject other in _gameObjects)
                { 
                    if(!other.Equals(gameObject) && other.GetComponent(gameObjects.Component.Collision) != null)
                    {
                        if (isColliding(gameObject, other))
                        {
                            if (gameObject is Player && other is Cannonball)
                            {
                                ((Player)gameObject).HurtPlayer();
                                overlapX = 0;
                                overlapY = 0;
                                continue;
                            }
                            ((CollisionComponent)gameObject.GetComponent(gameObjects.Component.Collision)).Collide(overlapX, overlapY);
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
        if (gameObject is Cannonball)
        {
            return false;
        }
        if (gameObject is Enemy && other is Cannonball)
        {
            return false;
        }

        if (((CollisionComponent)gameObject.GetComponent(gameObjects.Component.Collision)).IsCollidable && ((CollisionComponent)other.GetComponent(gameObjects.Component.Collision)).IsCollidable)
        {
            Vector2 velocity = ((PhysicsComponent)gameObject.GetComponent(gameObjects.Component.Physics)).Velocity;

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

                if (gameObject is SpecialTile && other is Player)
                {
                    ((SpecialTile)gameObject).SpecialCollision();
                }
                else if (other is SpecialTile && gameObject is Player)
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