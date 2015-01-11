﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class _GameUtil : MonoBehaviour {

	public string LocateThreeDiamond(List<GameObject> players)
	{
		// checks held cards of each player for 3 diamond
		foreach(GameObject player in players)
		{
			foreach(GameObject card in player.GetComponent<CardHoldings>().heldCards)
			{
				Sprite cardSprite = card.GetComponent<SpriteRenderer>().sprite;
				// 3 diamond -> level = 1 (diamond), value = "3"
				if(GetTypeLevel(cardSprite) == 1 && GetCardValue(cardSprite) == "3")
				{
					return player.name;
				}
			}
		}
		return "";
	}
	
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

	public bool IsOnePair(List<GameObject> cards)
	{
		return true;
	}
	public bool IsThreeOfAKind(List<GameObject> cards)
	{
		return true;
	}
	public bool IsStraight(List<GameObject> cards)
	{
		return true;
	}
	public bool IsFlush(List<GameObject> cards)
	{
		return true;
	}
	public bool IsFullHouse(List<GameObject> cards)
	{
		return true;
	}
	public bool IsFourOfAKind(List<GameObject> cards)
	{
		return true;
	}
	public bool IsStraightFlush(List<GameObject> cards)
	{
		return true;
	}
}
