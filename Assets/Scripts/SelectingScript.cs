using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectingScript : MonoBehaviour {

	public Texture defaultvalue;

	IEnumerator SelectPeople(Item currentItem){

		List<PeopleScript> people =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());

		foreach (PeopleScript p in people) {
			p.gameObject.SetActive (true);
			if(p.GetComponent<AreaRandomMover>()!=null)
				p.GetComponent<AreaRandomMover>().enabled = false;
		}

		List<PeopleScript> seleccionados = new List<PeopleScript> ();

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
			p.GetComponent<PujadorScript>().enabled = true;
		}

		AutoFade.LoadLevel("AuctionScene", 0.2f,0.5f, Color.black);
		yield return new WaitForSeconds(0.25f);

		GameObject.FindObjectOfType<Auction>().state = Auction.AuctionState.Auctioning;
	}

	private ItemSelectionGUI selectionGUI;

	void Start () {
		HomeArea h = FindObjectOfType<HomeArea>();
		selectionGUI = h.Menu.GetComponent<ItemSelectionGUI>();
		selectionGUI.enabled = true;
		selectionGUI.setController(this);
	}

	void Update () {

	}

	public void itemDropped(Item item){
		StartCoroutine(SelectPeople(item));
	}

}
