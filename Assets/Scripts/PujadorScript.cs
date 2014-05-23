using UnityEngine;
using System.Collections;

public class PujadorScript : MonoBehaviour {

	private float lastBid;
	private bool isBidding;

	//public BidPrefab bidPrefab;
	public float frenzy;
	public float money;
	public float maxBid;
	public float frenzyIncrementPerOwnBid;
	public float frenzyIncrementPerOtherBid;

	private PeopleScript people;



	// Use this for initialization
	void Start () {
		people = GetComponent<PeopleScript>();
	}

	// Update is called once per frame
	void Update () {
		if(this.enabled){
			if(!people.Moving)
				GetComponent<Animator>().Play("south");


			//Item item = Auction.current.item;
			//float val = people.eval(item);

			//if(Auction.current.price + Auction.current.bid < money){
			//
			//}
		}
	}
}
