using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public bool destroyOnSpawn; //If it spawn just one time

	public bool hasSpawned = false;

	void OnEnable() {
		//To make sure that it will spawn again after a player's respawn
		if (hasSpawned) {
			if (gameObject.transform.childCount > 0) {
				Destroy (gameObject.transform.GetChild (0).gameObject);
			}
			hasSpawned = false;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!hasSpawned && Camera.main.ViewportToWorldPoint (new Vector3 (1.5f, 0, 0)).x >= transform.position.x &&
			Camera.main.ViewportToWorldPoint (new Vector3 (0.99f, 0, 0)).x <= transform.position.x) {
			Instantiate (enemyPrefab, transform.position, Quaternion.identity, transform);
			hasSpawned = true;
			if (destroyOnSpawn) {
				gameObject.transform.DetachChildren ();
				Destroy (gameObject);
			}
		}
		
	}
}
