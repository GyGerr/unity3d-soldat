using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemySpawner : MonoBehaviour {

	private GameObject[] spawnPoints;
	public int maxSpawnCount = 25;
	private CoreGameLoop cgl;
	private int[] enemiesPerLevel;
	public GameObject enemyPrefab;
	public int enemiesCounterKilled = 0;
	public int enemiesCountSpawned = 0; // since level started
	private GameObject remainIraqi;

	void Awake () {
		remainIraqi = new GameObject();
		remainIraqi.AddComponent<GUIText>();
		remainIraqi.transform.position = new Vector3(0.05f, 0.4f, 0.0f);
		remainIraqi.guiText.text = "";

		cgl = GetComponent<CoreGameLoop>();
		spawnPoints = GameObject.FindGameObjectsWithTag ("EnemySpawnPoint");
		enemiesPerLevel = new int[] { 0, 1, 2, 3, 6, 10, 15, 21, 28, 36, 45 };
	}
	
	void FixedUpdate () {
		remainIraqi.guiText.text = "Remain: " + (enemiesPerLevel[cgl.level] - enemiesCounterKilled).ToString();
		if (enemiesCountSpawned == enemiesPerLevel[cgl.level] && enemiesCounterKilled >= enemiesCountSpawned) {
			enemiesCountSpawned = 0;
			enemiesCounterKilled = 0;
			if(!cgl.nextLevel()) {
				return;
			}
		}
		if (enemiesCountSpawned < enemiesPerLevel[cgl.level] && (enemiesCountSpawned - enemiesCounterKilled) < maxSpawnCount) {
			spawn();
		}
	}

	void spawn () {
		int rand = Random.Range (0, spawnPoints.Length - 1);
		GameObject go = spawnPoints[rand];
		Instantiate (enemyPrefab, go.transform.position, go.transform.rotation);
		enemiesCountSpawned++;
	}
}
