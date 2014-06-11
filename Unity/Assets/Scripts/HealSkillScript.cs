using UnityEngine;
using System.Collections;
using System.Linq;

public class HealSkillScript : MonoBehaviour {
	public float SkillCooldown;
	public float NextSkillActivateTime;
	public float HealRange;
	public float HealAmount;

	void Update()
	{
		if (Time.time >= NextSkillActivateTime && CanHealUnit ()) {
			HealUnits();
			NextSkillActivateTime = Time.time + SkillCooldown;
				}
	}

	private bool CanHealUnit()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, HealRange);

		return colliders.Any (x => x.tag == transform.tag && x.transform != transform && x is BoxCollider2D);
	}

	private void HealUnits()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, HealRange);

		colliders.Where (x => x.tag == transform.tag && x.transform != transform && x is BoxCollider2D).ToList ().ForEach (x => {
			UtilityFunctions.DebugMessage ("Healing - " + x.name);                                                             
			x.GetComponent <CreepScript>().Heal (HealAmount);

		});
	}
}
