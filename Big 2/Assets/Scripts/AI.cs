using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
	public Text sumCards;
	public _GameUtil util;
	public _GameMaster master;
	List<GameObject> heldCards;
	public ActiveCards activeCards;
	float timer;
	bool canPlay;
	public List<GameObject> preparationCards_AI;

	// sumsOfEachValue counts how many of each card value
	// Ex: There are 2 ace cards in the heldcard
	// Each of the index represents a value from the lowest to highest
	// Ex: index 0 = value 3, index 1 = value 4, ..., index 13 = value 2
	public int[] sumsOfEachValue;

	// Similar with sumsOfEachValue, only that it counts the number of the suits
	// Ex: spade = 4, heart = 4, club = 3, diamond = 2
	public int[] sumsOfEachSuit;

	// Use this for initialization
	void Start () {
		sumsOfEachValue = new int[13];
		sumsOfEachSuit = new int[4];
		timer = 2;
	}
	
	// Update is called once per frame
	void Update () {
		heldCards = this.gameObject.GetComponent<CardHoldings>().heldCards;
		ShowTotalCard();
		CountEachValue();
		CountEachSuit();
		if(master.currentTurn == this.gameObject.name && heldCards.Count > 0)
		{
			TimeToThink();
			if(timer <= 0)
			{
				Think();
				timer = 2;
			}
		}
	}
	
	// Displays the total card the AI has left
	void ShowTotalCard()
	{
		sumCards.text = "x" + this.gameObject.GetComponent<CardHoldings>().heldCards.Count.ToString();
	}

	void CountEachValue()
	{
		// Clear the array
		for(int i = 0; i < 13; i++)
		{
			sumsOfEachValue[i] = 0;
		}
		//sumsOfEachValue.Capacity = 13;
		foreach(GameObject card in heldCards)
		{
			switch(util.GetValueLevel(util.GetSprite(card)))
			{
			case 3:
				sumsOfEachValue[0] += 1;
				break;
			case 4:
				sumsOfEachValue[1] += 1;
				break;
			case 5:
				sumsOfEachValue[2] += 1;
				break;
			case 6:
				sumsOfEachValue[3] += 1;
				break;
			case 7:
				sumsOfEachValue[4] += 1;
				break;
			case 8:
				sumsOfEachValue[5] += 1;
				break;
			case 9:
				sumsOfEachValue[6] += 1;
				break;
			case 10:
				sumsOfEachValue[7] += 1;
				break;
			// Jack
			case 11:
				sumsOfEachValue[8] += 1;
				break;
			// Queen
			case 12:
				sumsOfEachValue[9] += 1;
				break;
			// King
			case 13:
				sumsOfEachValue[10] += 1;
				break;
			// Ace
			case 14:
				sumsOfEachValue[11] += 1;
				break;
			// 2
			case 15:
				sumsOfEachValue[12] += 1;
				break;
			}
		}
	}

	void CountEachSuit()
	{
		// Clear the array
			for(int i = 0; i < 4; i++)
		{
			sumsOfEachSuit[i] = 0;
		}
		foreach(GameObject card in heldCards)
		{
			switch(util.GetSuitLevel(util.GetSprite(card)))
			{
			// Diamond
			case 1:
				sumsOfEachSuit[0] += 1;
				break;
			// Club
			case 2:
				sumsOfEachSuit[1] += 1;
				break;
			// Heart
			case 3:
				sumsOfEachSuit[2] += 1;
				break;
			// Spade
			case 4:
				sumsOfEachSuit[3] += 1;
				break;
			}
		}
	}

	// Removes played card from hand
	void RemoveFromHand(GameObject card)
	{
		foreach(GameObject heldCard in heldCards)
		{
			if(heldCard.name == card.name)
			{
				this.gameObject.GetComponent<CardHoldings>().heldCards.Remove(card);
				break;
			}
		}
	}

	// Sets time gap between AI turns
	void TimeToThink()
	{
		timer -= 1 * Time.deltaTime;
	}

	// Sorts the preparationCards_AI based on card value and card suit
	int CardComparer(GameObject a, GameObject b)
	{
		int valueLevel_a = util.GetValueLevel(util.GetSprite(a));
		int valueLevel_b = util.GetValueLevel(util.GetSprite(b));
		int suitLevel_a = util.GetSuitLevel(util.GetSprite(a));
		int suitLevel_b = util.GetSuitLevel(util.GetSprite(b));
		
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

	
	// The logic for AI to play or pass turn
	void Think()
	{
		canPlay = false; // A boolean check whether the AI can play any cards or not
		preparationCards_AI.Clear();
		if(master.GetTurn() == 1)
		{
			if(master.GetRound() == 1)
			{
				// Tries all possible play cards
				// Starts from the highest 5 card play
				// Tries to play a four of a kind
				// That starts with 3 of diamond
				// Due to sorting, 3 of diamond will always be in index 0
				DoFourOfAKind(heldCards[0]);

				// Tries to play a full house
				if(!canPlay)
				{
					DoFullHouse(heldCards[0]);
				}
				// Tries to play a flush
				if(!canPlay)
				{
					DoFlush(heldCards[0]);
				}
				// Tries to play a straight
				if(!canPlay)
				{
					DoStraight (heldCards[0]);
				}
				// Tries to play a triplet
				if(!canPlay)
				{
					DoTriplet(heldCards[0]);
				}
				// Tries to play a pair
				if(!canPlay)
				{
					DoPair(heldCards[0]);
				}
				// Tries to play a singleton
				if(!canPlay)
				{
					DoSingleton(heldCards[0]);
				}
			}
			else
			{
				// Tries all possible play cards
				// Starts from the highest 5 card play
				// Tries to play a four of a kind
				foreach(GameObject card in heldCards)
				{
					DoFourOfAKind(card);
					if(canPlay)
					{
						break;
					}
				}
				// Tries to play a full house
				if(!canPlay)
				{
					foreach(GameObject card in heldCards)
					{
						DoFullHouse(card);
						if(canPlay)
						{
							break;
						}
					}
				}
				// Tries to play a flush
				if(!canPlay)
				{
					foreach(GameObject card in heldCards)
					{
						DoFlush(card);
						if(canPlay)
						{
							break;
						}
					}
				}
				// Tries to play a straight
				if(!canPlay)
				{
					foreach(GameObject card in heldCards)
					{
						DoStraight(card);
						if(canPlay)
						{
							break;
						}
					}
				}
				// Tries to play a triplet
				if(!canPlay)
				{
					foreach(GameObject card in heldCards)
					{
						DoTriplet(card);
						if(canPlay)
						{
							break; // Breaks the foreach loop
						}
					}
				}
				// Tries to play a pair
				if(!canPlay)
				{
					foreach(GameObject card in heldCards)
					{
						DoPair(card);
						if(canPlay)
						{
							break;
						}
					}
				}
				// Tries to play a singleton
				if(!canPlay)
				{
					foreach(GameObject card in heldCards)
					{
						DoSingleton(card);
						if(canPlay)
						{
							break;
						}
					}
				}
			}
		}
		else
		{
			// Singleton
			if(activeCards.GetPlayingCard() == 1)
			{
				foreach(GameObject card in heldCards)
				{
					// If the card has higher value than the active card
					// Ex: 4 vs. 3, 4 is higher
					if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_1)))
					{
						preparationCards_AI.Add(card);
						canPlay = true;
						preparationCards_AI.Sort(CardComparer);
						activeCards.PlaceCards(1,preparationCards_AI);
						break;
					}
					// If it has the same value
					else if(util.GetValueLevel(util.GetSprite(card)) == util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_1)))
					{
						// Check its suit level
						// If it's higher...
						if(util.GetSuitLevel(util.GetSprite(card)) > util.GetSuitLevel(util.GetSprite(activeCards.activeCard_for_1)))
						{
							preparationCards_AI.Add(card);
							canPlay = true;
							preparationCards_AI.Sort(CardComparer);
							activeCards.PlaceCards(1,preparationCards_AI);
							break;
						}
					}
				}
			}

			// Pair
			else if(activeCards.GetPlayingCard() == 2)
			{
				foreach(GameObject card in heldCards)
				{
					// Compares the held card with the highest card of the pair
					// If the value of the card is higher
					// Ex: 4 vs. 3, 4 is higher
					if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_2[1])))
					{
						// Checks the sumsOfEachValue whether the AI has two or more cards of it
						// The index of the sumsOfEachValue is determined by the value level of the card
						// Index 0 -> value level 3, index 1 -> value level 4 
						// Therefore, difference between the index and the value level is 3
						if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 2)
						{
							// Searches the heldCards again to find the two cards of such value
							for(int j = 0; j<heldCards.Count; j++)
							{
								if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
								{
									preparationCards_AI.Add(heldCards[j]);
									if(preparationCards_AI.Count == 2)
									{
										canPlay = true;
										preparationCards_AI.Sort(CardComparer);
										activeCards.PlaceCards(2,preparationCards_AI);
										break;
									}
								}
							}
						}
					}
					// If it has the same value
					else if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_2[1])))
					{
						// Check its suit level
						// If it's higher...
						if(util.GetSuitLevel(util.GetSprite(card)) > util.GetSuitLevel(util.GetSprite(activeCards.activeCard_for_2[1])))
						{
							// Checks the sumsOfEachValue whether the AI has two or more cards of it
							// The index of the sumsOfEachValue is determined by the value level of the card
							// Index 0 -> value level 3, index 1 -> value level 4 
							// Therefore, difference between the index and the value level is 3
							if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 2)
							{
								// Searches the heldCards again to find the two cards of such value
								for(int j = 0; j<heldCards.Count; j++)
								{
									if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
									{
										preparationCards_AI.Add(heldCards[j]);
										if(preparationCards_AI.Count == 2)
										{
											canPlay = true;
											preparationCards_AI.Sort(CardComparer);
											activeCards.PlaceCards(2,preparationCards_AI);
											break;
										}
									}
								}
							}
						}
					}
					if(canPlay)
					{
						break; // Breaks the foreach loop
					}
				}
			}

			// Triplet
			else if(activeCards.GetPlayingCard() == 3)
			{
				foreach(GameObject card in heldCards)
				{
					// Compares the held card with the highest card of the triplet
					// If the value of the card is higher
					// Ex: 4 vs. 3, 4 is higher
					if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_3[2])))
					{
						// Checks the sumsOfEachValue whether the AI has three or more cards of it
						// The index of the sumsOfEachValue is determined by the value level of the card
						// Index 0 -> value level 3, index 1 -> value level 4 
						// Therefore, difference between the index and the value level is 3
						if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 3)
						{
							// Searches the heldCards again to find the two cards of such value
							for(int j = 0; j<heldCards.Count; j++)
							{
								if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
								{
									preparationCards_AI.Add(heldCards[j]);
									if(preparationCards_AI.Count == 3)
									{
										canPlay = true;
										preparationCards_AI.Sort(CardComparer);
										activeCards.PlaceCards(3,preparationCards_AI);
										break;
									}
								}
							}
						}
					}

					// If it has the same value
					else if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_3[2])))
					{
						// Check its suit level
						// If it's higher...
						if(util.GetSuitLevel(util.GetSprite(card)) > util.GetSuitLevel(util.GetSprite(activeCards.activeCard_for_3[2])))
						{
							// Checks the sumsOfEachValue whether the AI has three or more cards of it
							// The index of the sumsOfEachValue is determined by the value level of the card
							// Index 0 -> value level 3, index 1 -> value level 4 
							// Therefore, difference between the index and the value level is 3
							if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 3)
							{
								// Searches the heldCards again to find the two cards of such value
								for(int j = 0; j<heldCards.Count; j++)
								{
									if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
									{
										preparationCards_AI.Add(heldCards[j]);
										if(preparationCards_AI.Count == 3)
										{
											canPlay = true;
											preparationCards_AI.Sort(CardComparer);
											activeCards.PlaceCards(3,preparationCards_AI);
											break;
										}
									}
								}
							}
						}
					}
					if(canPlay)
					{
						break; // Breaks the foreach loop
					}
				}
			}

			// 5 cards
			else if(activeCards.GetPlayingCard() == 5)
			{
				// Straight
				if(util.IsStraight(activeCards.activeCard_for_5) && !util.IsStraightFlush(activeCards.activeCard_for_5))
				{
					foreach(GameObject card in heldCards)
					{
						// Compares the held card with the highest card of the straight
						// If the value of the card is higher
						// Ex: 8 vs. 7, 8 is higher
						if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[4])))
						{
							// Check for the previous 5 values cards
							// Ex: if the value is 8, check if value 4,5,6,7 are available
							int available = 0;
							for(int i = 1; i < 5; i++)
							{
								if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card))-3-i] >= 1)
								{
									available += 1;
								}
								else
								{
									break;
								}
							}
							// If available...
							if(available == 4)
							{
								int prevValue = 0; // A variable to avoid adding card with same value as previous
								// Search for the cards, then add them into the preparationCards_AI list
								for(int i = 0; i < heldCards.Count; i++)
								{
									int difference = util.GetValueLevel(util.GetSprite(card)) - util.GetValueLevel(util.GetSprite(heldCards[i]));
									// Add all the 5 cards
									if(difference < 5 && difference >= 0)
									{
										if(prevValue == 0 || prevValue != util.GetValueLevel(util.GetSprite(heldCards[i])))
										{
											prevValue = util.GetValueLevel(util.GetSprite(heldCards[i]));
											preparationCards_AI.Add(heldCards[i]);
										}
									}
								}
								canPlay = true;
								preparationCards_AI.Sort(CardComparer);
								activeCards.PlaceCards(5, preparationCards_AI);
								break;
							}
						}
						// If it has the same value
						else if(util.GetValueLevel(util.GetSprite(card)) == util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[4])))
						{
							// Check its suit level
							// If it's higher...
							if(util.GetSuitLevel(util.GetSprite(card)) > util.GetSuitLevel(util.GetSprite(activeCards.activeCard_for_5[4])))
							{
								// Check for the previous 5 values cards
								// Ex: if the value is 8, check if value 4,5,6,7 are available
								int available = 0;
								for(int i = 1; i < 5; i++)
								{
									if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card))-3-i] >= 1)
									{
										available += 1;
									}
									else
									{
										break;
									}
								}
								// If available...
								if(available == 4)
								{
									int prevValue = 0; // A variable to avoid adding card with same value as previous
									// Search for the cards, then add them into the preparationCards_AI list
									for(int i = 0; i < heldCards.Count; i++)
									{
										int difference = util.GetValueLevel(util.GetSprite(card)) - util.GetValueLevel(util.GetSprite(heldCards[i]));
										// Add only the previous 4 values first
										if(difference < 4 && difference >= 0)
										{
											if(prevValue == 0 || prevValue != util.GetValueLevel(util.GetSprite(heldCards[i])))
											{
												prevValue = util.GetValueLevel(util.GetSprite(heldCards[i]));
												preparationCards_AI.Add(heldCards[i]);
											}
										}
									}
									// Then add the last card
									canPlay = true;
									preparationCards_AI.Add (card);
									preparationCards_AI.Sort(CardComparer);
									activeCards.PlaceCards(5, preparationCards_AI);
									break;
								}
							}
						}
					}
					// If can't play
					// Try to play a flush
					if(!canPlay)
					{
						foreach(GameObject card in heldCards)
						{
							DoFlush(card);
						}
					}
					// If still can't play
					// Try to play a full house
					if(!canPlay)
					{
						foreach(GameObject card in heldCards)
						{
							DoFullHouse(card);
						}
					}
					// If still can't play
					// Try to play a four of a kind
					if(!canPlay)
					{
						foreach(GameObject card in heldCards)
						{
							DoFourOfAKind(card);
						}
					}
				}

				// Flush
				else if(util.IsFlush(activeCards.activeCard_for_5)&& !util.IsStraightFlush(activeCards.activeCard_for_5))
				{
					foreach(GameObject card in heldCards)
					{
						// Compares the held card with the highest card of the flush
						// If the value of the card is higher
						// Ex: 8 vs. 7, 8 is higher
						if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[4])))
						{
							// Check for the cards with the same suit
							// Ex: if the suit is spade, check for other cards with the same suit 
							if(sumsOfEachSuit[util.GetSuitLevel(util.GetSprite(card))-1] >= 5)
							{
								// Search for cards with the same suit
								foreach(GameObject heldCard in heldCards)
								{
									// Add 4 cards of the same suit
									if(util.GetSuitLevel(util.GetSprite(heldCard)) == util.GetSuitLevel(util.GetSprite(card)) && preparationCards_AI.Count <= 3
									   && util.GetValueLevel(util.GetSprite(heldCard)) != util.GetValueLevel(util.GetSprite(card)))
									{
										preparationCards_AI.Add(heldCard);
									}
								}
								// Then add the last card
								canPlay = true;
								preparationCards_AI.Add(card);
								preparationCards_AI.Sort(CardComparer);
								activeCards.PlaceCards(5, preparationCards_AI);
								break;
							}
						}
						// If it has the same value
						else if(util.GetValueLevel(util.GetSprite(card)) == util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[4])))
						{
							// Check its suit level
							// If it's higher...
							if(util.GetSuitLevel(util.GetSprite(card)) > util.GetSuitLevel(util.GetSprite(activeCards.activeCard_for_5[4])))
							{
								// Check for the cards with the same suit
								// Ex: if the suit is spade, check for other cards with the same suit 
								if(sumsOfEachSuit[util.GetSuitLevel(util.GetSprite(card))-1] >= 5)
								{
									// Search for cards with the same suit
									foreach(GameObject heldCard in heldCards)
									{
										// Add 4 cards of the same suit
										if(util.GetSuitLevel(util.GetSprite(heldCard)) == util.GetSuitLevel(util.GetSprite(card)) && preparationCards_AI.Count <= 3
										   && util.GetValueLevel(util.GetSprite(heldCard)) != util.GetValueLevel(util.GetSprite(card)))
										{
											preparationCards_AI.Add(heldCard);
										}
									}
									// Then add the last card
									canPlay = true;
									preparationCards_AI.Add(card);
									preparationCards_AI.Sort(CardComparer);
									activeCards.PlaceCards(5, preparationCards_AI);
									break;
								}
							}
						}
					}
					// If can't play
					// Try to play a full house
					if(!canPlay)
					{
						foreach(GameObject card in heldCards)
						{
							DoFullHouse(card);
						}
					}
					// If still can't play
					// Try to play a four of a kind
					if(!canPlay)
					{
						foreach(GameObject card in heldCards)
						{
							DoFourOfAKind(card);
						}
					}
				}

				// Full House
				else if(util.IsFullHouse(activeCards.activeCard_for_5))
				{
					foreach(GameObject card in heldCards)
					{
						// Compares the held card with the highest card of the triplet
						// If the value of the card is higher
						// Ex: 4 vs. 3, 4 is higher
						// Due to sorting, index 2 will always be part of the triplet
						if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[2])))
						{
							// Checks the sumsOfEachValue whether the AI has three or more cards of it
							// The index of the sumsOfEachValue is determined by the value level of the card
							// Index 0 -> value level 3, index 1 -> value level 4 
							// Therefore, difference between the index and the value level is 3
							if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 3)
							{
								// Searches the heldCards again to find the three cards of such value
								for(int j = 0; j<heldCards.Count; j++)
								{
									if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
									{
										preparationCards_AI.Add(heldCards[j]);
										if(preparationCards_AI.Count == 3)
										{
											break;
										}
									}
								}
								// Get the pair
								foreach(GameObject heldCard in heldCards)
								{
									if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(heldCard)) - 3] >= 2)
									{
										// Searches the heldCards again to find the two cards of such value
										for(int j = 0; j<heldCards.Count; j++)
										{
											if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(heldCard)))
											{
												preparationCards_AI.Add(heldCards[j]);
												if(preparationCards_AI.Count == 5)
												{
													canPlay = true;
													preparationCards_AI.Sort(CardComparer);
													activeCards.PlaceCards(5,preparationCards_AI);
													break;
												}
											}
										}
									}
									if(canPlay)
									{
										break;
									}
								}
							}
						}
						
						// If it has the same value
						else if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[2])))
						{
							// Check its suit level
							// If it's higher...
							if(util.GetSuitLevel(util.GetSprite(card)) > util.GetSuitLevel(util.GetSprite(activeCards.activeCard_for_5[2])))
							{
								// Checks the sumsOfEachValue whether the AI has three or more cards of it
								// The index of the sumsOfEachValue is determined by the value level of the card
								// Index 0 -> value level 3, index 1 -> value level 4 
								// Therefore, difference between the index and the value level is 3
								if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 3)
								{
									// Searches the heldCards again to find the three cards of such value
									for(int j = 0; j<heldCards.Count; j++)
									{
										if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
										{
											preparationCards_AI.Add(heldCards[j]);
											if(preparationCards_AI.Count == 3)
											{
												break;
											}
										}
									}
									// Get the pair
									foreach(GameObject heldCard in heldCards)
									{
										if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(heldCard)) - 3] >= 2)
										{
											// Searches the heldCards again to find the two cards of such value
											for(int j = 0; j<heldCards.Count; j++)
											{
												if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(heldCard)))
												{
													preparationCards_AI.Add(heldCards[j]);
													if(preparationCards_AI.Count == 5)
													{
														canPlay = true;
														preparationCards_AI.Sort(CardComparer);
														activeCards.PlaceCards(5,preparationCards_AI);
														break;
													}
												}
											}
										}
										if(canPlay)
										{
											break;
										}
									}
								}
							}
						}
						if(canPlay)
						{
							break; // Breaks the foreach loop
						}
					}
					// If can't play
					// Try to play a four of a kind
					if(!canPlay)
					{
						foreach(GameObject card in heldCards)
						{
							DoFourOfAKind(card);
						}
					}
				}

				// Four of a Kind
				else if(util.IsFourOfAKind(activeCards.activeCard_for_5))
				{
					foreach(GameObject card in heldCards)
					{
						// Compares the held card with the highest card of the quad
						// If the value of the card is higher
						// Ex: 4 vs. 3, 4 is higher
						// Due to sorting, the index 2 will always be the part of the quad
						if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[2])))
						{
							// Checks the sumsOfEachValue whether the AI has three or more cards of it
							// The index of the sumsOfEachValue is determined by the value level of the card
							// Index 0 -> value level 3, index 1 -> value level 4 
							// Therefore, difference between the index and the value level is 3
							if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 4)
							{
								// Searches the heldCards again to find the four cards of such value
								for(int j = 0; j<heldCards.Count; j++)
								{
									if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
									{
										preparationCards_AI.Add(heldCards[j]);
										if(preparationCards_AI.Count == 4)
										{
											break;
										}
									}
								}
								// Add one last card
								for(int i = 0; i<heldCards.Count;i++)
								{
									if(util.GetValueLevel(util.GetSprite(heldCards[i])) != util.GetValueLevel(util.GetSprite(card)))
									{
										preparationCards_AI.Add(heldCards[i]);
										canPlay = true;
										preparationCards_AI.Sort(CardComparer);
										activeCards.PlaceCards(5,preparationCards_AI);
										break;
									}
								}
							}
						}
						// If it has the same value
						else if(util.GetValueLevel(util.GetSprite(card)) > util.GetValueLevel(util.GetSprite(activeCards.activeCard_for_5[2])))
						{
							// Check its suit level
							// If it's higher...
							if(util.GetSuitLevel(util.GetSprite(card)) > util.GetSuitLevel(util.GetSprite(activeCards.activeCard_for_5[2])))
							{
								// Checks the sumsOfEachValue whether the AI has three or more cards of it
								// The index of the sumsOfEachValue is determined by the value level of the card
								// Index 0 -> value level 3, index 1 -> value level 4 
								// Therefore, difference between the index and the value level is 3
								if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 4)
								{
									// Searches the heldCards again to find the four cards of such value
									for(int j = 0; j<heldCards.Count; j++)
									{
										if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
										{
											preparationCards_AI.Add(heldCards[j]);
											if(preparationCards_AI.Count == 4)
											{
												break;
											}
										}
									}
									// Add one last card
									for(int i = 0; i<heldCards.Count;i++)
									{
										if(util.GetValueLevel(util.GetSprite(heldCards[i])) != util.GetValueLevel(util.GetSprite(card)))
										{
											preparationCards_AI.Add(heldCards[i]);
											canPlay = true;
											preparationCards_AI.Sort(CardComparer);
											activeCards.PlaceCards(5,preparationCards_AI);
											break;
										}
									}
								}
							}
						}
						if(canPlay)
						{
							break; // Breaks the foreach loop
						}
					}
				}
			}


		}
		// If the AI cannot play any cards
		if(!canPlay)
		{
			master.PassTurn();
		}
		else
		{
			foreach(GameObject card in preparationCards_AI)
			{
				RemoveFromHand(card);
			}
			preparationCards_AI.Clear(); // Clears the list from previous played card
			master.PlayTurn();
			
		}
	}

	// Does a normal singleton without checking active cards
	void DoSingleton(GameObject card)
	{
		preparationCards_AI.Add(card);
		canPlay = true;
		preparationCards_AI.Sort(CardComparer);
		activeCards.SetPlayingCard(1);
		activeCards.PlaceCards(1,preparationCards_AI);
	}
	
	// Does a normal pair without checking active cards
	void DoPair(GameObject card)
	{
		if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 2)
		{
			// Searches the heldCards again to find the two cards of such value
			for(int j = 0; j<heldCards.Count; j++)
			{
				if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
				{
					preparationCards_AI.Add(heldCards[j]);
					if(preparationCards_AI.Count == 2)
					{
						canPlay = true;
						preparationCards_AI.Sort(CardComparer);
						activeCards.SetPlayingCard(2);
						activeCards.PlaceCards(2,preparationCards_AI);
						break;
					}
				}
			}
		}
	}
	
	// Does a normal triplet without checking active cards
	void DoTriplet(GameObject card)
	{
		// Checks the sumsOfEachValue whether the AI has three or more cards of it
		// The index of the sumsOfEachValue is determined by the value level of the card
		// Index 0 -> value level 3, index 1 -> value level 4 
		// Therefore, difference between the index and the value level is 3
		if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 3)
		{
			// Searches the heldCards again to find the two cards of such value
			for(int j = 0; j<heldCards.Count; j++)
			{
				if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
				{
					preparationCards_AI.Add(heldCards[j]);
					if(preparationCards_AI.Count == 3)
					{
						canPlay = true;
						preparationCards_AI.Sort(CardComparer);
						activeCards.SetPlayingCard(3);
						activeCards.PlaceCards(3,preparationCards_AI);
						break;
					}
				}
			}
		}

	}
	
	// Does a normal straight without checking active cards
	void DoStraight(GameObject card)
	{
		// Check for the previous 5 values cards
		// Ex: if the value is 8, check if value 4,5,6,7 are available
		int available = 0;
		for(int i = 1; i < 5; i++)
		{
			if(util.GetValueLevel(util.GetSprite(card))-3-i >= 0 && sumsOfEachValue[util.GetValueLevel(util.GetSprite(card))-3-i] >= 1)
			{
				available += 1;
			}
			else
			{
				break;
			}
		}
		// If available...
		if(available == 4)
		{
			int prevValue = 0; // A variable to avoid adding card with same value as previous
			// Search for the cards, then add them into the preparationCards_AI list
			for(int i = 0; i < heldCards.Count; i++)
			{
				int difference = util.GetValueLevel(util.GetSprite(card)) - util.GetValueLevel(util.GetSprite(heldCards[i]));
				// Add all the 5 cards
				if(difference < 5 && difference >= 0)
				{
					if(prevValue == 0 || prevValue != util.GetValueLevel(util.GetSprite(heldCards[i])))
					{
						prevValue = util.GetValueLevel(util.GetSprite(heldCards[i]));
						preparationCards_AI.Add(heldCards[i]);
					}
				}
			}
			canPlay = true;
			preparationCards_AI.Sort(CardComparer);
			activeCards.SetPlayingCard(5);
			activeCards.PlaceCards(5, preparationCards_AI);
		}
	}
	
	// Does a normal flush without checking active cards
	void DoFlush(GameObject card)
	{
		// Check for the cards with the same suit
		// Ex: if the suit is spade, check for other cards with the same suit 
		if(sumsOfEachSuit[util.GetSuitLevel(util.GetSprite(card))-1] >= 5)
		{
			// Search for cards with the same suit
			foreach(GameObject heldCard in heldCards)
			{
				// Add 4 cards of the same suit
				if(util.GetSuitLevel(util.GetSprite(heldCard)) == util.GetSuitLevel(util.GetSprite(card)) && preparationCards_AI.Count <= 3
				   && util.GetValueLevel(util.GetSprite(heldCard)) != util.GetValueLevel(util.GetSprite(card)))
				{
					preparationCards_AI.Add(heldCard);
				}
			}
			// Then add the last card
			canPlay = true;
			preparationCards_AI.Add(card);
			preparationCards_AI.Sort(CardComparer);
			activeCards.SetPlayingCard(5);
			activeCards.PlaceCards(5, preparationCards_AI);
		}
	}
	
	// Does a normal full house without checking active cards
	void DoFullHouse(GameObject card)
	{
		// Checks the sumsOfEachValue whether the AI has three or more cards of it
		// The index of the sumsOfEachValue is determined by the value level of the card
		// Index 0 -> value level 3, index 1 -> value level 4 
		// Therefore, difference between the index and the value level is 3
		if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 3)
		{
			// Searches the heldCards again to find the three cards of such value
			for(int j = 0; j<heldCards.Count; j++)
			{
				if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
				{
					preparationCards_AI.Add(heldCards[j]);
					if(preparationCards_AI.Count == 3)
					{
						break;
					}
				}
			}
			// Get the pair
			foreach(GameObject heldCard in heldCards)
			{
				if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(heldCard)) - 3] >= 2)
				{
					// Searches the heldCards again to find the two cards of such value
					for(int j = 0; j<heldCards.Count; j++)
					{
						if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(heldCard)))
						{
							preparationCards_AI.Add(heldCards[j]);
							if(preparationCards_AI.Count == 5)
							{
								canPlay = true;
								preparationCards_AI.Sort(CardComparer);
								activeCards.SetPlayingCard(5);
								activeCards.PlaceCards(5,preparationCards_AI);
								break;
							}
						}
					}
				}
				if(canPlay)
				{
					break;
				}
			}
		}
	}
	
	// Does a normal four of a kind without checking active cards
	void DoFourOfAKind(GameObject card)
	{
		if(sumsOfEachValue[util.GetValueLevel(util.GetSprite(card)) - 3] >= 4)
		{
			// Searches the heldCards again to find the four cards of such value
			for(int j = 0; j<heldCards.Count; j++)
			{
				if(util.GetValueLevel(util.GetSprite(heldCards[j])) == util.GetValueLevel(util.GetSprite(card)))
				{
					preparationCards_AI.Add(heldCards[j]);
					if(preparationCards_AI.Count == 4)
					{
						break;
					}
				}
			}
			// Add one last card
			for(int i = 0; i<heldCards.Count;i++)
			{
				if(util.GetValueLevel(util.GetSprite(heldCards[i])) != util.GetValueLevel(util.GetSprite(card)))
				{
					Debug.Log(".");
					preparationCards_AI.Add(heldCards[i]);
					canPlay = true;
					preparationCards_AI.Sort(CardComparer);
					activeCards.SetPlayingCard(5);
					activeCards.PlaceCards(5,preparationCards_AI);
					break;
				}
			}
		}
	}
}
