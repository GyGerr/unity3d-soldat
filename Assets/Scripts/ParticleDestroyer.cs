using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {

    public float timeToDestroy = 5.0f;

    private float instantiateTime;

    void Awake()
    {
        instantiateTime = Time.time;
    }

	void FixedUpdate () {
        if (instantiateTime - timeToDestroy >= 0)
        {
            //this.particleSystem.Stop();
            Destroy(gameObject);
        }
	}
}
