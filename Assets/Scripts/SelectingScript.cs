using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectingScript : MonoBehaviour {

	public Texture defaultvalue;

	private class CercaniaTransform : IComparer<PeopleScript> {
		private Transform to;
		public CercaniaTransform(Transform to){
			this.to = to;
		}
		public int Compare(PeopleScript p1, PeopleScript p2){
			return Mathf.RoundToInt((p1.transform.position - to.position).magnitude - (p2.transform.position - to.position).magnitude);
		}

	}

	public string auctionScene = "AuctionScene";

	IEnumerator SelectPeople(int i){

		Item item = Auction.currentAuction.items[i];

		List<PeopleScript> people =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());

		foreach (PeopleScript p in people) {
			p.gameObject.SetActive (true);
			if(p.GetComponent<AreaRandomMover>()!=null)
				p.GetComponent<AreaRandomMover>().enabled = false;
		}

		List<PeopleScript> seleccionados = new List<PeopleScript> ();
		Transform door = GameObject.FindObjectOfType<HomeArea>().AuctionDoor;
		seleccionados.Sort(new CercaniaTransform(door));

		while( people.Count != 0){
			int person = Random.Range(0, people.Count);
			int gusto = Random.Range(0, 10);

			if(gusto>=5){
				people[person].moveTo(door.position);
				people[person].gameObject.AddComponent("AutoRemoverScript");
				seleccionados.Add(people[person]);
				yield return new WaitForSeconds(0.66f);
			}

			people.RemoveAt (person);
		}

		bool terminado = false;
		while(!terminado){
			terminado = true;
			foreach(PeopleScript p in seleccionados){
				if(p.gameObject.activeSelf){
					terminado=false;
					break;
				}
			}
			yield return new WaitForSeconds(0.1f);
		}

		List<PeopleScript> total =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());
		
		foreach(PeopleScript p in total)
			DontDestroyOnLoad(p.gameObject);
		
		foreach(PeopleScript p in seleccionados){
			DontDestroyOnLoad(p.gameObject);
			p.gameObject.SetActive(true);
			p.GetComponent<PujadorScript>().selected = true;
		}

		AutoFade.LoadLevel(auctionScene, 0.2f,0.5f, Color.black);
		yield return new WaitForSeconds(0.25f);

		GameObject.FindObjectOfType<Auction>().state = Auction.AuctionState.Auctioning;
	}

	private ItemSelectionGUI selectionGUI;

	void Start () {
		bool hasItems = false;
		foreach(Item i in Auction.currentAuction.items)
			if( i != null) hasItems = true;

		if(!hasItems){
			PeopleScript[] restantes = GameObject.FindObjectsOfType<PeopleScript>();
			foreach(PeopleScript ps in restantes)
				GameObject.DestroyImmediate(ps.gameObject);
			Auction.currentAuction.state = Auction.AuctionState.Nothing;
		}else{
			HomeArea h = FindObjectOfType<HomeArea>();
			selectionGUI = h.Menu.GetComponent<ItemSelectionGUI>();
			selectionGUI.enabled = true;
			selectionGUI.setController(this);
		}
	}

	void Update () {

	}

	public void itemDropped(int i){
		Auction.currentAuction.currentSlot = i;
		StartCoroutine(SelectPeople(i));
	}

}
