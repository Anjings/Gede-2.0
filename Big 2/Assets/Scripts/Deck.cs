using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
	List<GameObject> players;
	public List<GameObject> cards;

	// Use this for initialization
	void Start () {
		players = GameObject.Find("GM").GetComponent<_GM>().players;
		DistributeCards();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DistributeCards()
	{
		foreach(GameObject card in cards)
		{
			int playerId = Random.Range(0,4);
			GiveCard(card,playerId);
		}
	}

	void GiveCard(GameObject card, int playerId)
	{
		List<GameObject> onHand;
		if(playerId == 0)
		{
			onHand = players[playerId].GetComponent<CardHoldings>().heldCards;
			if(onHand.Count < 13)
			{
				onHand.Add(card);
			}
			else
			{
				playerId = 1;
			}
		}
		if(playerId == 1)
		{
			onHand = players[playerId].GetComponent<CardHoldings>().heldCards;
			if(onHand.Count < 13)
			{
				onHand.Add(card);
			}
			else
			{
				playerId = 2;
			}
		}
		if(playerId == 2)
		{
			onHand = players[playerId].GetComponent<CardHoldings>().heldCards;
			if(onHand.Count < 13)
			{
				onHand.Add(card);
			}
			else
			{
				playerId = 3;
			}
		}
		if(playerId == 3)
		{
			onHand = players[playerId].GetComponent<CardHoldings>().heldCards;
			if(onHand.Count < 13)
			{
				onHand.Add(card);
			}
			else
			{
				GiveCard(card,0);
			}
		}
	}
}
