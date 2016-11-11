using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {

	public static List<GameObject> players;

    public int playerNum;

    private bool isController;
	private string shootTongue;
	private string horizontal;
	private string vertical;
    private string jump;

	// Use this for initialization
	void Start () {
		switch(Application.platform) {

		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			if (playerNum == 0) {
				jump = "Leap";
				shootTongue = "Jump";
				horizontal = "Horizontal";
				vertical = "Vertical";
			} else if (playerNum == 1) {
				jump = "Leap2";
				shootTongue = "Jump2";
				horizontal = "Horizontal2";
				vertical = "Vertical2";
			}
			break;

		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.OSXEditor:
			if (playerNum == 0) {
				jump = "Leap";
				shootTongue = "JumpMac";
				horizontal = "Horizontal";
				vertical = "Vertical";
			} else if (playerNum == 1) {
				jump = "Leap2";
				shootTongue = "Jump2Mac";
				horizontal = "Horizontal2";
				vertical = "Vertical2";
			}
			break;
		}

		if (players == null) {
			players = new List<GameObject> ();
		}
		players.Add (gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public Vector3 GetAimDir()
    {
        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxis(horizontal);
        dir.z = Input.GetAxis(vertical);
        dir.Normalize();

        return dir;
    }

    public bool GetShootDown()
    {
        return Input.GetButtonDown(shootTongue);
    }

    public bool GetJumpDown()
    {
        return Input.GetButtonDown(jump);
    }

    public bool GetShootUp()
    {
        return Input.GetButtonUp(shootTongue);
    }

    public bool GetShootHeld()
    {
        return Input.GetButton(shootTongue);
    }
}
