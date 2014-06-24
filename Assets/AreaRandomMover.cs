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

			Vector3 corner1 = areaProvider.transform.TransformPoint(areaProvider.start);
			Vector3 corner2 = areaProvider.transform.TransformPoint(areaProvider.end);

			Rect area = new Rect(corner1.x, corner1.z, corner2.x - corner1.x, corner2.z - corner1.z);

			Vector3 nuevoPunto = new Vector3((area.x + area.width*Random.Range(0f,1f))*horizontal + transform.position.x * (1-horizontal),
			                                 transform.position.y,
			                                 (area.y + area.height*Random.Range(0f,1f))*(1-horizontal) + transform.position.z * (horizontal));

			peopleScript.moveTo(nuevoPunto);
			timeToMove = generateTime();
		}
	}
}
