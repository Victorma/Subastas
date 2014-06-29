using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PubliScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		PubliGUI publi = GameObject.FindObjectOfType<PubliGUI>();
		publi.enabled = true;
		publi.setController(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static int chooseRandom(params float[] posibilities){

		float total = 0;
		foreach(float f in posibilities)
			total+=f;

		float chosen = Random.Range(0, total);
		int p = 0;
		while(chosen > posibilities[p]){
			chosen-=posibilities[p];
			p++;
		}

		return p;
	}

	public void SelectPubli(Dictionary<PubliTypes.PubliType, float> distribution, float money){
		StartCoroutine(CreatePeople(distribution, money));		
	}

	IEnumerator CreatePeople(Dictionary<PubliTypes.PubliType, float> distribution, float money){
		//TODO gente random por categoria

		AutoFade.LoadLevel("Home", 0.2f,0.5f, Color.black);
		yield return new WaitForSeconds(0.25f);

		int people = Mathf.FloorToInt(Mathf.Sqrt(money/5)/Mathf.Sqrt(2));
		
		float hombre=0, mujer=0, joven=0, adulto=0, anciano=0, prole=0, media=0, burgues=0;
		
		foreach(PubliTypes.PubliType t in distribution.Keys){
			float v = distribution[t];
			
			hombre+= v*t.Hombre;
			mujer+= v*t.Mujer;
			joven+= v*t.Joven;
			adulto+= v*t.Adulto;
			anciano+= v*t.Anciano;
			prole+= v*t.Prole;
			media+= v*t.Media;
			burgues+= v*t.Burgues;
			
		}
		
		HomeArea ha = GameObject.FindObjectOfType<HomeArea>();
		AreaProvider hap = ha.GetComponentInChildren<AreaProvider>();
		Vector3 corner1 = hap.transform.TransformPoint(hap.start);
		Vector3 corner2 = hap.transform.TransformPoint(hap.end);
		
		Rect area = new Rect(corner1.x, corner1.z, corner2.x - corner1.x, corner2.z - corner1.z);
		
		for(int i = 0; i < people; i++){
			
			int hm = chooseRandom(hombre, mujer);
			int jaan = chooseRandom(joven,adulto, anciano);
			int pmb = chooseRandom(prole, media, burgues);
			int selected = hm*100+jaan*10+pmb;
			
			PeopleTypes pt = Resources.Load<PeopleTypes>("PeopleTypes");
			
			PeopleScript.Genre genre = PeopleScript.Genre.Male;
			switch(hm){
			case 0: genre = PeopleScript.Genre.Male; break;
			case 1: genre = PeopleScript.Genre.Female; break;
			}
			
			PeopleScript.Age age = PeopleScript.Age.Young;
			switch(jaan){
			case 0: age = PeopleScript.Age.Young; break;
			case 1: age = PeopleScript.Age.Adult; break;
			case 2: age = PeopleScript.Age.Old; break;
			}
			
			PeopleScript.SocialClass socialClass = PeopleScript.SocialClass.Low;
			switch(pmb){
			case 0: socialClass = PeopleScript.SocialClass.Low; break;
			case 1: socialClass = PeopleScript.SocialClass.Middle; break;
			case 2: socialClass = PeopleScript.SocialClass.Gentry; break;
			}
			
			GameObject person = pt.createFrom(age, genre, socialClass);
			person.transform.position = ha.FrontDoor.transform.position;

			Vector3 nuevoPunto = new Vector3(area.x + area.width*Random.Range(0f,1f), person.transform.position.y, area.y + area.height*Random.Range(0f,1f));

			//Vector3 point = ha.transform.TransformPoint(hap.start) + (ha.transform.TransformPoint(hap.end)-ha.transform.TransformPoint(hap.start))*Random.Range(0f,1f);
			
			person.GetComponent<PeopleScript>().moveTo(nuevoPunto);
			person.GetComponent<AreaRandomMover>().areaProvider = hap;

			yield return new WaitForSeconds(0.2f);
		}

		Auction.currentAuction.state = Auction.AuctionState.Selecting;
	}
}

/*switch(selected){
			case 000: //hombre joven prole 
				Debug.Log ("Creado: hombre joven prole ");
				break;
			case 001: //hombre joven media 
				Debug.Log ("Creado: hombre joven media ");
				break;
			case 002: //hombre joven burgues 
				Debug.Log ("Creado: hombre joven burgues ");
				break;
			case 010: //hombre adulto prole 
				Debug.Log ("Creado: hombre adulto prole ");
				break;
			case 011: //hombre adulto media 
				Debug.Log ("Creado: hombre adulto media ");
				break;
			case 012: //hombre adulto burgues 
				Debug.Log ("Creado: hombre adulto burgues ");
				break;
			case 020: //hombre anciano prole 
				Debug.Log ("Creado: hombre anciano prole ");
				break;
			case 021: //hombre anciano media 
				Debug.Log ("Creado: hombre anciano media ");
				break;
			case 022: //hombre anciano burgues 
				Debug.Log ("Creado: hombre anciano burgues ");
				break;
			case 100: //mujer joven prole 
				Debug.Log ("Creado: mujer joven prole ");
				break;
			case 101: //mujer joven media 
				Debug.Log ("Creado: mujer joven media ");
				break;
			case 102: //mujer joven burgues 
				Debug.Log ("Creado: mujer joven burgues ");
				break;
			case 110: //mujer adulto prole 
				Debug.Log ("Creado: mujer adulto prole ");
				break;
			case 111: //mujer adulto media 
				Debug.Log ("Creado: mujer adulto media ");
				break;
			case 112: //mujer adulto burgues 
				Debug.Log ("Creado: mujer adulto burgues ");
				break;
			case 120: //mujer anciano prole 
				Debug.Log ("Creado: mujer anciano prole ");
				break;
			case 121: //mujer anciano media 
				Debug.Log ("Creado: mujer anciano media ");
				break;
			case 122: //mujer anciano burgues 
				Debug.Log ("Creado: mujer anciano burgues ");
				break;
			}*/
