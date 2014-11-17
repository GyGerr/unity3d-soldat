using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    private RaycastHit hit;

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        Physics.Raycast( ray, out hit );
        transform.position = new Vector3(hit.point.x, 0, hit.point.z);
	}
}
