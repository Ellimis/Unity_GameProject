using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    public interface ICollisionHandler
    {
        void HandleCollision(GameObject obj, Collision c);
    }

    public class FireCollisionForwardScript : MonoBehaviour
    {
        public ICollisionHandler CollisionHandler;

        public void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag != "Meteor" && collision.transform.tag != "Boss" && collision.transform.tag != "Weapon" && collision.transform.tag != "ProjectileCollider")
            {
                CollisionHandler.HandleCollision(gameObject, collision);
            }
        }
    }
}
