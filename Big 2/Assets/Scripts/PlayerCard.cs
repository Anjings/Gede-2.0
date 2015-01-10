using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCard : MonoBehaviour {
	Vector3 originalPosition;
	string originalName;
	public Sprite cardSprite;
	public Sprite currentSprite;
	bool isClicked;
	public GameObject preparationPlace;

	// Use this for initialization
	void Start () {
		originalPosition = this.transform.position;
		originalName = this.gameObject.name;
		isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isClicked)
		{
			currentSprite = cardSprite;
		}
		this.gameObject.GetComponent<SpriteRenderer>().sprite = currentSprite; // This object's sprite is controlled by the currentSprite

		// Change gameobject name into the card name it possesses 
		if(currentSprite != null)
		{
			this.gameObject.name = currentSprite.name;
		}
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
		if(Input.GetMouseButtonDown(0) && currentSprite != null && !IsPrepareListFull())
		{
			isClicked = true;
			PrepareCard();
			currentSprite = null;
		}
	}

	// Prepare the card to be played
	void PrepareCard()
	{
		int i = 0;
		foreach(Sprite sprite in preparationPlace.GetComponent<PreparationCards>().preparationList)
		{
			if(sprite == null)
			{
				// Fills the sprite of the first empty slot in the list
				preparationPlace.GetComponent<PreparationCards>().preparationList[i] = currentSprite;
				break;
			}
			i++;
		}
	}

	// Checks if the preparation list is full (max 5 cards)
	bool IsPrepareListFull()
	{
		int nullSprite = 0; // The indicator of empty slot in the list

		for(int i=0; i < preparationPlace.GetComponent<PreparationCards>().preparationList.Count; i++)
		{
			if(preparationPlace.GetComponent<PreparationCards>().preparationList[i] == null)
			{
				nullSprite += 1;
			}
		}
		if(nullSprite != 0)
		{
			return false;
		}
		return true;
	}
}
