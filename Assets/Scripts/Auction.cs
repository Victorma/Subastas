using UnityEngine;
using System.Collections.Generic;

public class Auction : MonoBehaviour {

	public static Auction currentAuction;


	public Item[] items;
	public int currentSlot;

	public enum AuctionState {
		Nothing/*, BuyingObjects, SpendingPubli*/, Selecting, Auctioning, Buying 
	}

	private AuctionState state;
	public AuctionState State {
		get { return state; }
		set {
			if(GetComponent(state.ToString()+"Script")!=null);
				Destroy(GetComponent(state.ToString()+"Script"));

			state = value;
			this.gameObject.AddComponent(State.ToString()+"Script");
		}
	}

	private AuctionState[] states = new AuctionState[]{
		AuctionState.Nothing,
		AuctionState.Selecting,
		AuctionState.Auctioning,
		AuctionState.Buying
	};

	public void nextState(){
		int pos = (int) State;
		pos = (pos+1)%states.Length;
		State = states[pos];
	}

	public Auction current {
		get{ return currentAuction; }
	}

	// Use this for initialization
	void Awake () {
		if(currentAuction == null){	
			currentAuction = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else						GameObject.Destroy(this);
	}

	public Item getAuctioningItem(){
		return items[currentSlot];
	}

	void Start(){
		State = AuctionState.Buying;
	}


}
