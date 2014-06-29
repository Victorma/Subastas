using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuctioningScript : MonoBehaviour {

	public enum AuctionState {
		Starting, Auctioning, Ending
	}

	List<PeopleScript> pujadores;

	public AuctionState state;

	public float bid = 150;
	public float price = 0;
	public float timeSinceLastBid = 0;
	public float timeToStartCountDown = 5;

	private IntroScript intro;

	public PujadorScript ganadorActual;
	public float Frenzy {
		get {
			float frenzy = 0;
			PujadorScript[] pujadores = GameObject.FindObjectsOfType<PujadorScript>();
			foreach(PujadorScript p in pujadores)
				frenzy+= p.frenzy;

			return frenzy / (float) pujadores.Length;

		}
	}

	IEnumerator LaunchPeople(List<PeopleScript> people, List<GameObject> chairs){

		intro.DoIntroSecuence(3);

		int avoidance = 20;
		List<PujadorScript> pujadores = new List<PujadorScript>();
		while(chairs.Count != 0 && people.Count != 0){
			int person = Random.Range(0, people.Count);
			int chair = Random.Range(0, chairs.Count);

			if(!people[person].GetComponent<PujadorScript>().selected)
				people.RemoveAt (person);
			else{
				people[person].transform.position = GameObject.FindObjectOfType<AuctionArea>().doorPosition.position;
				people[person].GetComponent<NavMeshAgent>().avoidancePriority = avoidance;
				avoidance++;

				PujadorScript pujador = people[person].GetComponent<PujadorScript>();
				pujador.silla = chairs[chair].transform;
				//people[person].moveTo(chairs[chair].transform.TransformPoint(chairs[chair].GetComponent<AreaProvider>().center));
				people[person].gameObject.SetActive(true);
				pujador.enabled = true;
				pujadores.Add(pujador);

				people.RemoveAt (person);
				chairs.RemoveAt (chair);
				
				yield return new WaitForSeconds(0.2f);
			}
		}

		while(!intro.introFirstPartFinished)
			yield return new WaitForSeconds(0.1f);

		foreach(PujadorScript p in pujadores)
			p.state = PujadorScript.PujadorState.Normal;

		this.state = AuctionState.Auctioning;
		
	}

	public void TakeBid(BidScript bs){
		ganadorActual = bs.bidParent;
		price += bs.bidCuantity;
		timeSinceLastBid = 0;
	}

	// Use this for initialization
	void Start () {
		this.state = AuctionState.Starting;
		this.intro = GameObject.FindObjectOfType<IntroScript>();
		pujadores =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());
		foreach(PeopleScript p in pujadores)
			p.gameObject.SetActive(false);
		List<GameObject> sillas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Chair"));

		StartCoroutine(LaunchPeople(new List<PeopleScript>(pujadores), sillas));
	
	}
	private bool ended = false;
	// Update is called once per frame
	void Update () {
	
		switch(state){
		case AuctionState.Auctioning:{
			timeSinceLastBid+=Time.deltaTime;
			if(!intro.isDoingOutro && timeSinceLastBid > timeToStartCountDown){
				GameObject.FindObjectOfType<IntroScript>().DoOutroSecuence(3f);
			}else if(intro.isDoingOutro && timeSinceLastBid < timeToStartCountDown){
				intro.stop();
			}

			if(timeSinceLastBid > timeToStartCountDown + 3f){
				this.state = AuctionState.Ending;
			}
		}break;

		case AuctionState.Ending:{
			if(!ended){
				AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();
				foreach(AudioSource sound in sounds)
					sound.Stop();

				foreach(PujadorScript p in GameObject.FindObjectsOfType<PujadorScript>())
					if(p == ganadorActual) p.state = PujadorScript.PujadorState.Waiting;
					else p.state = PujadorScript.PujadorState.Leaving;


				ended = true;
			}
		}break;
		}

	}

	public void End(){
		StartCoroutine(EndC());
	}

	IEnumerator EndC(){
		List<PeopleScript> toRem = new List<PeopleScript>();

		for(int i = pujadores.Count-1; i>=0; i--){
			if(pujadores[i] == null || pujadores[i].GetComponent<PujadorScript>().silla != null){
				if(pujadores[i] != null)
					GameObject.DestroyImmediate(pujadores[i].gameObject);
				pujadores.RemoveAt(i);
			}
		}


		AutoFade.LoadLevel("Home", 0.5f,0.3f, Color.black);
		yield return new WaitForSeconds(0.51f);

		HomeArea ha = GameObject.FindObjectOfType<HomeArea>();
		AreaProvider hap = ha.GetComponentInChildren<AreaProvider>();
		Vector3 corner1 = hap.transform.TransformPoint(hap.start);
		Vector3 corner2 = hap.transform.TransformPoint(hap.end);
		Rect area = new Rect(corner1.x, corner1.z, corner2.x - corner1.x, corner2.z - corner1.z);

		foreach(PeopleScript p in pujadores){
			//person.transform.position = ha.FrontDoor.transform.position;
			
			Vector3 nuevoPunto = new Vector3(area.x + area.width*Random.Range(0f,1f), p.transform.position.y, area.y + area.height*Random.Range(0f,1f));
			p.transform.position = nuevoPunto;
			p.GetComponent<AreaRandomMover>().enabled = true;
			p.GetComponent<AreaRandomMover>().areaProvider = hap;
			p.GetComponent<PujadorScript>().enabled = false;
			p.gameObject.SetActive(true);

			//Vector3 point = ha.transform.TransformPoint(hap.start) + (ha.transform.TransformPoint(hap.end)-ha.transform.TransformPoint(hap.start))*Random.Range(0f,1f);

			//person.GetComponent<PeopleScript>().moveTo(nuevoPunto);
			//person.GetComponent<AreaRandomMover>().areaProvider = hap;
			
			//yield return new WaitForSeconds(0.2f);
		}
		Auction.currentAuction.items[Auction.currentAuction.currentSlot] = null;
		Auction.currentAuction.state = Auction.AuctionState.Selecting;

	}
}
