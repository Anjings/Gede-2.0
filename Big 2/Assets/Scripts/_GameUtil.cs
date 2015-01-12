using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class _GameUtil : MonoBehaviour {

	public int GetTypeLevel(Sprite cardSprite)
	{
		int level = 0;
		// ex: spade_ace -> type = spade
		string[] splitString = cardSprite.name.Split('_');
		string type = splitString[0];
		
		// spade = level 4 (the strongest type), diamond = level 1 (the weakest type)
		switch(type)
		{
		case "spade":
			level = 4;
			break;
		case "heart":
			level = 3;
			break;
		case "club":
			level = 2;
			break;
		case "diamond":
			level = 1;
			break;
		}
		return level;
	}

	string GetCardValue(Sprite cardSprite)
	{
		// ex: spade_ace -> value = ace
		string[] splitString = cardSprite.name.Split('_');
		string value = splitString[1];
		return value;
	}

	// Gets the value power level of the card value(needed to determine which is higher or lower)
	public int GetValueLevel(Sprite cardSprite)
	{
		string value = GetCardValue(cardSprite);
		int level = 0;
		switch(value)
		{
		case "3":
			level = 3;
			break;
		case "4":
			level = 4;
			break;
		case "5":
			level = 5;
			break;
		case "6":
			level = 6;
			break;
		case "7":
			level = 7;
			break;
		case "8":
			level = 8;
			break;
		case "9":
			level = 9;
			break;
		case "10":
			level = 10;
			break;
		case "jack":
			level = 11;
			break;
		case "queen":
			level = 12;
			break;
		case "king":
			level = 13;
			break;
		case "ace":
			level = 14;
			break;
		case "2":
			level = 15;
			break;
		}
		return level;
	}

	int CountCards(List<GameObject> cards)
	{
		int i = 0;
		foreach(GameObject card in cards)
		{
			if(card.GetComponent<SpriteRenderer>().sprite != null)
			{
				i++;
			}
		}
		return i;
	}

	public bool IsOnePair(List<GameObject> cards)
	{
		// Counts the cards played
		int sumCards = CountCards(cards);
		// Checks the total of the cards played (must be 2)
		if(sumCards == 2)
		{
			// Checks whether both cards have the same value
			if(GetValueLevel(GetSprite(cards[0])) == GetValueLevel(GetSprite(cards[1])))
			{
				return true;
			}
		}
		return false;
	}
	public bool IsTriple(List<GameObject> cards)
	{
		// Counts the cards played
		int sumCards = CountCards(cards);
		// Checks the total of the cards played (must be 3)
		if(sumCards == 3)
		{
			// Checks whether all cards have the same value
			if(GetValueLevel(GetSprite(cards[0])) == GetValueLevel(GetSprite(cards[1])) && GetValueLevel(GetSprite(cards[0])) == GetValueLevel(GetSprite(cards[2])))
			{
				return true;
			}
		}
		return false;
	}
	public bool IsStraight(List<GameObject> cards)
	{
		// Counts the cards played
		int sumCards = CountCards(cards);

		// Math formula: a + (a+1) + (a+2) + (a+3) + (a+4) = 5a + 10
		// Checks the total of the cards played (must be 5)
		if(sumCards == 5)
		{
			// Counts the total value level
			int sumValue = 0;
			foreach(GameObject card in cards)
			{
				sumValue += GetValueLevel(GetSprite(card));
			}

			// Checks if the value of the cards are in sequence
			if(5*GetValueLevel(GetSprite(cards[0]))+10 == sumValue)
			{
				return true;
			}
		}
		return false;
	}
	public bool IsFlush(List<GameObject> cards)
	{
		// Counts the cards played
		int sumCards = CountCards(cards);
		// Checks the total of the cards played (must be 5)
		if(sumCards == 5)
		{
			int typeLevel = GetTypeLevel(GetSprite(cards[0]));
			// Checks whether all cards have the same type
			for(int i = 0; i < 5; i++)
			{
				// If the type is different...
				if(GetTypeLevel(GetSprite(cards[i])) != typeLevel)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}
	public bool IsFullHouse(List<GameObject> cards)
	{
		// Counts the cards played
		int sumCards = CountCards(cards);
		// Checks the total of the cards played (must be 5)
		if(sumCards == 5)
		{
			// Due to sorting, the index 2/third list member will always be the part of the triple
			int valueLevel = GetValueLevel(GetSprite(cards[2]));
			int sameValue = 0;
			// Checks whether there are three cards with same value 
			foreach(GameObject card in cards)
			{
				if(GetValueLevel(GetSprite(card)) == valueLevel)
				{
					sameValue++;
				}
			}
			if(sameValue == 3)
			{
				// Checks if the other two cards are a pair
				// Due to sorting, the pair can only be in the index 0,1 or 3,4
				if(GetValueLevel(GetSprite(cards[0])) == GetValueLevel(GetSprite(cards[1])) || 
				   GetValueLevel(GetSprite(cards[3])) == GetValueLevel(GetSprite(cards[4])))
				{
					return true;
				}
			}
		}
		return false;
	}
	public bool IsFourOfAKind(List<GameObject> cards)
	{
		// Counts the cards played
		int sumCards = CountCards(cards);
		// Checks the total of the cards played (must be 5)
		if(sumCards == 5)
		{
			// Due to sorting, the index 1/second list member will always be the part of the quad
			int valueLevel = GetValueLevel(GetSprite(cards[1]));
			int sameValue = 0;
			// Checks whether there are four cards with same value 
			foreach(GameObject card in cards)
			{
				if(GetValueLevel(GetSprite(card)) == valueLevel)
				{
					sameValue++;
				}
			}
			if(sameValue == 4)
			{
				return true;
			}
		}
		return false;
	}
	public bool IsStraightFlush(List<GameObject> cards)
	{
		// If the cards are both straight and flush
		if(IsStraight(cards) && IsFlush(cards))
		{
			return true;
		}
		return false;
	}

	// Gets the value of the 5 cards played
	public int GetComboLevel(List<GameObject> cards)
	{
		if(cards.Count == 5)
		{
			if(IsStraightFlush(cards))
			{
				return 5;
			}
			else if(IsStraight(cards))
			{
				return 1;
			}
			else if(IsFlush(cards))
			{
				return 2;
			}
			else if(IsFullHouse(cards))
			{
				return 3;
			}
			else if(IsFourOfAKind(cards))
			{
				return 4;
			}
			return 0;
		}
		return 0;
	}

	public Sprite GetSprite(GameObject card)
	{
		return card.GetComponent<SpriteRenderer>().sprite;
	}
	
}
