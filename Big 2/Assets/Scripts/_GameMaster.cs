using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class _GameMaster : MonoBehaviour {
	public List<GameObject> players; // List of all players
	_GameUtil util;
	public int round = 0; // No. of round in a game (round = 0 -> new game)
	public int turn = 0; // No. of turn in a round 

	public string currentTurn; // Determines whose turn it is
	public Text turnStatus;
	int passCount = 0;

	// Use this for initialization
	void Start () {
		util = this.gameObject.GetComponent<_GameUtil>();
	}
	
	// Update is called once per frame
	void Update () {
		if(round == 0 && turn == 0)
		{
			StartRound();
		}
		// Updates the turn status display
		turnStatus.text = currentTurn + "\'s turn";
	}

	void StartRound()
	{
		round = 1;
		turn = 1;
		// The player who has 3 of Diamond starts first
		LocateThreeDiamond();
	}

	void NextTurn()
	{
		int index = 0;
		// Finds the index of the current player in the player list
		index = players.IndexOf(GameObject.Find(currentTurn));

		// If the player is the last in the list
		// It's the first player in the list's turn
		if(index + 1 >= players.Count)
		{
			index = 0;
		}
		// Else it's the next player in the list's turn
		else
		{
			index += 1;
		}
		currentTurn = players[index].name;
		turn += 1;
	}

	// Passes the turn
	public void PassTurn()
	{
		// Player cannot pass the first turn
		if(turn != 1)
		{
			passCount += 1;
			// 3 pass counts mean that the player wins the round
			if(passCount >= 3)
			{
				passCount = 0; // Resets the pass count
				round += 1; // Moves to next round
				turn = 0; // Resets the turn
			}
			NextTurn();
		}
	}

	// Plays the turn
	public void PlayTurn()
	{
		// Resets the pass count
		passCount = 0;
		NextTurn();
	}

	void LocateThreeDiamond()
	{
		// Checks held cards of each player for 3 diamond
		foreach(GameObject player in players)
		{
			foreach(GameObject card in player.GetComponent<CardHoldings>().heldCards)
			{
				Sprite cardSprite = card.GetComponent<SpriteRenderer>().sprite;
				// 3 diamond -> level = 1 (diamond), value = "3"
				if(util.GetSuitLevel(cardSprite) == 1 && util.GetValueLevel(cardSprite) == 3)
				{
					currentTurn = player.name;
				}
			}
		}
	}
	

	public int GetTurn()
	{
		return turn;
	}

	public int GetRound()
	{
		return round;
	}
}
