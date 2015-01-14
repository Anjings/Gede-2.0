using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreparationCards : MonoBehaviour {
	public _GameUtil util;
	public _GameMaster master;
	public ActiveCards activeCards;
	public List<Sprite> preparationList; // list of the prepared cards
	public List<GameObject> preparationCards; // list of the position of the prepared cards in the preparation list
	public GameObject player;

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
		preparationList.Sort(CardComparer);
		for(int i = 0; i < preparationList.Count; i++)
		{
			preparationCards[i].GetComponent<SpriteRenderer>().sprite = preparationList[i];
		} 
	}

	// Sorts the preparation list based on card value and card suit
	int CardComparer(Sprite a, Sprite b)
	{
		int valueLevel_a = util.GetValueLevel(a);
		int valueLevel_b = util.GetValueLevel(b);
		int suitLevel_a = util.GetSuitLevel(a);
		int suitLevel_b = util.GetSuitLevel(b);

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
			// Compares the suit level
			// If the suit of a is weaker than b
			if(suitLevel_a < suitLevel_b)
			{
				return -1;
			}
			// If the suit of a is stronger than b
			// suit of a can't be the same with suit of b
			// As there is only a copy for each card
			else
			{
				return 1;
			}
		}
	}

	public void SubmitCard()
	{
		// Counts the number of cards
		int sumCards = 0;
		foreach(GameObject preparationCard in preparationCards)
		{
			if(util.GetSprite(preparationCard) != null)
			{
				sumCards++;
			}
		}

		// If the number of the cards played are correct (1,2,3,5)
		if(sumCards > 0 && sumCards != 4)
		{
			// Checks the validity of the card combination
			if(activeCards.IsValid(sumCards, preparationCards))
			{
				if(master.GetTurn() == 1)
				{
					// At round 1 stage 1, player must play 3 of diamond
					if(master.GetRound() == 1)
					{
						if(preparationCards[0].name == "diamond_3")
						{
							activeCards.SetPlayingCard(sumCards);
							PlayCard(sumCards);
						}
					}
					else
					{
						activeCards.SetPlayingCard(sumCards);
						PlayCard(sumCards);
					}
				}
				else
				{
					if(activeCards.GetPlayingCard() == sumCards && activeCards.IsLower(sumCards, preparationCards))
					{
						PlayCard(sumCards);
					}
				}
			}
		}
	}

	void PlayCard(int sumCards)
	{
		master.PlayTurn();
		// Removes the cards from hand
		foreach(GameObject playerCard in player.GetComponent<Player>().playerCards)
		{
			if(util.GetSprite(playerCard)== null)
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
		
		// Places cards from preparation hand to active hand
		activeCards.PlaceCards(sumCards, preparationCards);
		
		// Clears the preparation list
		preparationList.Clear();
		
		// Clears all display of preparation cards display to be redisplayed 
		for(int j = 0; j < preparationCards.Count;j++)
		{
			preparationCards[j].GetComponent<SpriteRenderer>().sprite = null;
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
