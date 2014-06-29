using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NothingScript : MonoBehaviour {

	public float StayTime = 120;
	public float PeoplePerMinute = 5;
	public int rareLevel = 0;
	public Dictionary<PeopleScript, float> customers;

	private List<PeopleScript> peopleLeft;

	private Item[] commonItems;
	private Item[] uncommonItems;
	private Item[] rareItems;
	private Item[] epicItems;
	private Item[] legendaryItems;

	private ItemScorer scorer;
	private HomeArea harea;

	void Start () {

		StayTime = 120;
		PeoplePerMinute = 5;
		rareLevel = 0;

		customers = new Dictionary<PeopleScript, float>();
		harea = FindObjectOfType<HomeArea>();

		peopleLeft = new List<PeopleScript>();

		commonItems = Resources.LoadAll<Item>("items/objects/common");
		uncommonItems = Resources.LoadAll<Item>("items/objects/uncommon");
		rareItems = Resources.LoadAll<Item>("items/objects/rare");
		epicItems = Resources.LoadAll<Item>("items/objects/epic");
		legendaryItems = Resources.LoadAll<Item>("items/objects/legendary");

		scorer = Resources.Load<ItemScorer>("ItemScorer");

		GameObject.FindObjectOfType<NothingGUI>().setController(this);
	
	}

	void Update () {

		float PPS = PeoplePerMinute / 60f;
		float realProb = PPS * Time.deltaTime;
		if(realProb > Random.Range(0f,1f)){
			float cP = 0, uP = 0, rP = 0, eP = 0, lP = 0;

			switch(rareLevel){
			case 0: cP = 1f/2f; uP = cP/2f; rP = uP/2f; eP = rP/2f; lP = eP/2f; break;
			case 1: uP = 1f/2f; rP = cP = uP/2f; eP = rP/2f; lP = eP/2f; break;
			case 2: rP = 1f/2f; eP = uP = rP/2f; lP = cP = eP/2f; break;
			case 3: eP = 1f/2f; rP = lP = eP/2f; uP = rP/2f; cP = uP/2f; break;
			case 4: lP = 1f/2f; eP = lP/2f; rP = eP/2f; uP = rP/2f; cP = uP/2f; break;
			}

			int chosen = PubliScript.chooseRandom(new float[]{cP,uP,rP,eP,lP});

			Item[] font = null;
			switch(chosen){
			case 0: font = commonItems; break;
			case 1: font = uncommonItems; break;
			case 2: font = rareItems; break;
			case 3: font = epicItems; break;
			case 4: font = legendaryItems; break;
			}

			if(font.Length > 0){

			Item i = font[Random.Range(0, font.Length)];
				GameObject[] people = scorer.getOrderedPeople(i);
				GameObject person = people[people.Length-1];

				GameObject instance = GameObject.Instantiate(person) as GameObject;
				instance.transform.position = harea.FrontDoor.position;
				instance.GetComponent<PeopleScript>().item = i;
				instance.GetComponent<AreaRandomMover>().areaProvider = harea.GetComponentInChildren<AreaProvider>();

				customers.Add(instance.GetComponent<PeopleScript>(), Time.time + StayTime*(1f+Random.Range(-0.1f,0.1f))); // Un 10% de variacion
			}
		}

		foreach(PeopleScript ps in customers.Keys){
			if(customers[ps] < Time.time){
				ps.moveTo(harea.FrontDoor.position);
				ps.GetComponent<AreaRandomMover>().enabled = false;
				ps.gameObject.AddComponent<AutoRemoverScript>();
				peopleLeft.Add(ps);
			}
		}

		for(int i = peopleLeft.Count-1; i>= 0; i--){
			if(customers.ContainsKey(peopleLeft[i]))
				customers.Remove(peopleLeft[i]);
			if(!peopleLeft[i].gameObject.activeSelf){
				GameObject.DestroyImmediate(peopleLeft[i].gameObject);
				peopleLeft.RemoveAt(i);
			}
		}

	}

	public void Buy(){
		StartCoroutine(BuyC());
	}

	IEnumerator BuyC(){
		
		for(int i = peopleLeft.Count-1; i>= 0; i--){
			if(customers.ContainsKey(peopleLeft[i]))
				customers.Remove(peopleLeft[i]);
			
			GameObject.DestroyImmediate(peopleLeft[i].gameObject);
			peopleLeft.RemoveAt(i);
		}
		foreach(PeopleScript ps in customers.Keys){
			ps.GetComponent<AreaRandomMover>().enabled = false;
			DontDestroyOnLoad(ps.gameObject);
		}

		AutoFade.LoadLevel("BuyScene", 0.5f, 0.2f, Color.black);
		yield return new WaitForSeconds(0.51f);

		Auction.currentAuction.state = Auction.AuctionState.Buying;

	}
}
