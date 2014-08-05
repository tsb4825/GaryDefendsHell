using UnityEngine;
using System.Collections;
using System.Linq;

public class SpeedBoostScript : MonoBehaviour
{
    public float SkillCooldown;
    public float NextSkillActivateTime;
    public float SpeedBoostRange;
    public float SpeedBoostAmount;
    public float Duration;

    void Update()
    {
        if (Time.time >= NextSkillActivateTime && CanBoostUnit())
        {
            SpeedBoostUnits();
            NextSkillActivateTime = Time.time + SkillCooldown;
        }
    }

    private bool CanBoostUnit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SpeedBoostRange);

        return colliders.Any(x => x.tag == transform.tag && x.transform != transform && x is BoxCollider2D);
    }

    private void SpeedBoostUnits()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SpeedBoostRange);

        colliders.Where(x => x.tag == transform.tag && x is BoxCollider2D).ToList().ForEach(x =>
        {
            UtilityFunctions.DebugMessage("SpeedBoost - " + x.name);
            x.GetComponent<CreepScript>().AddAffliction(AfflictionTypes.SpeedBoost, Duration, SpeedBoostAmount);

        });
    }
}
