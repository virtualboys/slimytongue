using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public FrogSize[] players;

	public float gameTime;

	public TextMesh gameOver;

	public TextMesh time;

	public TextMesh[] scores;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > gameTime) {
			gameOver.text = "GAME OVER !!!";
		}
		else{
			time.text=((int)Time.time).ToString();
		}
		for(int i=0; i<players.Length; i++){
			scores[i].text=(players[i].score).ToString();
		}

	}
}
