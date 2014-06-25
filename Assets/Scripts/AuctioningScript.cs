using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuctioningScript : MonoBehaviour {

	List<PeopleScript> pujadores;
	public float bid = 150;
	public float price = 0;
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
		
		int avoidance = 20;
		while(chairs.Count != 0 && people.Count != 0){
			int person = Random.Range(0, people.Count);
			int chair = Random.Range(0, chairs.Count);

			if(!people[person].GetComponent<PujadorScript>().enabled)
				people.RemoveAt (person);
			else{
				people[person].transform.position = GameObject.FindObjectOfType<AuctionArea>().doorPosition.position;
				people[person].GetComponent<NavMeshAgent>().avoidancePriority = avoidance;
				avoidance++;
				
				people[person].moveTo(chairs[chair].transform.TransformPoint(chairs[chair].GetComponent<AreaProvider>().center));
				people[person].gameObject.SetActive(true);

				people.RemoveAt (person);
				chairs.RemoveAt (chair);
				
				yield return new WaitForSeconds(0.2f);
			}
		}

		GameObject.FindObjectOfType<IntroScript>().DoIntroSecuence(3);

		yield return new WaitForSeconds(6f);

		GameObject.FindObjectOfType<CurrentMaxBid>().Show();

		yield return new WaitForSeconds(0.3f);

		GameObject.FindObjectOfType<BidButtons>().Show();

		yield return new WaitForSeconds(0.3f);

		GameObject.FindObjectOfType<FrenzyRect>().Show();
		
	}

	// Use this for initialization
	void Start () {

		pujadores =  new List<PeopleScript>(GameObject.FindObjectsOfType<PeopleScript>());
		foreach(PeopleScript p in pujadores)
			p.gameObject.SetActive(false);
		List<GameObject> sillas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Chair"));
		
		StartCoroutine(LaunchPeople(new List<PeopleScript>(pujadores), sillas));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
