using UnityEngine;
using System.Collections.Generic;

public class Auction : MonoBehaviour {

	public static Auction currentAuction;

	public AuctionState state;
	public Item[] items;
	public int currentSlot;

	public enum AuctionState {
		BuyingObjects, SpendingPubli, Auctioning 
	}

	public Auction current {
		get{ return currentAuction; }
	}

	// Use this for initialization
	void Start () {
		if(currentAuction == null){
			currentAuction = this;
			state = AuctionState.BuyingObjects;
		}else
			GameObject.Destroy(this);
	}

	public Item getAuctioningItem(){
		return null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
