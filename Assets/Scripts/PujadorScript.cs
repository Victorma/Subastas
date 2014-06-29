using UnityEngine;
using System.Collections;

public class PujadorScript : MonoBehaviour {
	
	private bool isBidding;

	public GameObject bidPrefab;
	public GameObject bidInstance;
	public Transform silla;

	public float frenzy;
	public float money;
	public float maxBid;

	public float frenzyDecrementPerSecond;
	public float frenzyIncrementPerOwnBid;
	public float frenzyIncrementPerOtherBid;

	private PeopleScript people;
	private ItemScorer scorer;

	private Animator animator;

	// Use this for initialization
	void Start () {
		people = GetComponent<PeopleScript>();
		animator = GetComponent<Animator> ();
		scorer = Resources.Load<ItemScorer>("ItemScorer");
		if (animator == null)
			animator = GetComponentInChildren<Animator> ();
	}

	private float timePerTry = 0.2f;
	private float actualTime = 0;
	private float timeFighting = 0;
	public float timeToDie = 5;

	public AnimationCurve FrenzyPerOwnBid;
	public AnimationCurve FrenzyPerOtherBid;

	public AnimationCurve PriceProbabilityCurve;
	public AnimationCurve BidProbabilityCurve;
	public AnimationCurve FrenzyProbabilityCurve;
	public AnimationCurve LikeProbabilityCurve;

	public float extraProbability = 1f;

	public enum PujadorState {
		Waiting, Normal, Fighting, Leaving
	}

	public PujadorState state;
	public PujadorScript opponent;
	public bool iniciator;

	public GameObject fightPrefab;
	public GameObject fightInstance;

	public bool selected;

	// Update is called once per frame
	void Update () {
		if(this.enabled){
			switch(state){
			case PujadorState.Waiting:{
				if(!people.Moving)
					animator.Play("south");

				people.moveTo(silla.position);

			}break;
			case PujadorState.Normal:{
				if(!people.Moving)
					animator.Play("south");

				people.moveTo(silla.position);

				AuctioningScript auct = Auction.currentAuction.GetComponent<AuctioningScript>();
				if(auct == null)
					return;

				if(auct.state == AuctioningScript.AuctionState.Ending){
					if(this.bidInstance != null)
						GameObject.DestroyImmediate(bidInstance);

					if(auct.ganadorActual == this)
						this.state = PujadorState.Waiting;
					else
						this.state = PujadorState.Leaving;
				}

				if(!people.Moving){
					
					if(auct.ganadorActual != this)
						frenzy -= frenzyDecrementPerSecond*Time.deltaTime;
					
					if(bidInstance != null || auct.ganadorActual == this)
						return;
					
					if(actualTime <= 0){
						
						float probability = extraProbability * 
							PriceProbabilityCurve.Evaluate(auct.price / money) *
							BidProbabilityCurve.Evaluate(auct.bid / maxBid) *
							FrenzyProbabilityCurve.Evaluate(frenzy) *
							LikeProbabilityCurve.Evaluate(scorer.getItemScore(people, Auction.currentAuction.items[Auction.currentAuction.currentSlot]));
						
						
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
			}break;

			case PujadorState.Fighting:{
				if(bidInstance != null)
					GameObject.DestroyImmediate(bidInstance);

				if(timeFighting > timeToDie){
					if(fightInstance != null)
						GameObject.DestroyImmediate(fightInstance);

					Animator a = GetComponentInChildren<Animator>();
					a.enabled = false;

					timeFighting+=Time.deltaTime;
					bool on = (Mathf.RoundToInt(timeFighting*20)%2) == 0;
					SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
					Color color = sr.color;
					color.a = (on)?255:0;
					sr.color = color;

					if(timeFighting > timeToDie+1.5f ){
						GameObject.Destroy(this.gameObject);
					}	

				}else if((transform.position - opponent.transform.position).magnitude < 0.8f){
					timeFighting+=Time.deltaTime;
					if(this.iniciator) {
						if(fightInstance == null)
							fightInstance = GameObject.Instantiate(fightPrefab) as GameObject;
						if(fightInstance != null)
							fightInstance.transform.position = (transform.position + opponent.transform.position)/2f;
					}

					this.people.unfollow();
				}else if(!people.Moving){
					people.follow(opponent.transform,0.6f);
				}
			}break;

			case PujadorState.Leaving:{

				this.people.moveTo(GameObject.FindObjectOfType<AuctionArea>().doorPosition.position);
				if(!destroyer){
					this.gameObject.AddComponent<AutoRemoverScript>();
					destroyer = false;
				}
			}break;
			}
		}
	}
	private bool destroyer = false;

	public void TakeBid(BidScript bid){
		if(bid == bidInstance)
			frenzy+=FrenzyPerOwnBid.Evaluate(frenzy);
		else
			frenzy+=FrenzyPerOtherBid.Evaluate(frenzy);

		if(frenzy >= 1f){
			frenzy = 1f;
			PujadorScript enemy = bid.bidParent;
			if(enemy != this && this.state != PujadorState.Fighting && enemy.state != PujadorState.Fighting){
				this.state = enemy.state = PujadorState.Fighting;

				enemy.opponent = this;
				enemy.iniciator = false;
				this.opponent = enemy;
				this.iniciator = true;
			}
		}
	}

	public void PassBid(BidScript bid){
		this.state = PujadorState.Leaving;

	}
}
