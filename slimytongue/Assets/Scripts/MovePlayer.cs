using UnityEngine;

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

        MaintainRotation();
        print(transform.rotation);

		if (m_tongueController.CanMove ()) {
			movement = Move (input);
		}

		if (m_tongueController.CanMove () || m_tongueController.IsAiming ()) {
			Aim (input);
		}

		transform.position += movement;

		animator.SetFloat ("Speed", movement.magnitude);

        KeepPlayerGrounded();
	}

    private void MaintainRotation()
    {
        var rot = transform.rotation;
        rot.Set(rot.x, 0.0f, rot.z, 1.0f);
        transform.rotation = rot;
    }

    private void KeepPlayerGrounded()
    {
        // start ray at player pos
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit hitInfo;

        // get da damn layer for our ground. should have a mesh collider
        LayerMask layer = 1 << LayerMask.NameToLayer("Ground");

        //cast ray
        if (Physics.Raycast(ray, out hitInfo, layer))
        {
            //get where our raycast hit on the ground
            float z = hitInfo.point.z;

            // get the current position
            Vector3 pos = transform.position;
            
            // change the z of our current position so that boy don't leave the ground.
            pos.z = z;

            // override the current position with the grounded position
            transform.position = pos;
        }
        //todo: might need an else here in case he somehow gets flipped upside down???
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
