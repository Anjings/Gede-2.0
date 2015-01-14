using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActiveCards : MonoBehaviour {
	public GameObject activeCard_for_1;
	public List<GameObject> activeCard_for_2;
	public List<GameObject> activeCard_for_3;
	public List<GameObject> activeCard_for_5;
	public _GameUtil util;
	public _GameMaster master;
	public Text cardStatus;
	int playingCard = 0; // Number of cards played (1,2,3,5)
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(master.GetTurn() == 1)
		{
			ClearAll();
			cardStatus.text = "";
			playingCard = 0;
		}
	}

	public void SetPlayingCard(int sum)
	{
		playingCard = sum;
	}
	
	public int GetPlayingCard()
	{
		return playingCard;
	}

	bool CheckCombination(int sumCards, List<GameObject> cards)
	{
		if(sumCards == 1)
		{
			cardStatus.text = "Singleton";
			return true;
		}
		else if(sumCards == 2)
		{
			if(util.IsOnePair(cards))
			{
				cardStatus.text = "One Pair";
				return true;
			}
			return false;
		}
		else if(sumCards == 3)
		{
			if(util.IsTriple(cards))
			{
				cardStatus.text = "Triple";
				return true;
			}
			return false;
		}
		else if(sumCards == 5)
		{
			if(util.IsStraightFlush(cards))
			{
				cardStatus.text = "Straight Flush";
				return true;
			}
			else if(util.IsStraight(cards))
			{
				cardStatus.text = "Straight";
				return true;
			}
			else if(util.IsFlush(cards))
			{
				cardStatus.text = "Flush";
				return true;
			}
			else if(util.IsFullHouse(cards))
			{
				cardStatus.text = "Full House";
				return true;
			}
			else if(util.IsFourOfAKind(cards))
			{
				cardStatus.text = "Four of a Kind";
				return true;
			}
		}
		return false;
	}

	public void PlaceCards(int sumCards, List<GameObject> cards)
	{
		ClearAll();
		if(sumCards == 1 && playingCard == 1)
		{
			if(CheckCombination(sumCards,cards))
			{
				activeCard_for_1.GetComponent<SpriteRenderer>().sprite = util.GetSprite(cards[0]);
			}
		}
		else if(sumCards == 2 && playingCard == 2)
		{
			if(CheckCombination(sumCards,cards))
			{
				for(int i=0; i<2; i++)
				{
					activeCard_for_2[i].GetComponent<SpriteRenderer>().sprite = util.GetSprite(cards[i]);
				}
			}
		}
		else if(sumCards == 3 && playingCard == 3)
		{
			if(CheckCombination(sumCards,cards))
			{
				for(int i=0; i<3; i++)
				{
					activeCard_for_3[i].GetComponent<SpriteRenderer>().sprite = util.GetSprite(cards[i]);
				}
			}

		}
		else if(sumCards == 5 && playingCard == 5)
		{
			if(CheckCombination(sumCards,cards))
			{
				for(int i=0; i<5; i++)
				{
					activeCard_for_5[i].GetComponent<SpriteRenderer>().sprite = util.GetSprite(cards[i]);
				}
			}
		}
	}

	// Clears the display of the active cards 
	void ClearAll()
	{
		activeCard_for_1.GetComponent<SpriteRenderer>().sprite = null;
		foreach(GameObject activeCard in activeCard_for_2)
		{
			activeCard.GetComponent<SpriteRenderer>().sprite = null;
		}
		foreach(GameObject activeCard in activeCard_for_3)
		{
			activeCard.GetComponent<SpriteRenderer>().sprite = null;
		}
		foreach(GameObject activeCard in activeCard_for_5)
		{
			activeCard.GetComponent<SpriteRenderer>().sprite = null;
		}
	}

	public bool IsValid(int sumCards, List<GameObject> preparationCards)
	{
		if(sumCards == 1)
		{
			return true;
		}
		else if(sumCards == 2)
		{
			return util.IsOnePair(preparationCards);
		}
		else if(sumCards == 3)
		{
			return util.IsTriple(preparationCards);
		}
		else if(sumCards == 5)
		{
			if(util.IsStraightFlush(preparationCards))
			{
				return true;
			}
			else if(util.IsStraightFlush(preparationCards))
			{
				return true;
			}
			else if(util.IsStraight(preparationCards))
			{
				return true;
			}
			else if(util.IsFlush(preparationCards))
			{
				return true;
			}
			else if(util.IsFullHouse(preparationCards))
			{
				return true;
			}
			else if(util.IsFourOfAKind(preparationCards))
			{
				return true;
			}
		}
		return false;
	}

	// Checks whether the active card is lower than the prepared card
	public bool IsLower(int sumCards, List<GameObject> preparedCards)
	{
		// For one card
		if(sumCards == 1)
		{
			// Card played has higher value level
			// Ex: card played = 5, active card = 3 -> 5 is higher than 3
			if(util.GetValueLevel(util.GetSprite(preparedCards[0])) > util.GetValueLevel(util.GetSprite(activeCard_for_1)))
			{
				return true;
			}
			// If both values are the same...
			else if(util.GetValueLevel(util.GetSprite(preparedCards[0])) == util.GetValueLevel(util.GetSprite(activeCard_for_1)))
			{
				// Checks for the suit
				// If it's higher...
				// Ex: card played = spade, active card = diamond -> spade is higher than diamond
				if(util.GetSuitLevel(util.GetSprite(preparedCards[0])) > util.GetSuitLevel(util.GetSprite(activeCard_for_1)))
				{
					return true;
				}
			}
			return false;
		}

		// For one pair
		else if(sumCards == 2)
		{
			// Card on the last member of the list played has higher value level 
			// Due to sorting, the last member is always the highest
			// Ex: card played = 5, active card = 3 -> 5 is higher than 3
			if(util.GetValueLevel(util.GetSprite(preparedCards[1])) > util.GetValueLevel(util.GetSprite(activeCard_for_2[1])))
			{
				return true;
			}
			// If both values are the same...
			else if(util.GetValueLevel(util.GetSprite(preparedCards[1])) == util.GetValueLevel(util.GetSprite(activeCard_for_2[1])))
			{
				// Checks for the suit
				// If it's higher...
				// Ex: card played = spade, active card = diamond -> spade is higher than diamond
				if(util.GetSuitLevel(util.GetSprite(preparedCards[1])) > util.GetSuitLevel(util.GetSprite(activeCard_for_2[1])))
				{
					return true;
				}
			}
			return false;
		}

		// For triplet
		else if(sumCards == 3)
		{
			// Card on the last member of the list played has highest value level 
			// Due to sorting, the last member is always the highest
			// Ex: card played = 5, active card = 3 -> 5 is higher than 3
			if(util.GetValueLevel(util.GetSprite(preparedCards[2])) > util.GetValueLevel(util.GetSprite(activeCard_for_3[2])))
			{
				return true;
			}
			// If both values are the same...
			else if(util.GetValueLevel(util.GetSprite(preparedCards[2])) == util.GetValueLevel(util.GetSprite(activeCard_for_3[2])))
			{
				// Checks for the suit
				// If it's higher...
				// Ex: card played = spade, active card = diamond -> spade is higher than diamond
				if(util.GetSuitLevel(util.GetSprite(preparedCards[2])) > util.GetSuitLevel(util.GetSprite(activeCard_for_3[2])))
				{
					return true;
				}
			}
			return false;
		}

		// For five cards
		else if(sumCards == 5)
		{
			// Checks which combination is higher
			// Ex: straight flush is higher than full house
			// If the played one is higher...
			if(util.GetComboLevel(preparedCards) > util.GetComboLevel(activeCard_for_5))
			{
				return true;
			}
			// If it's the same...
			// Ex: full house vs full house
			else if(util.GetComboLevel(preparedCards) == util.GetComboLevel(activeCard_for_5))
			{
				// The last member of straight and flush hands determines which is higher
				if(util.IsStraightFlush(preparedCards) || util.IsStraight(preparedCards) || util.IsFlush(preparedCards))
				{
					// Card on the last member of the list played has highest value level 
					// Due to sorting, the last member is always the highest
					// Ex: card played = 5, active card = 3 -> 5 is higher than 3
					if(util.GetValueLevel(util.GetSprite(preparedCards[4])) > util.GetValueLevel(util.GetSprite(activeCard_for_5[4])))
					{
						return true;
					}
					// If both values are the same...
					else if(util.GetValueLevel(util.GetSprite(preparedCards[4])) == util.GetValueLevel(util.GetSprite(activeCard_for_5[4])))
					{
						// Checks for the suit
						// If it's higher...
						// Ex: card played = spade, active card = diamond -> spade is higher than diamond
						if(util.GetSuitLevel(util.GetSprite(preparedCards[4])) > util.GetSuitLevel(util.GetSprite(activeCard_for_5[4])))
						{
							return true;
						}
					}
					return false;
				}
				// The third member of the cards is always a part of the triplet of quad
				else if(util.IsFullHouse(preparedCards) || util.IsFourOfAKind(preparedCards))
				{
					// Card on the last member of the list played has highest value level 
					// Due to sorting, the last member is always the highest
					// Ex: card played = 5, active card = 3 -> 5 is higher than 3
					if(util.GetValueLevel(util.GetSprite(preparedCards[2])) > util.GetValueLevel(util.GetSprite(activeCard_for_5[2])))
					{
						return true;
					}
					// If both values are the same...
					else if(util.GetValueLevel(util.GetSprite(preparedCards[2])) == util.GetValueLevel(util.GetSprite(activeCard_for_5[2])))
					{
						// Checks for the suit
						// If it's higher...
						// Ex: card played = spade, active card = diamond -> spade is higher than diamond
						if(util.GetSuitLevel(util.GetSprite(preparedCards[2])) > util.GetSuitLevel(util.GetSprite(activeCard_for_5[2])))
						{
							return true;
						}
					}
					return false;
				}
			}
		}
		return false;
	}
}
