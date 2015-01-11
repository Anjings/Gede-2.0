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
		DisplayPlayerCards();
	}

	// Displays all held cards by the player to the screen
	void DisplayPlayerCards()
	{
		int i = 0;
		foreach(GameObject heldCard in heldCards)
		{
			playerCards[i].name = heldCard.name;
			playerCards[i].GetComponent<PlayerCard>().cardSprite = heldCard.GetComponent<SpriteRenderer>().sprite;
			i++;
		}
	}


	
}
