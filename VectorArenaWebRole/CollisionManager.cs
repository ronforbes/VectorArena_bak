using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VectorArenaWebRole
{
    public class CollisionManager
    {
        List<GameObject> gameObjects;

        public CollisionManager()
        {
            gameObjects = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public void Update()
        {
            Parallel.ForEach(gameObjects, gameObject =>
            {
                List<GameObject> otherObjects = gameObjects;
                foreach (GameObject otherObject in gameObjects)
                {
                    if (gameObject == otherObject)
                    {
                        continue;
                    }

                    if (gameObject.IsCollidingWith(otherObject))
                    {
                        gameObject.CollideWith(otherObject);
                        otherObject.CollideWith(gameObject);
                    }
                }
            });
        }
    }
}