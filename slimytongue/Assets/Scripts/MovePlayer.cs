using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	public float baseSpeed;
    public float jumpVel;
	public float jumpDist;
	public float jumpResetTime;

	private PlayerInput playerInput;
	private Animator animator;

    private Rigidbody m_rigidbody;
	private SphereCollider m_collider;
	private TongueController m_tongueController;

	private bool m_isGrounded;
	private bool m_isJumping;

	private float m_speedMult;

	private Vector3 m_jumpDir;
	private float m_currJumpDist;
	private float m_disableTimer;

	public ParticleSystem particlesDust;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
		m_tongueController = GetComponent<TongueController> ();
		m_speedMult = 1.0f;

		m_collider = GetComponent<SphereCollider> ();
		m_isGrounded = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (m_disableTimer > 0) {
			m_disableTimer -= Time.fixedDeltaTime;
			return;
		}

		Vector3 input = playerInput.GetMovement();

		Vector3 movement = Vector3.zero;

		if (!m_isJumping && !m_tongueController.IsTongueOut ()) {

			if (!m_tongueController.IsAiming ()) {
				if (playerInput.GetJumpDown ()) {
					Jump (input);
				} else {
					movement = Move (input);
				}
			}

			Aim (input);
		} else if (m_isJumping) {
			movement = DoJump ();
		}

		transform.position += movement;

		animator.SetFloat ("Speed", movement.magnitude);
	}

	private void Aim(Vector3 input) {
		if (input == Vector3.zero) {
			return;
		}

		transform.rotation = Quaternion.LookRotation (input.normalized);
	}

	private Vector3 Move(Vector3 input) {
		if (input == Vector3.zero) {
			particlesDust.startSpeed = 0;
			return Vector3.zero;
		}
		particlesDust.startSpeed = 0.005f;
		return input * Time.fixedDeltaTime * baseSpeed * m_speedMult;
	}

    private void Jump(Vector3 input)
    {
		if (input.magnitude == 0) {
			input = transform.forward;
		}

		m_isJumping = true;
		m_jumpDir = input.normalized;
		m_currJumpDist = 0;
    }

	private Vector3 DoJump() {
		float s = Time.fixedDeltaTime * jumpVel * m_speedMult;

		m_currJumpDist += s;
		if (m_currJumpDist >= jumpDist) {
			m_isJumping = false;
			m_disableTimer = jumpResetTime;
		}

		return s * m_jumpDir;
	}

	private void UpdateIsGrounded() {
		//RaycastHit hit;
		//
		//Vector3 p1 = transform.position + charCtrl.center;
		//float distanceToObstacle = 0;
		//
		//// Cast a sphere wrapping character controller 10 meters forward
		//// to see if it is about to hit anything.
		//if (Physics.SphereCast(p1, charCtrl.height / 2, transform.forward, out hit, 10)) {
		//	distanceToObstacle = hit.distance;
		//}
	}

	public void SetSpeedMult(float speedMult) {
		m_speedMult = speedMult;
	}
}
