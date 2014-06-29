using UnityEngine;
using System.Collections.Generic;

public class Auction : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Vector2 size = new Vector2(480, 30);

	public static Auction currentAuction;
	
	public Item[] items;
	public int currentSlot;

	public float dinero = 10000;

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

	public void OnGUI(){
		float rx = (Screen.width / (float)targetScreenSize.x);
		float ry = (Screen.height/ (float)targetScreenSize.y);
		
		Matrix4x4 bc = GUI.matrix;
		
		GUI.depth = 1;
		
		GUI.matrix = Matrix4x4.TRS (new Vector2(0, 0) , Quaternion.identity, new Vector3 (rx, ry, 1));

		GUILayout.BeginArea(new Rect(0,0, size.x, size.y));


		GUILayout.EndArea();


		GUI.matrix = bc;

	}


}
