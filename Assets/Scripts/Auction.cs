using UnityEngine;
using System.Collections.Generic;

public class Auction : MonoBehaviour {

	public static Auction currentAuction;
	
	public Item[] items;
	public int currentSlot;

	public AuctionState state;
	private AuctionState lastState;

	private void changeState(){

		Component c = GetComponent(lastState.ToString()+"Script");
		if(c!=null)	Destroy(c);

		lastState = state;
		this.gameObject.AddComponent(state.ToString()+"Script");
	}

	public enum AuctionState {
		Nothing, Buying, Publi, Selecting, Auctioning
	}

	private AuctionState[] states = new AuctionState[]{
		AuctionState.Nothing,
		AuctionState.Selecting,
		AuctionState.Auctioning,
		AuctionState.Buying
	};

	public void nextState(){
		int pos = (int) state;
		pos = (pos+1)%states.Length;
		state = states[pos];
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
		this.changeState();
	}

	void Update(){
		if(lastState!=state)
			this.changeState();

	}


}
