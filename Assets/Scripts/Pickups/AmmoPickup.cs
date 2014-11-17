using UnityEngine;
using System.Collections;

public class AmmoPickup : Pickup {

	public int amount = 45;

	void Update() {
		rorate ();
	}

	public override void action (Collider col)
	{
		RefToPlayer playerRef = col.gameObject.GetComponent<RefToPlayer> ();
		if (playerRef != null)
		{
			M4A1Controller wb = playerRef.player.GetComponentInChildren<M4A1Controller> ();
			if (wb != null)
			{
				Amunition ammo = wb.getAmmo ();
				ammo.reserve += amount;
				wb.setAmmo(ammo);
			}
		}
	}
}
