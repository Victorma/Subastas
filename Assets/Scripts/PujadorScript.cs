using UnityEngine;
using System.Collections;

public class PujadorScript : MonoBehaviour {
	
	private bool isBidding;

	public GameObject bidPrefab;
	public GameObject bidInstance;

	public float frenzy;
	public float money;
	public float maxBid;

	public float frenzyDecrementPerSecond;
	public float frenzyIncrementPerOwnBid;
	public float frenzyIncrementPerOtherBid;

	private PeopleScript people;

	private Animator animator;

	// Use this for initialization
	void Start () {
		people = GetComponent<PeopleScript>();
		animator = GetComponent<Animator> ();
		if (animator == null)
			animator = GetComponentInChildren<Animator> ();
	}

	private float timePerTry = 0.2f;
	private float actualTime = 0;
	
	public AnimationCurve PriceProbabilityCurve;
	public AnimationCurve BidProbabilityCurve;
	public AnimationCurve FrenzyProbabilityCurve;
	public AnimationCurve LikeProbabilityCurve;

	 
	// Update is called once per frame
	void Update () {
		if(this.enabled){
			if(!people.Moving){
				animator.Play("south");

				AuctioningScript auct = Auction.currentAuction.GetComponent<AuctioningScript>();
				if(auct == null)
					return;

				if(auct.ganadorActual != this)
					frenzy -= frenzyDecrementPerSecond*Time.deltaTime;

				if(bidInstance != null || auct.ganadorActual == this)
					return;

				if(actualTime <= 0){

					float probability = PriceProbabilityCurve.Evaluate(auct.price / money) *
										BidProbabilityCurve.Evaluate(auct.bid / maxBid) *
										FrenzyProbabilityCurve.Evaluate(frenzy) *
										LikeProbabilityCurve.Evaluate(10f);


					if(Random.Range(0f,1f) < probability){
						bidInstance = GameObject.Instantiate(bidPrefab) as GameObject;
						bidInstance.transform.parent = this.transform;
						bidInstance.transform.localPosition = new Vector3(0,0,0);
						BidScript bs = bidInstance.GetComponent<BidScript>();
						if(bs!=null){
							bs.bidCuantity = auct.bid;
							bs.bidParent = this;
						}
					}					

					actualTime = timePerTry;
				}else
					actualTime -= Time.deltaTime;
			}


		}
	}

	public void TakeBid(BidScript bid){
		if(bid == bidInstance)
			frenzy+=frenzyIncrementPerOwnBid;
		else
			frenzy+=frenzyIncrementPerOtherBid;

		if(frenzy >= 1f)
			frenzy = 1f;
	}

	public void PassBid(BidScript bid){
		this.enabled = false;
		this.gameObject.GetComponent<PeopleScript>().moveTo(GameObject.FindObjectOfType<AuctionArea>().doorPosition.position);
		this.gameObject.AddComponent<AutoRemoverScript>();

	}
}
