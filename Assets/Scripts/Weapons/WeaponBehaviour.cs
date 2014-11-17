using UnityEngine;
using System.Collections;

public class WeaponBehaviour : MonoBehaviour {

    protected Amunition ammo;
    protected float rof = 0.0f;
    public ParticleSystem gunSmoke;
    //public ParticleSystem gunSparks;
    protected float reloadingTime = 1.0f;

	public Rigidbody projectilePrefab;
	public GameObject soldierParent;
	public Transform spawnPoint;

    protected bool isReloading = false;
    protected bool isFiring = false;
	public GameObject muzzleFlash;
	protected float lastFireTime;

    void OnEnable() {
        gunSmoke.Stop();
		lastFireTime = 0;
    }

    #region Reloading

    protected void OnStartReload()
    {
        isReloading = true;
        //Debug.Log("Reloading...");
    }

    public bool Reload()
    {
        if (ammo.reserve <= 0)
        {
            return false;
        }

        int sumBullets = ammo.currentMag + ammo.reserve;
        if (sumBullets > ammo.magSize)
        {
            ammo.currentMag = ammo.magSize;
            ammo.reserve = sumBullets - ammo.magSize;
        }
        else
        {
            ammo.currentMag = sumBullets;
            ammo.reserve = 0;
        }
        
        return true;
    }

	public Amunition getAmmo() {
		return ammo;
	}

	public void setAmmo(Amunition munition) {
		ammo = munition;
	}

    protected void OnStopReload()
    {
        isReloading = false;
        //Debug.Log("Reloaded.");
    }

    #endregion

    #region Firing

    public virtual bool Shoot(int firedBulletCount = 1)
    {
        if (ammo.currentMag <= 0)
        {
            //Debug.Log("Clip is empty!");
            return false;
        }

        // positive value of difference between magazine and bullets which are fired
        // for example we have 2 bullets in clip and we would like to fire 3 bullets we gonna have:
        // 2 - 3 = -1
        // we don't have 1 bullet according to burst "request", thats not fine
        // so 3 - 1 = 2; 
        if (ammo.currentMag - firedBulletCount <= 0)
        {
            for (int i = 0; i < ammo.currentMag; i++)
			{
			    Fire();                
			}
        }
        else
        {
            for (int i = 0; i < firedBulletCount; i++)
			{
			    Fire();
			}
        }

        return true;
    }

    protected void OnStartFire()
    {
        isFiring = true;
		muzzleFlash.renderer.enabled = true;
		ammo.currentMag--;
		lastFireTime = Time.time;
    }

    public virtual void Fire() {  
		OnStartFire();
    }

    protected void OnStopFire()
    {
        StartCoroutine(StoppedFiring());
    }

	public virtual float getDamage(float distance, WeaponProperties.BodyPoints bodyPoint) {
		return 0;
	}

    IEnumerator StoppedFiring()
    {
        yield return new WaitForSeconds(rof * 5);
		muzzleFlash.renderer.enabled = false;
		gunSmoke.Stop ();
        isFiring = false;
    }

    #endregion
}
