using UnityEngine;
using System.Collections;

public class CameraMovementStatic : MonoBehaviour {

    public GameObject player;
	public GameObject cursor;
	public float damping = 5.0f;
	public float viewDistance = 10.0f;

	private Vector3 offset;

	void Start () {
        offset = transform.position;
	}
	
    // LateUpdate - good for procedural animations
	void Update () {
		if(player) {
			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewDistance);
			Vector3 cursorPos = Camera.main.ScreenToWorldPoint(mousePos);
			Vector3 playerPos = player.transform.position;

			Vector3 center = new Vector3((playerPos.x + cursorPos.x) / 2, playerPos.y, (playerPos.z + cursorPos.z) / 2);

			transform.position = Vector3.Lerp(transform.position, center + new Vector3(0.0f, offset.y, offset.z), Time.deltaTime * damping);        
		}
	}
}
