using UnityEngine;
using System.Collections;

public enum BugState {
	Stuck,
	Moving,
	Idle
}

public class BugController : MonoBehaviour {

	public float launchSpeed;
	public float moveDist;
	public float moveSpeed;
	public float size;

	private Animator animator;

	private BugState m_state;
	private float m_timeInState;
	private Vector3 m_destination;

	private GameObject m_tongue;

	private Collider m_collider;
	private Rigidbody m_rigidbody;

    protected virtual void StartBug() { }
    protected virtual void UpdateBug() { }

    public virtual bool IsShootable()
    {
        return false;
    }
    
	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
		m_collider = GetComponent<Collider> ();
		m_rigidbody = GetComponent<Rigidbody> ();

		m_state = BugState.Idle;
		m_timeInState = 0;

        StartBug();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateState ();

        UpdateBug();
	}

	private void UpdateState() {
		m_timeInState += Time.deltaTime;

		switch (m_state) {
		case BugState.Idle:
			DoIdle ();
			break;
		case BugState.Moving:
			DoMove ();
			break;
		case BugState.Stuck:
			DoStuck ();
			break;
		}
	}

	private void DoStuck() {
		Vector3 newPos = transform.position;
		newPos.x = m_tongue.transform.position.x;
		newPos.z = m_tongue.transform.position.z;
		transform.position = newPos;
	}

	private void DoMove() {
		Vector3 d = m_destination - transform.position;
		d.y = 0;
		float s = moveSpeed * Time.deltaTime;

		//reached destination
		if (d.magnitude < s * 1.05f) {
			m_timeInState = 0;
			animator.SetFloat ("Speed", 0);
			m_state = BugState.Idle;
			return;
		}

		d.Normalize ();
		transform.position += d * s;
		transform.rotation = Quaternion.LookRotation (d);
	}

	private void DoIdle() {
		float v = Random.value;
		// done idle
		if (m_timeInState > 1.0f && v < .05f) {
			m_timeInState = 0;
			m_state = BugState.Moving;
			animator.SetFloat ("Speed", 1.0f);

			// get new dest
			Vector3 destOffset = new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
			destOffset.Normalize ();
			destOffset *= moveDist + Random.Range (-.5f, .5f) * moveDist;
			m_destination = transform.position + destOffset;

		}
	}

	public void GetStuck(GameObject tongue) {
		m_collider.enabled = false;
		m_rigidbody.useGravity = false;
		m_tongue = tongue;
		m_state = BugState.Stuck;
	}

	public void Drop() {
		m_collider.enabled = true;
		m_rigidbody.useGravity = true;
		m_tongue = null;
		m_state = BugState.Idle;
		m_timeInState = 0;

		Launch ();
	}

	private void Launch() {
		Vector3 launchDir = Vector3.zero;
		launchDir.x = Random.Range (-1.0f, 1.0f);
		launchDir.y = Random.value;
		launchDir.z = Random.Range (-1.0f, 1.0f);
		
		launchDir.Normalize ();
		
		m_rigidbody.AddForce (launchDir * launchSpeed * m_rigidbody.mass);
	}

	public float GetSize() {
		return size;
	}
}
