using UnityEngine;
using System.Collections;

// MuzzleFlash
public class SparkScript : MonoBehaviour {

	void Update () {
		transform.localScale = Vector3.one * Random.Range(0.01f,0.04f);
		//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.x, Random.Range(0,90.0f));
	}
}
