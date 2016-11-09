using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	public float baseSpeed;
	private float m_speedMult;

	public PlayerInput playerInput;
	public Animator animator;

	private TongueController m_tongueController;

	// Use this for initialization
	void Start () {
		m_tongueController = GetComponent<TongueController> ();
		m_speedMult = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 input = playerInput.GetAimDir();

		Vector3 movement = Vector3.zero;

		if (m_tongueController.CanMove ()) {
			movement = Move (input);
		}

		if (m_tongueController.CanMove () || m_tongueController.IsAiming ()) {
			Aim (input);
		}

		transform.position += movement;

		animator.SetFloat ("Speed", movement.magnitude);
	}

	private void Aim(Vector3 input) {
		if (input == Vector3.zero) {
			return;
		}

		transform.rotation = Quaternion.LookRotation (input);
	}

	private Vector3 Move(Vector3 input) {
		if (input == Vector3.zero) {
			return Vector3.zero;
		}

		return input.normalized * Time.deltaTime * baseSpeed * m_speedMult;
	}

	public void SetSpeedMult(float speedMult) {
		m_speedMult = speedMult;
	}
}
