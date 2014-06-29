using UnityEngine;
using System.Collections.Generic;

public class WalkingGuysCreator : MonoBehaviour {

	public List<GameObject> prefabs;
	public int minStartPeople;
	public int maxStartPeople;
	public float spawingProb;

	private AreaProvider areaProvider;
	// Use this for initialization
	void Start () {
		areaProvider = GetComponent<AreaProvider>();

		Vector3 corner1 = areaProvider.transform.TransformPoint(areaProvider.start);
		Vector3 corner2 = areaProvider.transform.TransformPoint(areaProvider.end);
		Rect area = new Rect(corner1.x, corner1.z, corner2.x - corner1.x, corner2.z - corner1.z);

		if(areaProvider!=null){
			int people = Random.Range(minStartPeople, maxStartPeople);	
			for(int i = 0; i< people; i++){
				int person = Random.Range(0, prefabs.Count);
				GameObject newGo = GameObject.Instantiate(prefabs[person])as GameObject;

				newGo.transform.position = new Vector3(area.x + area.width*Random.Range(0f,1f), 1, area.y + area.height*Random.Range(0f,1f));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
