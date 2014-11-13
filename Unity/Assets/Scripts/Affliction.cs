using UnityEngine;
using System.Collections;

public class Affliction {
	public float EndTime {get; set;}
	public AfflictionTypes AfflictionType { get; set; }
	public float AffectAmount { get; set; }
}

public enum AfflictionTypes{
	SpeedBoost,
    Stun
}