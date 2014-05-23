using UnityEngine;
using System.Collections.Generic;

public class ItemAuctionManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

		List<PeopleScript> pujadores =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());
		List<GameObject> sillas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Chair"));

		while(sillas.Count != 0 && pujadores.Count != 0){
			int pujador = Random.Range(0, pujadores.Count);
			int silla = Random.Range(0, sillas.Count);

			pujadores[pujador].transform.position = new Vector3(0,0,-1);
			pujadores[pujador].moveTo(sillas[silla].transform.TransformPoint(sillas[silla].GetComponent<AreaProvider>().area.center));

			pujadores.RemoveAt (pujador);
			sillas.RemoveAt(silla);
		}

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
