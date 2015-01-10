using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreparationCards : MonoBehaviour {
	public List<Sprite> preparationList; // list of the prepared cards
	public List<GameObject> preparationCards; // list of the position of the prepared cards
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DisplayCards();
	}

	// Displays the cards in the preparation list to the screen
	void DisplayCards()
	{
		for(int i = 0; i < preparationList.Count; i++)
		{
			preparationCards[i].GetComponent<SpriteRenderer>().sprite = preparationList[i];
		} 
	}
}
