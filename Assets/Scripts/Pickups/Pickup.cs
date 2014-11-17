using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public float rorateDelta = 30.0f;

	protected void rorate() {
		transform.rotation = Quaternion.Euler(transform.rotation.x, rorateDelta * Time.timeSinceLevelLoad, transform.rotation.z);
	}

	public virtual void action(Collider col) {
		return;
	}

	public virtual void collect()
	{
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag=="Player") {
			action(col);
			collect();
		}
	}
}
