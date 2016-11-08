﻿using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	public float speed;

	public PlayerInput playerInput;
	public Animator animator;

	private TongueController m_tongueController;

	// Use this for initialization
	void Start () {
		m_tongueController = GetComponent<TongueController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 input = Vector3.zero;
		input.x = Input.GetAxisRaw (playerInput.horizontal);
		input.z = Input.GetAxisRaw (playerInput.vertical);

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

		return input.normalized * Time.deltaTime * speed;
	}
}