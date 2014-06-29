using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemScorer : ScriptableObject {

	public List<ItemType> table = new List<ItemType>();

	void Start () {
		// Hombre, Mujer, Joven, Adulto, Viejo, Prole, Medio, Burgues
		
	}

	private class LikeSort: IComparer<PeopleScript>{
		private ItemScorer i;
		private Item item;
		public LikeSort(ItemScorer i, Item item){
			this.i = i;
			this.item = item;
		}
		public int Comparer(PeopleScript p1, PeopleScript p2){
			return Mathf.RoundToInt((i.getItemScore(p2,item) - i.getItemScore(p1, item))*10f);
		}
	}

	public GameObject[] getOrderedPeople(Item i){
		List<PeopleScript> liked = new List<PeopleScript>();

		PeopleTypes pt = Resources.Load<PeopleTypes>("PeopleTypes");
		foreach(GameObject go in pt.peopleTypes){
			PeopleScript ps = go.GetComponent<PeopleScript>();
			if(ps!=null)
				liked.Add(ps);
		}

		liked.Sort(new LikeSort(this, i));
		return liked.ToArray() as GameObject[];
	}

	public float getItemScore(PeopleScript p, Item i) {

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
