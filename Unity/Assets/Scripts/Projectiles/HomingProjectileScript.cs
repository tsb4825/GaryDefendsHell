using UnityEngine;
using System.Collections;

public class HomingProjectileScript : ProjectileScript
{
    public Transform Target;
    public float MaxAngleTurn;
    public float Force;

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            var angle = Vector3.Lerp(transform.forward, Target.position, Time.deltaTime * MaxAngleTurn);
            Debug.Log(angle);
            transform.eulerAngles = angle;
            transform.GetComponent<Rigidbody2D>().AddForce((Target.position - transform.position).normalized * Force);
        }
        else
        {
            Target = TowerFiredFrom.GetComponent<HomingTowerScript>().Target;
        }
    }
}
