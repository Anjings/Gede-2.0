using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _GameMaster : MonoBehaviour {
	public List<GameObject> players; // List of all players
	_GameUtil util;
	int stage; // No. of stage in a game
	int turn; // No. of turn in a stage
	public string playerTurn; // Determines whose turn it is

	// Use this for initialization
	void Start () {
		util = this.gameObject.GetComponent<_GameUtil>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void StartStage()
	{
		stage = 1;
		turn = 1;
		util.LocateThreeDiamond(players);
	}


}
