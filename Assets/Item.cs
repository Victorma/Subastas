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

	public enum Type{
		normal, art
	}

	public string name;
	public string description;
	public Sprite image;
	public Type type;
	public RareType rareness;
	public Size size;
	public float BuyingPrice;
	public float buyingPrice{ 
		get{ if(!discount)return BuyingPrice;
			else return Mathf.Round(BuyingPrice*(1f-variants[currentVariant].percentdiscount));}
		set{ BuyingPrice = value; }
	}
	public float SellingPrice;
	public float sellingPrice{ 
		get{ if(!discount)return SellingPrice;
			else return Mathf.Round(SellingPrice*(1f-variants[currentVariant].percentdiscount));}
		set{ SellingPrice = value; }
	}
	public List<string> types;

	public List<ItemVariant> variants;

	public int currentVariant = 0;

	private bool discount = false;

	public static Item generateVariant(Item item){
		Item itm = ScriptableObject.CreateInstance<Item> ();

		itm.name = item.name;
		itm.description = item.description;
		itm.image = item.image;
		itm.rareness = item.rareness;
		itm.size = item.size;

		float rand = Random.Range (1, 100) / 100f;
		itm.buyingPrice = item.buyingPrice-(item.buyingPrice*0.1f)+(item.buyingPrice*0.2f*rand);

		itm.sellingPrice = item.sellingPrice;
		itm.types = item.types;
		itm.type = item.type;
		itm.variants = item.variants;

		rand = Random.Range (1, 100) / 100f;

		float prob = 0f;
		for (int i=0; i<item.variants.Count; i++) {
			prob+=item.variants[i].probability;
			if(rand<prob){
				itm.currentVariant = i; break;
			}
		}

		return itm;
	}

	public void activateDiscounts(){
		this.discount = true;
	}
}
