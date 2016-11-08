using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TongueTrigger : MonoBehaviour {

	public TongueController tongueController;

	private Collider m_collider;
	private List<BugController> m_bugs;

	// Use this for initialization
	void Awake(){
		m_bugs = new List<BugController> ();
		m_collider = GetComponent<Collider> ();
	}

	void Start () {
		
	}
	

	void Update(){
		//foreach (BugController bc in m_bugs) {
		//	bc.gameObject.transform.position = transform.position;
		//}

		if (Input.GetButtonDown ("Jump2")) {
			tongueController.Strike ();
		}
	}

	public List<BugController> GetBugs() {
		return m_bugs;
	}

	public void EnableTrigger () {
		m_collider.enabled = true;
	}

	public void DisableTrigger() {
		m_collider.enabled = false;
	}

	public void EatBugs() {
		for (int i = 0; i < m_bugs.Count; i++) {
			Destroy (m_bugs [i].gameObject);
		}

		m_bugs.Clear ();
	}

	public void DropBugs() {
		for (int i = 0; i < m_bugs.Count; i++) {
			m_bugs [i].Drop ();
		}

		m_bugs.Clear ();
	}

	void OnCollisionEnter(Collision collision){
		Debug.Log ("Collision");
		GameObject obj = collision.gameObject;
		if (obj.tag == "bug") {
			BugController bug = obj.GetComponent<BugController> ();
			bug.GetStuck (gameObject);
			m_bugs.Add (bug);
		} else if (obj.tag == "player") {
			Debug.Log ("hit");
			TongueController player = obj.GetComponent<TongueController> ();
			player.Strike ();
		}
	}
}
