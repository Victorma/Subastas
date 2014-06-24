using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyingScript : MonoBehaviour {

	ItemDataGUI idg;
	ItemBottomMenu ibm;
	List<PeopleScript> vendedores;

	bool asignando = true;

	IEnumerator LaunchPeople(List<PeopleScript> people){
		
		int avoidance = 20;

		people [0].gameObject.SetActive (true);
		people [0].transform.position = GameObject.FindObjectOfType<BuyingArea> ().EnterDoor.position;
		people [0].moveTo (GameObject.FindObjectOfType<BuyingArea> ().TradePoint.position);

		yield return new WaitForSeconds(1f);

		for (int i=1; i<people.Count; i++) {
				people [i].gameObject.SetActive (true);
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
		StartCoroutine(nextItem());
	}

	IEnumerator nextItem(){
		idg.hide ();
		vendedores [0].moveTo (GameObject.FindObjectOfType<BuyingArea> ().ExitDoor.position);
		vendedores [0].gameObject.AddComponent("AutoRemoverScript");
		if (vendedores.Count > 1) {
				vendedores [1].unfollow ();
				yield return new WaitForSeconds (1f);
				vendedores [1].moveTo (GameObject.FindObjectOfType<BuyingArea> ().TradePoint.position);
		}
		vendedores.Remove (vendedores [0]);
		yield return new WaitForSeconds(0.1f);
	}
	
	void Update () {
		if(!asignando){
			if(vendedores.Count>0){
				if(vendedores[0].Moving==false){
					showItemGui(vendedores[0].item);
				}
			}
		}
	}
}
