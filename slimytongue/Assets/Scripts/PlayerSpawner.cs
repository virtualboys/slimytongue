using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;
	public Transform[] spawnPositions;

	public string[] horizontalInputs;
	public string[] verticalInputs;
	public string[] shootInputs;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 2; i++) {
			GameObject player = GameObject.Instantiate (playerPrefab, spawnPositions [i].position, Quaternion.identity) as GameObject;
			PlayerInput pi = player.GetComponent<PlayerInput> ();
			pi.horizontal = horizontalInputs [i];
			pi.vertical = verticalInputs [i];
			pi.shootTongue = shootInputs [i];
		}
	}
}
