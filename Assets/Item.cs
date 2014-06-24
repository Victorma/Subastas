using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Item : ScriptableObject{

	public enum RareType{
		Common, Uncommon, Rare, Epic, Legendary
	}

	public enum Size{
		small, medium, large
	}

	public string name;
	public string description;
	public Sprite image;
	public RareType rareness;
	public Size size;
	public float buyingPrice;
	public float sellingPrice;
	public List<string> types;

}
