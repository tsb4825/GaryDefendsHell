using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
    public float Damage;
    public float ProjectileSpeed;
    public float AliveTime;
    public bool DebugMode;
    public Transform TowerFiredFrom;

    void Awake()
    {
        Destroy(gameObject, AliveTime);
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy" && collider is BoxCollider2D)
        {
            UtilityFunctions.DebugMessage("Projectile Hit");
            collider.GetComponent<CreepScript>().TakeDamage(Damage);
            Destroy(this.gameObject);
        }
    }
}
