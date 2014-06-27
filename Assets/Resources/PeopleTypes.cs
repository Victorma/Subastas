using UnityEngine;
using System.Collections.Generic;

public class PeopleTypes : ScriptableObject {

	public GameObject[] peopleTypes; 

	public GameObject createFrom(PeopleScript.Age age, PeopleScript.Genre genre, PeopleScript.SocialClass social){
		foreach(GameObject go in peopleTypes){

			PeopleScript p = go.GetComponent<PeopleScript>();

			if(p!=null && p.age == age && p.genre == genre && p.socialClass == social){
				return GameObject.Instantiate(go) as GameObject;
			}
		}
		return null;
	}

}
