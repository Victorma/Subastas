using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemScorer : ScriptableObject {

	public List<ItemType> table = new List<ItemType>();

	void Start () {
		// Hombre, Mujer, Joven, Adulto, Viejo, Prole, Medio, Burgues
		
	}

	public float getItemScore(PeopleScript p, ItemScript i) {

		float score = 0;
		foreach(ItemType item in this.table )
		{
			if(item.name == i.name) {
				//Genero
				switch(p.genre){
				case PeopleScript.Genre.Female:{
						score += item.mujer;
				}break;
				case PeopleScript.Genre.Male:{
						score += item.hombre;
				}break;
				}
				//Edad
				switch(p.age){
					case PeopleScript.Age.Young:{
						score += item.joven;
					}break;
					case PeopleScript.Age.Adult:{
						score += item.adulto;
					}break;
					case PeopleScript.Age.Old:{
						score += item.anciano;
					}break;
				}
				//Clase social
				switch(p.socialClass){
					case PeopleScript.SocialClass.Low: {
						score += item.prole;
					}break;
					case PeopleScript.SocialClass.Middle: {
						score += item.normal;
					}break;
					case PeopleScript.SocialClass.Gentry: {
						score += item.burgues;
					}break;
				}

			}//name fond
		}//for-each
		return score;
	}//Function

	public void addItemScore(ItemType i) {
		this.table.Add (i);
	}
}
