using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCard : MonoBehaviour {
	Vector3 originalPosition;
	public string originalName;
	public Sprite cardSprite;
	Sprite currentSprite;
	public bool isClicked;
	public GameObject preparationCardsObject;
	PreparationCards preparationCards;

	// Use this for initialization
	void Start () {
		preparationCards = preparationCardsObject.GetComponent<PreparationCards>();
		originalPosition = this.transform.position;
		originalName = this.gameObject.name;
		isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isClicked)
		{
			currentSprite = cardSprite;
			this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
		this.gameObject.GetComponent<SpriteRenderer>().sprite = currentSprite; // This object's sprite is controlled by the currentSprite
	}

	// Moves card up when mouse hovers above the card
	void OnMouseEnter()
	{
		this.transform.Translate(0,0.1f,0);
	}

	// Moves card back at original position when mouse quits hovering the card
	void OnMouseExit()
	{
		this.transform.position = originalPosition;
	}

	// Moves card into preparation card list when clicked
	void OnMouseDown()
	{
		if(Input.GetMouseButtonDown(0) && currentSprite != null && preparationCards.preparationList.Count<5)
		{
			isClicked = true;
			PrepareCard();
			currentSprite = null;
		}
	}

	// Prepare the card to be played
	void PrepareCard()
	{
		// Add the card sprite to the preparation list
		preparationCards.preparationList.Add(currentSprite);
	}
}
