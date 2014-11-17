using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public float lifeTime = 10.0f;
    public float dist = 100.0f;

    private float spawnTime = 0.0f;
	private WeaponBehaviour gun;
    private Vector3 originPoint;
    private string originTag;

    void OnEnable()
    { 
	    spawnTime = Time.time;
        originPoint = transform.position;
	}
	
	void Update () {
        if (Time.time > spawnTime + lifeTime || Vector3.Distance(transform.position, originPoint) >= dist)
        { 
            Destroy (gameObject);
        }
	}

	public void Init(WeaponBehaviour carbineRef, string originTagVal = "Player")
    {
        gun = carbineRef;
        originTag = originTagVal;
    }

    public float getDistance()
    {
        return Vector3.Distance(transform.position, originPoint);
    }

    public float getDamage(WeaponProperties.BodyPoints bodyPoint = WeaponProperties.BodyPoints.Body)
    {
        return gun.getDamage(getDistance(), bodyPoint);
    }

	// void OnCollisionEnter(Collision col) {
	// 	if (col.collider.gameObject.isStatic)
	// 	{
	// 		Destroy(gameObject);
	// 		return;
	// 	}
	// 	else if(col.collider.tag == "Enemy" || col.collider.tag == "Player") 
	// 	{
	// 		if(col.collider.gameObject!=null) {
	// 			HP hp = col.collider.gameObject.GetComponent<HP>();
	// 			if (hp != null) {
	// 				hp.takeHit(getDamage());
	// 			}
	// 		}
	// 		Destroy(gameObject); 
	// 		return;
	// 	}
	// }

    void OnTriggerEnter(Collider col)
    {
        // Dispose object
		if (col.gameObject.isStatic)
		{
			Destroy(gameObject);
			return;
		}
		else if((col.tag == "Enemy" || col.tag == "Player") && originTag != col.tag) 
		{
			if(col.gameObject!=null) {
				HP hp = col.gameObject.GetComponent<HP>();
				if (hp != null) {
					hp.takeHit(getDamage());
				}

				if (col.tag == "Enemy") {
					EnemySight enemySight = col.gameObject.GetComponentInChildren<EnemySight>();
					if (enemySight != null) {
						enemySight.personalLastSighting = originPoint;
					}
				}

			}
			Destroy(gameObject); 
			return;
		}
    }
}
