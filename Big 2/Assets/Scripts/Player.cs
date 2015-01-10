using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	List<GameObject> heldCards;
	public List<GameObject> playerCards;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		heldCards = this.gameObject.GetComponent<CardHoldings>().heldCards;
		DisplayPlayerCard();
	}

	// Displays all held cards by the player to the screen
	void DisplayPlayerCard()
	{
		int i = 0;
		foreach(GameObject card in heldCards)
		{
			playerCards[i].GetComponent<PlayerCard>().cardSprite = card.GetComponent<SpriteRenderer>().sprite;
			i++;
		}
	}
}
