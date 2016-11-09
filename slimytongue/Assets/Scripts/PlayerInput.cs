using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    public int playerNum;

    private bool isController;
	private string shootTongue;
	private string horizontal;
	private string vertical;

	// Use this for initialization
	void Start () {
        if (playerNum == 0)
        {
            shootTongue = "Jump";
            horizontal = "Horizontal";
            vertical = "Vertical";
        }
        else if (playerNum == 1)
        {
            shootTongue = "Jump2";
            horizontal = "Horizontal2";
            vertical = "Vertical2";
        }
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

        Debug.Log(dir);

        return dir;
    }

    public bool GetShootDown()
    {
        return Input.GetButtonDown(shootTongue);
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
