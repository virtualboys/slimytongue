using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {

	public static List<GameObject> players;

    public int playerNum;

	private bool m_isController;
	private string m_shootTongue;
	private string m_horizontal;
	private string m_vertical;
	private string m_jump;

	// Use this for initialization
	void Start () {
		switch(Application.platform) {

		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			if (playerNum == 0) {
				m_jump = "Leap";
				m_shootTongue = "Jump";
				m_horizontal = "Horizontal";
				m_vertical = "Vertical";
			} else if (playerNum == 1) {
				m_jump = "Leap2";
				m_shootTongue = "Jump2";
				m_horizontal = "Horizontal2";
				m_vertical = "Vertical2";
			}
			break;

		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.OSXEditor:
			if (playerNum == 0) {
				m_jump = "Leap";
				m_shootTongue = "JumpMac";
				m_horizontal = "Horizontal";
				m_vertical = "Vertical";
			} else if (playerNum == 1) {
				m_jump = "Leap2";
				m_shootTongue = "Jump2Mac";
				m_horizontal = "Horizontal2";
				m_vertical = "Vertical2";
			}
			break;
		}

		if (playerNum < Input.GetJoystickNames ().Length) {
			m_isController = true;
		} else {
			m_isController = false;
		}

		if (players == null) {
			players = new List<GameObject> ();
		}
		players.Add (gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

	/// <summary>
	/// normalized direction
	/// </summary>
	/// <returns>The aim dir.</returns>
    public Vector3 GetAimDir()
    {
        Vector3 dir = Vector3.zero;
		dir.x = GetHorizAxis ();
		dir.z = GetVertAxis ();
        dir.Normalize();

        return dir;
    }

	/// <summary>
	/// non normalized direction
	/// </summary>
	/// <returns>The movement.</returns>
	public Vector3 GetMovement() {
		Vector3 dir = Vector3.zero;
		dir.x = GetHorizAxis ();
		dir.z = GetVertAxis ();

		return dir;
	}

	public float GetHorizAxis() {
		if (m_isController) {
			return Input.GetAxis (m_horizontal);
		} else {
			return Input.GetAxisRaw (m_horizontal);
		}
	}

	public float GetVertAxis() {
		if (m_isController) {
			return Input.GetAxis (m_vertical);
		} else {
			return Input.GetAxisRaw (m_vertical);
		}
	}

    public bool GetShootDown()
    {
        return Input.GetButtonDown(m_shootTongue);
    }

    public bool GetJumpDown()
    {
        return Input.GetButtonDown(m_jump);
    }

    public bool GetShootUp()
    {
        return Input.GetButtonUp(m_shootTongue);
    }

    public bool GetShootHeld()
    {
        return Input.GetButton(m_shootTongue);
    }
}
