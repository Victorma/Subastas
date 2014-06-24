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

		if(areaProvider!=null){
			int people = Random.Range(minStartPeople, maxStartPeople);	
			for(int i = 0; i< people; i++){
				int person = Random.Range(0, prefabs.Count);
				GameObject newGo = GameObject.Instantiate(prefabs[person])as GameObject;

				float x = 0;//Random.Range(areaProvider.area.x, areaProvider.area.x + areaProvider.area.width);
				float y = 0;//Random.Range(areaProvider.area.y, areaProvider.area.y + areaProvider.area.height);

				newGo.transform.position = new Vector3(x,y,areaProvider.transform.position.z);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
