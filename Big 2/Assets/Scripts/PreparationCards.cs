using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreparationCards : MonoBehaviour {
	public GameObject GM;
	_GameUtil util;
	public List<Sprite> preparationList; // list of the prepared cards
	public List<GameObject> preparationCards; // list of the position of the prepared cards
	public GameObject player;
	public List<GameObject> activeCards;

	// Use this for initialization
	void Start () {
		util = GM.GetComponent<_GameUtil>();
	}
	
	// Update is called once per frame
	void Update () {
		DisplayCards();
	}

	// Displays the cards in the preparation list to the screen
	void DisplayCards()
	{
		preparationList.Sort(CardComparer);
		for(int i = 0; i < preparationList.Count; i++)
		{
			preparationCards[i].GetComponent<SpriteRenderer>().sprite = preparationList[i];
		} 
	}

	// Sorts the preparation list based on card value and card type
	int CardComparer(Sprite a, Sprite b)
	{
		int valueLevel_a = util.GetValueLevel(a);
		int valueLevel_b = util.GetValueLevel(b);
		int typeLevel_a = util.GetTypeLevel(a);
		int typeLevel_b = util.GetTypeLevel(b);

		// Checks for sorting position
		// If value level a is lesser than value level b
		if(valueLevel_a < valueLevel_b)
		{
			return -1;
		}
		// If value level a is greater than value level b
		else if(valueLevel_a > valueLevel_b)
		{
			return 1;
		}
		// If value level a is the same with value level b
		else
		{
			// Compares the type level
			// If the type of a is weaker than b
			if(typeLevel_a < typeLevel_b)
			{
				return -1;
			}
			// If the type of a is stronger than b
			// Type of a can't be the same with type of b
			// As there is only a copy for each card
			else
			{
				return 1;
			}
		}
	}

	public void PlayCard()
	{
		// Removes the cards from hand
		foreach(GameObject playerCard in player.GetComponent<Player>().playerCards)
		{
			if(playerCard.GetComponent<SpriteRenderer>().sprite == null)
			{
				foreach(GameObject heldCard in player.GetComponent<CardHoldings>().heldCards)
				{
					if(playerCard.name == heldCard.name)
					{
						player.GetComponent<CardHoldings>().heldCards.Remove(heldCard);
						break;
					}
				}
			}
		}
		// To be redisplayed (updated), removes the display of empty card(s)
		ClearPlayerCards();

		// Counts the number of cards
		int sumCards = 0;
		foreach(GameObject preparationCard in preparationCards)
		{
			if(preparationCard.GetComponent<SpriteRenderer>().sprite != null)
			{
				sumCards++;
			}
		}

		// Places cards from preparation hand to active hand
		PlaceCards(sumCards);

		// Clears the preparation list
		preparationList.Clear();

		// Clears all display of preparation cards display to be redisplayed 
		for(int j = 0; j < preparationCards.Count;j++)
		{
			preparationCards[j].GetComponent<SpriteRenderer>().sprite = null;
		} 
	}

	void PlaceCards(int sumCards)
	{
		if(sumCards == 1)
		{
			activeCards[2].GetComponent<SpriteRenderer>().sprite = preparationCards[0].GetComponent<SpriteRenderer>().sprite;
		}
		else if(sumCards == 2)
		{
			activeCards[5].GetComponent<SpriteRenderer>().sprite = preparationCards[0].GetComponent<SpriteRenderer>().sprite;
			activeCards[6].GetComponent<SpriteRenderer>().sprite = preparationCards[1].GetComponent<SpriteRenderer>().sprite;
		}
		else if(sumCards == 3)
		{
			for(int i = 0; i<3;i++)
			{
				activeCards[i+1].GetComponent<SpriteRenderer>().sprite = preparationCards[i].GetComponent<SpriteRenderer>().sprite;
			}
		}
		else if(sumCards == 5)
		{
			for(int i = 0; i<5;i++)
			{
				activeCards[i].GetComponent<SpriteRenderer>().sprite = preparationCards[i].GetComponent<SpriteRenderer>().sprite;
			}
		}
	}

	void ClearPlayerCards()
	{
		foreach(GameObject playerCard in player.GetComponent<Player>().playerCards)
		{
			playerCard.GetComponent<PlayerCard>().cardSprite = null;
			playerCard.name = playerCard.GetComponent<PlayerCard>().originalName;
			playerCard.GetComponent<PlayerCard>().isClicked = false;
		}
	}
}
