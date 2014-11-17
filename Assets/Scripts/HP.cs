using UnityEngine;
using System.Collections;

public class HP : MonoBehaviour {

    public float hp = 100; // health
    public float ap = 100; // armor

	public float hpModifier = 0.5f;
	public float apModifier = 0.7f;

	public GameObject ammoPickupPrefab;
	public GameObject hpPickupPrefab;


	void OnCollisionEnter(Collision col)
    {
        if (enabled && col.collider.tag == "Bullet")
        {
            BulletBehaviour projectile = col.collider.GetComponent<BulletBehaviour>();
            if (projectile != null)
            {
				Debug.Log("HP distance:" + projectile.getDistance() + "| HP hit dmg: " + projectile.getDamage());
				takeHit(projectile.getDamage());
				Destroy(col.collider.gameObject);
            }
        }
    }

	public void takeHit(float dmg)
	{
		if (ap <= 0) {
			apModifier = 0;
			hpModifier = 1;
		}

		hp -= dmg * hpModifier;
		ap -= dmg * apModifier;

		if (hp <= 0) {
			dead();
		}
	}

	void dead()
	{
		GameObject coreGame = GameObject.FindGameObjectWithTag("GameController");
		CoreGameLoop cgl = coreGame.GetComponent<CoreGameLoop>();

		// Player
		if (gameObject.tag == "Player") {
			cgl.gameOver = 1;
			Destroy(gameObject);
			return;
		}

		//Enemy
		spawnPickup ();
		EnemySpawner enemySpawner = coreGame.GetComponent<EnemySpawner> ();
		enemySpawner.enemiesCounterKilled++;

		// this.gameObject.SetActive(false);
		Destroy(gameObject);
	}

	void spawnPickup() {
		if (ammoPickupPrefab != null && hpPickupPrefab != null) {
			int rand = Random.Range(0, 3);
			
			if (rand==1) {
				GameObject go = Instantiate(ammoPickupPrefab, this.transform.position, this.transform.rotation) as GameObject;
				int amount = Random.Range(30, 90);
				AmmoPickup ammo = go.GetComponent<AmmoPickup>();
				if (ammo != null) {
					ammo.amount = amount;
				}
			} else if (rand==2) {
				GameObject go = Instantiate(hpPickupPrefab, this.transform.position, this.transform.rotation) as GameObject;
				int amount = Random.Range(25, 100);
				HeathPickup hpPickup = go.GetComponent<HeathPickup>();
				ArmorPickup apPickup = go.GetComponent<ArmorPickup>();
				if (hpPickup != null && apPickup != null) {
					hpPickup.amount = amount;
					apPickup.amount = amount;
				}
			} else if (rand == 3) {
				int amount = 0;
				GameObject goAmmo = Instantiate(ammoPickupPrefab, this.transform.position, this.transform.rotation) as GameObject;
				GameObject goHp = Instantiate(hpPickupPrefab, this.transform.position, this.transform.rotation) as GameObject;
				amount = Random.Range(30, 90);
				AmmoPickup ammo = goAmmo.GetComponent<AmmoPickup>();
				if (ammo != null) {
					ammo.amount = amount;
				}
				amount = Random.Range(25, 100);
				HeathPickup hpPickup = goHp.GetComponent<HeathPickup>();
				ArmorPickup apPickup = goHp.GetComponent<ArmorPickup>();
				if (hpPickup != null && apPickup != null) {
					hpPickup.amount = amount;
					apPickup.amount = amount;
				}
			}
		}
	}


    //public Transform explosionPrefab;
    //void OnCollisionEnter(Collision collision) {
    //    ContactPoint contact = collision.contacts[0];
    //    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //    Vector3 pos = contact.point;
    //    Instantiate(explosionPrefab, pos, rot) as Transform;
    //    Destroy(gameObject);
    //}
}
