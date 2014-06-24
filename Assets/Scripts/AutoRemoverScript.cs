using UnityEngine;
using System.Collections;

public class AutoRemoverScript : MonoBehaviour {
	
	private PeopleScript people;

	// Use this for initialization
	void Start () {
		people = GetComponent<PeopleScript>();
	}


	void Update () {
		if (!people.Moving) {
			Debug.Log("parado");
			people.gameObject.SetActive(false);
			GameObject.DestroyImmediate(this);
		}
	}
}
