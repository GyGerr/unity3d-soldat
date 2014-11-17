using UnityEngine;
using System.Collections;

public class MeleeController : WeaponBehaviour {

	public Collider col;

	void OnEnable() {
	}

	public void Attack() {
		if (!isFiring) {
			isFiring = true;
			col.enabled = true;
			StartCoroutine(AttackTime());
			StartCoroutine(AttackCooldown());
		}
	}

	public bool IsFiring() {
		return isFiring;
	}

	IEnumerator AttackTime()
	{
		yield return new WaitForSeconds(1);
		col.enabled = false;
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(3);
		isFiring = false;
	}

	void OnTriggerStay(Collider col)
	{
		if(col.tag == "Enemy" && col.enabled == true) 
		{
			if(col.gameObject!=null) {
				HP hp = col.gameObject.GetComponent<HP>();
				if (hp != null) {
					hp.takeHit(100);
				}
				
				EnemySight enemySight = col.gameObject.GetComponentInChildren<EnemySight>();
				if (enemySight != null) {
					enemySight.personalLastSighting = transform.position;
				}
			}
		}
	}
}
