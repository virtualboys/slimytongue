﻿using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	public float baseSpeed;
    public float jumpVel;
	private float m_speedMult;

	private PlayerInput playerInput;
	private Animator animator;

    private Rigidbody m_rigidbody;
	private SphereCollider m_collider;
	private TongueController m_tongueController;
	private bool m_isGrounded;

	public ParticleSystem particlesDust;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
		m_tongueController = GetComponent<TongueController> ();
		m_speedMult = 1.0f;

		m_collider = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 input = playerInput.GetAimDir();

		Vector3 movement = Vector3.zero;

		if (m_tongueController.CanMove ()) {
            //if(!m_jumping)
            {
                if(playerInput.GetJumpDown())
                {
                    Jump(input);
                } else
                {
			        movement = Move (input);
                }
            } 
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
			particlesDust.startSpeed = 0;
			return Vector3.zero;
		}
		particlesDust.startSpeed = 0.005f;
		return input.normalized * Time.deltaTime * baseSpeed * m_speedMult;
	}

    private void Jump(Vector3 input)
    {
        input.y = 1;
        m_rigidbody.AddForce(input * jumpVel * m_rigidbody.mass);
        Debug.Log("Jump");

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
