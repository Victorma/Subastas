using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyingScript : MonoBehaviour {

	ItemDataGUI idg;
	ItemBottomMenu ibm;
	List<PeopleScript> vendedores;
	PoliceScript police;

	bool asignando = true;
	bool deteniendo = false;

	public int SabioReadingLevel = 0;

	private enum DetentionPhase{
		Empezar, Capturar, SeguirPuerta, VolverPosicion
	}

	DetentionPhase dp;

	IEnumerator LaunchPeople(List<PeopleScript> people){

		police.transform.position = GameObject.FindObjectOfType<BuyingArea> ().PoliceEnter.position;
		police.moveTo (GameObject.FindObjectOfType<BuyingArea> ().PolicePosition.position);

		int avoidance = 20;
		people [0].item = Item.generateVariant (people [0].item);
		people [0].gameObject.SetActive (true);
		people [0].transform.position = GameObject.FindObjectOfType<BuyingArea> ().EnterDoor.position;
		people [0].moveTo (GameObject.FindObjectOfType<BuyingArea> ().TradePoint.position);

		yield return new WaitForSeconds(1f);

		for (int i=1; i<people.Count && i<7; i++) {
				people [i].gameObject.SetActive (true);
				people [i].item = Item.generateVariant (people [i].item);
				people [i].transform.position = GameObject.FindObjectOfType<BuyingArea> ().EnterDoor.position;
				people [i].GetComponent<NavMeshAgent> ().avoidancePriority = avoidance;

				people[i].follow(people[i-1].transform,0.6f);
			yield return new WaitForSeconds(1f);
		}

		asignando = false;
		yield return new WaitForSeconds(0.1f);
	}
	
	void Start () {
		vendedores = new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());

		idg = GameObject.FindObjectOfType<ItemDataGUI> ();
		ibm = GameObject.FindObjectOfType<ItemBottomMenu> ();
		police = GameObject.FindObjectOfType<PoliceScript> ();

		foreach (PeopleScript p in vendedores) {
			p.gameObject.SetActive (false);
		}

		StartCoroutine(LaunchPeople(vendedores));
	}

	void showItemGui(Item item){
		idg.item = item;
		ibm.showingitem = item;
		ibm.setController(this);
		idg.show ();
	}

	public void next(){
		if (!deteniendo)
			StartCoroutine (nextItem ());
		else {
			deteniendo=false;
			StartCoroutine (nextItemPostDetention ());
		}
	}

	public void detener(){

		dp = DetentionPhase.Empezar;
		deteniendo = true;
	}

	IEnumerator nextItem(){

		asignando = true;

			idg.hide ();
			ibm.showingitem=null; 

			vendedores [0].moveTo (GameObject.FindObjectOfType<BuyingArea> ().ExitDoor.position);
			vendedores [0].gameObject.AddComponent("AutoRemoverScript");
			if (vendedores.Count > 1) {
					vendedores [1].unfollow ();
					yield return new WaitForSeconds (1f);
					vendedores [1].moveTo (GameObject.FindObjectOfType<BuyingArea> ().TradePoint.position);

					if(vendedores.Count>7){
						yield return new WaitForSeconds (2f);
						vendedores [7].gameObject.SetActive (true);
						vendedores [7].item = Item.generateVariant (vendedores [7].item);
						vendedores [7].transform.position = GameObject.FindObjectOfType<BuyingArea> ().EnterDoor.position;
						vendedores [7].GetComponent<NavMeshAgent> ().avoidancePriority = 20;
								
					vendedores[7].follow(vendedores[6].transform,0.6f);
					}
			}
			vendedores.Remove (vendedores [0]);

		asignando = false;
		yield return new WaitForSeconds(0.1f);
	}

	IEnumerator nextItemPostDetention(){
		
		asignando = true;

		vendedores [0].moveTo (GameObject.FindObjectOfType<BuyingArea> ().TradePoint.position);
			
		if(vendedores.Count>6){
			yield return new WaitForSeconds (2f);
			vendedores [6].gameObject.SetActive (true);
			vendedores [6].item = Item.generateVariant (vendedores [6].item);
			vendedores [6].transform.position = GameObject.FindObjectOfType<BuyingArea> ().EnterDoor.position;
			vendedores [6].GetComponent<NavMeshAgent> ().avoidancePriority = 20;
			
			vendedores[6].follow(vendedores[5].transform,0.6f);
		}
		asignando = false;
		yield return new WaitForSeconds(0.1f);
	}

	IEnumerator exit(){
		asignando = true;

		List<Item> items = new List<Item>();
		foreach(Item itm in ibm.items){
			items.Add (itm);
		}
		Auction.currentAuction.items = items.ToArray();

		yield return new WaitForSeconds (0.5f);
		for(int i=vendedores.Count-1; i>=0; i--){
			vendedores [i].unfollow();
			vendedores [i].moveTo (GameObject.FindObjectOfType<BuyingArea> ().ExitDoor.position);
			vendedores [i].gameObject.AddComponent("AutoRemoverScript");
			yield return new WaitForSeconds (0.5f);
		}
		yield return new WaitForSeconds(3f);

		AutoFade.LoadLevel("Home", 0.2f,0.5f, Color.black);
		yield return new WaitForSeconds(0.25f);
		
		GameObject.FindObjectOfType<Auction>().state = Auction.AuctionState.Publi;

		yield return new WaitForSeconds(0.1f);
	}
	
	void Update () {


		if(!deteniendo){
			if(!asignando){
				if(ibm.items[ibm.items.Count-1]==null && vendedores.Count>0){
					if(vendedores[0].Moving==false && !idg.shown)
						showItemGui(vendedores[0].item);

				}else StartCoroutine (exit ());
			}
		}else{
			switch(dp){
			case DetentionPhase.Empezar:
				idg.hide ();
				ibm.showingitem=null; 
				police.moveTo (vendedores [0].transform.position);
				police.follow (vendedores [0].transform, 0.4f);
				dp = DetentionPhase.Capturar;
				break;
			case DetentionPhase.Capturar:
				if(!police.Moving){
					dp = DetentionPhase.SeguirPuerta;
					vendedores [1].unfollow();
					vendedores [0].moveTo (GameObject.FindObjectOfType<BuyingArea> ().ExitDoor.position);
				}
				break;
			case DetentionPhase.SeguirPuerta:
				if(!vendedores [0].Moving){
					dp = DetentionPhase.VolverPosicion;
					police.unfollow();
					police.moveTo(GameObject.FindObjectOfType<BuyingArea> ().ExitDoor.position);

					PeopleScript criminal = vendedores [0];
					vendedores.Remove (criminal);
					criminal.gameObject.SetActive(false);
					GameObject.DestroyImmediate(criminal);
				}
				break;
			case DetentionPhase.VolverPosicion:
				if(!police.Moving){
					police.transform.position = GameObject.FindObjectOfType<BuyingArea> ().PoliceEnter.position;
					police.moveTo(GameObject.FindObjectOfType<BuyingArea> ().PolicePosition.position);
					next ();
				}
				break;
			}

		}
	}
}
