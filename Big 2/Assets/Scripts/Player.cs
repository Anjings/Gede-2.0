using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	List<GameObject> heldCards;
	public List<GameObject> cardPlacements;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		heldCards = this.gameObject.GetComponent<CardHoldings>().heldCards;
		DisplayPlayerCard();
	}

	void DisplayPlayerCard()
	{
		int i = 0;
		foreach(GameObject card in heldCards)
		{
			cardPlacements[i].GetComponent<SpriteRenderer>().sprite = card.GetComponent<SpriteRenderer>().sprite;
			i++;
		}
	}
}
