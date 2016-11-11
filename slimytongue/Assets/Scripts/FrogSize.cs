using UnityEngine;
using System.Collections;

public class FrogSize : MonoBehaviour {

	public float sizeInc;
	public float score;

	private MovePlayer m_movePlayer;

	// Use this for initialization
	void Start () {
		m_movePlayer = GetComponent<MovePlayer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GrowSize(float bugSize) {
		float m = 1.0f + bugSize * sizeInc;
		transform.localScale *=  m;
		score += bugSize;
		m_movePlayer.SetSpeedMult (1.0f / m);
	}
}
		