using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NothingScript : MonoBehaviour {

	public float StayTime = 120;
	public float PeoplePerMinute = 3;
	public int rareLevel = 0;
	public Dictionary<PeopleScript, float> customers;

	private Item[] commonItems;
	private Item[] uncommonItems;
	private Item[] rareItems;
	private Item[] epicItems;
	private Item[] legendaryItems;

	private ItemScorer scorer;
	private HomeArea harea;

	void Start () {

		harea = FindObjectOfType<HomeArea>();

		commonItems = Resources.LoadAll<Item>("items/common");
		uncommonItems = Resources.LoadAll<Item>("items/uncommon");
		rareItems = Resources.LoadAll<Item>("items/rare");
		epicItems = Resources.LoadAll<Item>("items/epic");
		legendaryItems = Resources.LoadAll<Item>("items/legendary");

		scorer = Resources.Load<ItemScorer>("ItemScorer");
	
	}

	void Update () {

		float PPS = PeoplePerMinute / 60f;
		float realProb = PPS * Time.deltaTime;
		if(realProb < Random.Range(0f,1f)){
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

			Item i = font[Random.Range(0, font.Length)];
			GameObject[] people = scorer.getOrderedPeople(i);
			GameObject person = people[people.Length-1];

			GameObject instance = GameObject.Instantiate(person);
			instance.transform.position = harea.FrontDoor.position;
			instance.GetComponent<PeopleScript>().item = i;
			instance.GetComponent<AreaRandomMover>().areaProvider = harea.GetComponentInChildren<AreaProvider>();

			customers.Add(instance, Time.time + StayTime*(1f+Random.Range(-0.1f,0,1f))); // Un 10% de variacion
		}

		foreach(PeopleScript ps in customers.Keys){
			if(customers[ps] > Time.time){
				ps.moveTo(harea.FrontDoor.position);
				ps.gameObject.AddComponent<AutoRemoverScript>();

			}
		}

	}
}
