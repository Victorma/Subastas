using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Item : ScriptableObject{

	public enum RareType{
		Common, Uncommon, Rare, Epic, Legendary
	}

	public string name;
	public Sprite image;
	public RareType rareness;
	public float buyingPrice;
	public float sellingPrice;
	public List<string> types;

}
