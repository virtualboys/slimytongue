using UnityEngine;
using System.Collections;

public class BugSpawner : MonoBehaviour {
	public Vector3 bounds;
    public float spawnChance;
	public GameObject[] bugs;



	// Use this for initialization
	void Start () {
		bounds = new Vector3 (10, 0, 3.9f);
		//bounds=GetBounds.s_GetBounds.getBounds ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.value < spawnChance) {
			Vector3 newBugPos=Vector3.zero;
			newBugPos = new Vector3 (Random.Range (-bounds.x, bounds.x), 2.0f, Random.Range (-bounds.z, bounds.z));
			GameObject.Instantiate (bugs [Random.Range (0, bugs.Length)], newBugPos, Quaternion.identity);
			
		}
	}
}
