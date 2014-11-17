using UnityEngine;
using System.Collections;

public class M4A1Controller : WeaponBehaviour {

    private M4A1WeaponProperties props;
    private CoreGameLoop cgl;
	public GameObject meleeGo;
	private MeleeController melee;

    // Use this for initialization
    void OnEnable()
    {
        cgl = GameObject.FindGameObjectWithTag("GameController").GetComponent<CoreGameLoop>();
        ammo = new Amunition(450, 30);
        props = new M4A1WeaponProperties();
        rof = this.getRof();
        gunSmoke.Stop();
		melee = meleeGo.GetComponent<MeleeController> ();
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetButton("Fire1"))
        {
            if (!isFiring && !isReloading)
            {
                if (Time.time > lastFireTime + 1 / props.getRof())
                {
                    if (cgl.cheatActivated)
                        Fire();
                    else
                        Shoot();
                }
            }
        }
        if(Input.GetButton("Melee"))
        {
            melee.Attack();
        }
        if (Input.GetButton("Reload"))
        {
            if (!isReloading)
            {
                if (Time.time > lastFireTime + 1 / props.getRof())
                {
                	Reload();
                }
            }
        }
	}

    public float getRof()
    {
        return props.getRof() / 3600;
    }

    public override float getDamage(float distance, WeaponProperties.BodyPoints bodyPoint) {
        return props.getDamage(distance, bodyPoint);
    }

    public override void Fire()
    {
        base.Fire();

        Rigidbody projectileInstance = Instantiate(
            projectilePrefab,
            spawnPoint.position,
			spawnPoint.rotation
        ) as Rigidbody;

        gunSmoke.Play();

        BulletBehaviour projectileInstanceScript = projectileInstance.GetComponent<BulletBehaviour>();
        if (projectileInstanceScript != null)
        {
            projectileInstanceScript.dist = props.getRange();
            projectileInstanceScript.Init(this);
        }

        if (projectileInstance != null)
        {
            projectileInstance.velocity = transform.TransformDirection(Vector3.up * props.getRof() / 20);
            //projectileInstance.velocity = transform.up * props.getRof();
        }

        OnStopFire();
    }

}
