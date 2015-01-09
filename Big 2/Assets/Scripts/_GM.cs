using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _GM : MonoBehaviour {
	public List<GameObject> players;
	int stage;
	int turn;
	public string playerTurn;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void StartStage()
	{
		stage = 1;
		turn = 1;
		LocateThreeDiamond();
	}

	void LocateThreeDiamond()
	{
		// checks held cards of each player for 3 diamond
		foreach(GameObject player in players)
		{
			foreach(GameObject card in player.GetComponent<CardHoldings>().heldCards)
			{
				// 3 diamond -> level = 1 (diamond), value = "3"
				if(GetCardLevel(card) == 1 && GetCardValue(card) == "3")
				{
					playerTurn = player.name;
					break;
				}
			}
		}
	}

	string GetCardValue(GameObject card)
	{
		// ex: spade_ace -> value = ace
		string[] splitString = card.name.Split('_');
		string value = splitString[1];
		return value;
	}

	int GetCardLevel(GameObject card)
	{
		int level = 0;
		// ex: spade_ace -> type = spade
		string[] splitString = card.name.Split('_');
		string type = splitString[0];
	
		// spade = level 4 (the strongest type), diamond = level 1 (the weakest type)
		switch(type)
		{
		case "spade":
			level = 4;
			break;
		case "heart":
			level = 3;
			break;
		case "club":
			level = 2;
			break;
		case "diamond":
			level = 1;
			break;
		}
		return level;
	}
}
