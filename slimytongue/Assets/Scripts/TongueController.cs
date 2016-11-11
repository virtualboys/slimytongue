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

	private PlayerInput playerInput;

	private TongueTrigger m_tongueTrigger;
	private FrogSize m_frogSize;

	private bool m_isShooting;
    private bool m_isFastShot;
	private bool m_isRetracting;
	private bool m_isAiming;

	private Quaternion m_tongueDir;
	private float m_tongueLength;

	private List<Transform> m_tonguePoints;

	void Awake() {
		m_tonguePoints = new List<Transform> ();
	}

	void Start () {
        playerInput = GetComponent<PlayerInput>();
		m_tongueTrigger = tongueTip.GetComponent<TongueTrigger> ();
        m_tongueTrigger.SetController(this);
		m_frogSize = GetComponent<FrogSize> ();
		tongueTip.SetActive (false);
	}

	void Update () {
		if(!IsTongueOut() && playerInput.GetShootDown()) {
			m_isAiming = true;

		} else if (m_isAiming && playerInput.GetShootUp()) {
			ShootTongue ();

		} else if (m_isShooting) {
			if(playerInput.GetShootHeld())
            {
                m_isFastShot = true;
            }

            if(m_isFastShot) {
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
        m_isFastShot = false;
		m_tongueLength = 0;
		m_tongueDir = transform.rotation;

		tongueTip.transform.position = transform.position;
		tongueTip.SetActive (true);
		m_tongueTrigger.EnableTrigger ();
	}

	public void RetractTongue() {
		m_isRetracting = true;
		m_isShooting = false;
	}

	private void DisableTongue() {
		List<BugController> bugs = m_tongueTrigger.GetBugs ();
		for (int i = 0; i < bugs.Count; i++) {
			m_frogSize.GrowSize (bugs [i].GetSize ());
		}
		m_tongueTrigger.DestroyBugs ();

		m_isRetracting = false;
		tongueTip.SetActive (false);
	}

	public void Strike() {
        if (IsTongueOut())
        {
            m_tongueTrigger.DropBugs();
            m_tongueTrigger.DisableTrigger();
            RetractTongue();
        }
	}

    public Vector3 GetTongueVelocity()
    {
        Vector3 v = m_tongueDir * Vector3.forward;
        v *= (m_isFastShot) ? fastTongueSpeed : baseTongueSpeed;
        return v;
    }

	private void UpdateTongueDir() {
        Vector3 aim = playerInput.GetAimDir();
        if(aim.magnitude == 0)
        {
            return;
        }

        Quaternion targetDir = Quaternion.LookRotation(playerInput.GetAimDir());

        //float rot = Input.GetAxisRaw (playerInput.horizontal) * turnSpeed * Time.deltaTime;
        //m_tongueDir *= Quaternion.AngleAxis (rot, Vector3.up);
        m_tongueDir = Quaternion.RotateTowards(m_tongueDir, targetDir, turnSpeed * Time.deltaTime);
		tongueTip.transform.rotation = m_tongueDir;
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

		dir.Normalize ();
		tongueTip.transform.position += dir * s;
		tongueTip.transform.rotation = Quaternion.LookRotation (-dir);
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

    public bool CanPickupBug()
    {
        return m_isShooting;
    }

	public GameObject GetTongueTip() {
		return tongueTip;
	}

	public List<Transform> GetControlPoints() {
		return m_tonguePoints;
	}

}
