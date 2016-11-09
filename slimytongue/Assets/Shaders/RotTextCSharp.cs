using UnityEngine;
using System.Collections;

public class RotTextCSharp : MonoBehaviour {

    public Vector2 offset;
    public float speed = .5f;

    private Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        offset.x = (offset.x - speed * Time.deltaTime) % 1;
        offset.y = (offset.y + speed * Time.deltaTime) % 1;
        mat.mainTextureOffset = offset;
    }
}
