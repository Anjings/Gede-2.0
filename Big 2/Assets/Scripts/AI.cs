using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AI : MonoBehaviour {
	public Text sumCards;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ShowTotalCard();
	}

	void ShowTotalCard()
	{
		sumCards.text = "x" + this.gameObject.GetComponent<CardHoldings>().heldCards.Count.ToString();
	}
}
