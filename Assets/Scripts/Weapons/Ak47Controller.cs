using UnityEngine;
using System.Collections;

public class Ak47Controller : WeaponBehaviour {

	private AK47WeaponProperties props;

	void Awake()
	{
		ammo = new Amunition(120, 120);
		props = new AK47WeaponProperties();
		rof = getRof();
	}

	public float getRof()
	{
		return props.getRof() / 3600;
	}

	public override float getDamage(float distance, WeaponProperties.BodyPoints bodyPoint) {
		return props.getDamage(distance, bodyPoint);
	}
	
	public override bool Shoot(int firedBulletCount = 1)
	{
		Fire();
		return true;
	}

	public override void Fire()
	{
        OnStartFire();
		gunSmoke.Play();
		
		Rigidbody projectileInstance = Instantiate(
			projectilePrefab,
			spawnPoint.position,
			spawnPoint.rotation
			) as Rigidbody;
		
		

		if (projectileInstance != null)
		{
			projectileInstance.velocity = transform.TransformDirection(Vector3.forward * props.getRof() / 20);
			//projectileInstance.velocity = transform.up * props.getRof();

			BulletBehaviour projectileInstanceScript = projectileInstance.GetComponent<BulletBehaviour>();
			if (projectileInstanceScript != null)
			{
				projectileInstanceScript.dist = props.getRange();
				projectileInstanceScript.Init(this, "Enemy");
			}
		}

		OnStopFire();
	}
}
