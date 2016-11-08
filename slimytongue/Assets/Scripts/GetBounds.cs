using UnityEngine;
using System.Collections;

public class GetBounds : MonoBehaviour {

	public static GetBounds s_GetBounds;

	void Awake() {
		s_GetBounds = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Bounds getBounds(){
		return GetComponent<MeshFilter>().mesh.bounds;
	}
}
