using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PreparationCard : MonoBehaviour {
	public GameObject preparationCardsObject; 
	PreparationCards preparationCards;
	public GameObject player;
	Sprite currentSprite;
	string originalName;

	// Use this for initialization
	void Start () {
		originalName = this.gameObject.name;
		preparationCards = preparationCardsObject.GetComponent<PreparationCards>();
	}
	
	// Update is called once per frame
	void Update () {
		currentSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
		// Change gameobject name into the card name it possesses 
		if(currentSprite != null)
		{
			this.gameObject.name = currentSprite.name;
		}
		else
		{
			this.gameObject.name = originalName;
		}
	}

	// Puts back the card onto player's hand from the preparation card list when clicked
	void OnMouseDown()
	{
		if(Input.GetMouseButtonDown(0) && currentSprite != null)
		{
			DrawBack();
		}
	}

	// Draws the prepared card back to hand
	void DrawBack()
	{
		// Returns the card to hand
		foreach(GameObject playerCard in player.GetComponent<Player>().playerCards)
		{
			if(playerCard.name == currentSprite.name)
			{
				playerCard.GetComponent<PlayerCard>().isClicked = false;
				break;
			}
		}

		// Removes the card from the preparation list
		preparationCards.preparationList.Remove(currentSprite);

		// Clears all display of preparation cards display to be redisplayed (updated card position) in PreparationCards class
		for(int j = 0; j < preparationCards.preparationCards.Count;j++)
		{
			preparationCards.preparationCards[j].GetComponent<SpriteRenderer>().sprite = null;
		} 
	}
}
