using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TongueController : MonoBehaviour {

	public float turnSpeed;

	public float baseTongueSpeed;
	public float fastTongueSpeed;

	public float retractSpeed;

	public float maxLength;
	public float distBetweenPoints;

	public GameObject tongueTip;
	public GameObject tonguePointPrefab;

	public PlayerInput playerInput;

	private TongueTrigger m_tongueTrigger;

	private bool m_isShooting;
	private bool m_isRetracting;
	private bool m_isAiming;

	private Quaternion m_tongueDir;
	private float m_tongueLength;

	private List<Transform> m_tonguePoints;

	void Awake() {
		m_tonguePoints = new List<Transform> ();
	}

	void Start () {
		m_tongueTrigger = tongueTip.GetComponent<TongueTrigger> ();
	}

	void Update () {
		if(!IsTongueOut() && Input.GetButtonDown(playerInput.shootTongue)) {
			m_isAiming = true;

		} else if (m_isAiming && Input.GetButtonUp (playerInput.shootTongue)) {
			ShootTongue ();

		} else if (m_isShooting) {
			
			if(Input.GetButton(playerInput.shootTongue)) {
				MoveTongueForward (fastTongueSpeed);
			} else {
				UpdateTongueDir ();
				MoveTongueForward (baseTongueSpeed);
			}
		} else if (m_isRetracting) {
			MoveTongueBackward ();
		}
	}

	private void ShootTongue() {
		m_isAiming = false;
		m_isShooting = true;
		m_tongueLength = 0;
		m_tongueDir = transform.rotation;

		tongueTip.transform.position = transform.position;
		tongueTip.SetActive (true);
		m_tongueTrigger.EnableTrigger ();
	}

	private void RetractTongue() {
		m_isRetracting = true;
		m_isShooting = false;
	}

	private void DisableTongue() {
		m_tongueTrigger.EatBugs ();

		m_isRetracting = false;
		tongueTip.SetActive (false);
	}

	public void Strike() {
		Debug.Log ("Struck");

		m_tongueTrigger.DropBugs ();
		m_tongueTrigger.DisableTrigger ();
		RetractTongue ();
	}

	private void UpdateTongueDir() {
		float rot = Input.GetAxisRaw (playerInput.horizontal) * turnSpeed * Time.deltaTime;
		m_tongueDir *= Quaternion.AngleAxis (rot, Vector3.up);
	}

	private void MoveTongueForward(float speed) {
		float s = Time.deltaTime * speed;
		Vector3 dir = m_tongueDir * Vector3.forward;
		tongueTip.transform.position += s * dir;
		m_tongueLength += s;

		if (m_tongueLength / distBetweenPoints > m_tonguePoints.Count) {
			GenTonguePoint (tongueTip.transform.position);
		}

		if (m_tongueLength > maxLength) {
			RetractTongue ();
		}
	}

	private void MoveTongueBackward() {
		Vector3 dir;
		if (m_tonguePoints.Count > 0) {
			dir = m_tonguePoints [m_tonguePoints.Count - 1].position - tongueTip.transform.position;
		} else {
			dir = transform.position - tongueTip.transform.position;
		}

		float s = Time.deltaTime * retractSpeed;

		if (dir.magnitude <= s) {
			if (m_tonguePoints.Count == 0) {
				DisableTongue ();
			} else {
				RemoveLastTonguePoint ();
			}

			return;
		}

		tongueTip.transform.position += dir.normalized * s;
	}

	private void GenTonguePoint(Vector3 pos) {
		GameObject p = (GameObject)GameObject.Instantiate (tonguePointPrefab, pos, Quaternion.identity, transform);
		m_tonguePoints.Add (p.transform);
	}

	private void RemoveLastTonguePoint() {
		Transform p = m_tonguePoints [m_tonguePoints.Count - 1];
		m_tonguePoints.RemoveAt (m_tonguePoints.Count - 1);

		Destroy (p.gameObject);
	}

	public bool IsTongueOut() {
		return m_isShooting || m_isRetracting;
	}

	public bool CanMove() {
		return !IsTongueOut () && !m_isAiming;
	}

	public bool IsAiming() {
		return m_isAiming;
	}

}
