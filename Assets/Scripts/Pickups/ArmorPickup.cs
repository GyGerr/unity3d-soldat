using UnityEngine;
using System.Collections;

public class ArmorPickup : Pickup {

	public float amount = 25.0f;
	
	void Update () {
		rorate ();
	}
	
	public override void action(Collider col) {
		RefToPlayer playerRef = col.gameObject.GetComponent<RefToPlayer> ();
		if (playerRef != null)
		{
			HP playerHp = playerRef.player.GetComponent<HP> ();
			if (playerHp != null) {
				playerHp.ap += amount;
				if (playerHp.ap > 0) {
					playerHp.hpModifier = 0.5f;
					playerHp.apModifier = 0.7f;					
				}
			}
		}		
	}
}
