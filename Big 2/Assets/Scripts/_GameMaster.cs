using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _GameMaster : MonoBehaviour {
	public List<GameObject> players;
	_GameUtil util;
	int stage;
	int turn;
	public string playerTurn;
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
