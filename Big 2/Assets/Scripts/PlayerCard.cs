using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCard : MonoBehaviour {
	Vector3 originalPosition;
	string originalName;
	Sprite sprite;
	public Sprite nullSprite;

	// Use this for initialization
	void Start () {
		originalPosition = this.transform.position;
		originalName = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;

		/*
		// change gameobject name into the card name it possesses 
		if(sprite != null)
		{
			this.gameObject.name = sprite.name;
		}
		// revert gameobject name into its original name when it possesses no card
		else
		{
			this.gameObject.name = originalName;
		}
		*/
	}

	void OnMouseEnter()
	{
		this.transform.Translate(0,0.1f,0);
	}

	void OnMouseExit()
	{
		this.transform.position = originalPosition;
	}

	void OnMouseDown()
	{
		if(Input.GetMouseButtonDown(0) && sprite != null)
		{

			sprite = nullSprite;
		}
	}
}
