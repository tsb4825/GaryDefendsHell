using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConstantAttackProjectileScript : ProjectileScript
{

    public float NextFireTime;
    public float AttackCooldown;
    public List<Transform> Targets;
    public Vector3 TargetLocation;
    public bool IsMoving = true;

    void Update()
    {
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetLocation, Time.deltaTime * ProjectileSpeed);
            if (transform.position == TargetLocation)
                IsMoving = false;
        }
        if (Time.time >= NextFireTime)
        {
            Targets.RemoveAll(x => x == null);
            Fire();
            NextFireTime = Time.time + AttackCooldown;
        }
    }

    public void Fire()
    {
        foreach (Transform target in Targets)
        {
            target.GetComponent<CreepScript>().TakeDamage(Damage);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider is BoxCollider2D)
        {
            Targets.Add(collider.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider is BoxCollider2D)
        {
            Targets.Remove(collider.transform);
        }
    }
}
