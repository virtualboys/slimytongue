using UnityEngine;
using System.Collections;

public class BugController : MonoBehaviour {

	public float launchSpeed;

	private bool m_isStuck;
	private GameObject m_tongue;

	private Collider m_collider;
	private Rigidbody m_rigidbody;

	// Use this for initialization
	void Start () {
		m_collider = GetComponent<Collider> ();
		m_rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_tongue != null) {
			Vector3 newPos = transform.position;
			newPos.x = m_tongue.transform.position.x;
			newPos.z = m_tongue.transform.position.z;
			transform.position = newPos;
		}
	}

	public void GetStuck(GameObject tongue) {
		m_collider.enabled = false;
		m_rigidbody.useGravity = false;
		m_tongue = tongue;
	}

	public void Drop() {
		m_collider.enabled = true;
		m_rigidbody.useGravity = true;
		m_tongue = null;

		Vector3 launchDir = Vector3.zero;
		launchDir.x = Random.Range (-1.0f, 1.0f);
		launchDir.y = Random.value;
		launchDir.z = Random.Range (-1.0f, 1.0f);

		launchDir.Normalize ();

		Debug.Log ("drop dir " + launchDir);

		m_rigidbody.AddForce (launchDir * launchSpeed * m_rigidbody.mass);

	}
}
