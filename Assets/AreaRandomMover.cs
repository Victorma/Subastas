using UnityEngine;
using System.Collections;

public class AreaRandomMover : MonoBehaviour {

	public AreaProvider areaProvider;
	public float minTimeBetweenMovements;
	public float randomVariation;

	private float timeToMove;
	private PeopleScript peopleScript;

	private float generateTime(){
		return minTimeBetweenMovements + randomVariation*Random.Range(0f,1f);
	}

	// Use this for initialization
	void Start () {
		timeToMove = generateTime();
		peopleScript = GetComponent<PeopleScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(areaProvider == null || peopleScript == null)
			return;


		timeToMove -= Time.deltaTime;
		if(timeToMove<0){

			int horizontal = Random.Range(0,2);

			Vector3 nuevoPunto = new Vector3((areaProvider.area.x + areaProvider.area.width*Random.Range(0f,1f))*horizontal + transform.position.x * (1-horizontal),
			                                 (areaProvider.area.y + areaProvider.area.height*Random.Range(0f,1f))*(1-horizontal) + transform.position.y * (horizontal),
			                                 transform.position.z);

			peopleScript.moveTo(nuevoPunto);
			timeToMove = generateTime();
		}
	}
}
