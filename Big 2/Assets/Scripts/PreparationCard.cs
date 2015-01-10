using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PreparationCard : MonoBehaviour {
	public GameObject preparationCards;
	public GameObject player;
	Sprite currentSprite;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
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
				playerCard.GetComponent<PlayerCard>().currentSprite = currentSprite;
				break;
			}
		}

		// Removes the card from the list
		int i = 0;
		foreach(Sprite sprite in preparationCards.GetComponent<PreparationCards>().preparationList)
		{
			if(sprite == currentSprite)
			{
				preparationCards.GetComponent<PreparationCards>().preparationList[i] = null;
				break;
			}
			i++;
		}
	}
}
