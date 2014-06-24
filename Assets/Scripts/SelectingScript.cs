using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectingScript : MonoBehaviour {

	private bool seleccionando;
	List<PeopleScript> seleccionados;

	IEnumerator SelectPeople(List<PeopleScript> people, Item currentItem){
		seleccionados = new List<PeopleScript> ();

		while( people.Count != 0){
			int person = Random.Range(0, people.Count);
			int gusto = Random.Range(0, 10);

			if(gusto>=5){
				people[person].moveTo(GameObject.FindObjectOfType<HomeArea>().AuctionDoor.position);
				people[person].gameObject.AddComponent("AutoRemoverScript");
				seleccionados.Add(people[person]);
				yield return new WaitForSeconds(1f);
			}

			people.RemoveAt (person);
		}
		this.seleccionando = false;
	}

	void Start () {
		List<PeopleScript> pujadores =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());

		foreach (PeopleScript p in pujadores) {
			p.gameObject.SetActive (true);
			if(p.GetComponent<AreaRandomMover>()!=null)
				p.GetComponent<AreaRandomMover>().enabled = false;
		}

		this.seleccionando = true;
		StartCoroutine(SelectPeople(pujadores, new Item()));
	}

	void Update () {
		if (!seleccionando) {
			bool terminado = true;
			foreach(PeopleScript p in seleccionados){
				if(p.gameObject.activeSelf){
					terminado=false;
					break;
				}
			}
			if(terminado){
				List<PeopleScript> total =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());
				
				foreach(PeopleScript p in total){
					DontDestroyOnLoad(p.gameObject);
				}
				
				foreach(PeopleScript p in seleccionados){
					DontDestroyOnLoad(p.gameObject);
					p.gameObject.SetActive(true);
					p.GetComponent<PujadorScript>().enabled = true;
				}
				StartCoroutine(changeState());
			}
		}
	}
	IEnumerator changeState(){
		AutoFade.LoadLevel("AuctionScene", 0.2f,0.5f, Color.black);
		yield return new WaitForSeconds(0.25f);
		GameObject.FindObjectOfType<Auction>().State = Auction.AuctionState.Auctioning;
	}
}
