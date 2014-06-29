using UnityEngine;
using System.Collections.Generic;

public class SabioGUI : MonoBehaviour {

	private ItemBottomMenu ibm;

	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Vector2 size = new Vector2(464, 177);
	private Vector2 scale;
	public Vector2 Scale{ 
		get{ return scale; }
		set{ scale = value; }
	}

	public bool shown = false;

	public Item item;

	public Texture2D fondo, sombragente;

	public Texture2D unknown, common, uncommon, rare, epic, legendary;

	public GameObject[] gente;

	public int readingLevel;

	// Use this for initialization
	void Start () {
		scale = new Vector2(0.0001f, 0.0001f);

		fondo = Resources.Load<Texture2D> ("buyarea/sabio");
		sombragente = Resources.Load<Texture2D> ("buyarea/sombra");
		unknown = Resources.Load<Texture2D> ("buyarea/values/unk");
		common = Resources.Load<Texture2D> ("buyarea/values/common");
		uncommon = Resources.Load<Texture2D> ("buyarea/values/uncommon");
		rare = Resources.Load<Texture2D> ("buyarea/values/rare");
		epic = Resources.Load<Texture2D> ("buyarea/values/epic");
		legendary = Resources.Load<Texture2D> ("buyarea/values/legendary");

		gente = new GameObject[0];

		ibm = this.GetComponent<ItemBottomMenu> ();
	}
	
	// Update is called once per frame
	void OnGUI () {
		
		float rx = (Screen.width / (float)targetScreenSize.x)*scale.x;
		float ry = (Screen.height/ (float)targetScreenSize.y)*scale.y;

		Matrix4x4 bc = GUI.matrix;
		
		GUI.depth = 1;

		GUI.matrix = Matrix4x4.TRS (new Vector2(Screen.width/2f, Screen.height - ((ibm.size.y+150)*ibm.publicscale.y)), Quaternion.identity, new Vector3 (rx, ry, 1));

		GUILayout.BeginArea (new Rect (-size.x/2f, -size.y/2f, size.x, size.y));
		GUI.DrawTexture (new Rect (0, 0, size.x, size.y), fondo);

		int level = 0;
		switch (this.item.rareness) {
		case Item.RareType.Common: level=0; break;
		case Item.RareType.Uncommon: level=1; break;
		case Item.RareType.Rare: level=2; break;
		case Item.RareType.Epic: level=3; break;
		case Item.RareType.Legendary: level=4; break;
		}

		if(readingLevel>=level){
		GUIStyle stl = new GUIStyle(Resources.Load<GUISkin> ("pix2").GetStyle ("RealValue"));

		Vector2 textsize = stl.CalcSize (new GUIContent("Valor:\n" + item.sellingPrice.ToString ()));

		GUI.TextArea (new Rect (14, 40, textsize.x, textsize.y), "Valor:\n" + item.sellingPrice.ToString(), stl);

		Rect rarenes = new Rect (340, 20, 112, 112);

		switch (item.rareness) {
			case Item.RareType.Common:
				GUI.DrawTexture (rarenes, common);
				break;
			case Item.RareType.Uncommon:
				GUI.DrawTexture (rarenes, uncommon);
				break;
			case Item.RareType.Rare:
				GUI.DrawTexture (rarenes, rare);
				break;
			case Item.RareType.Epic:
				GUI.DrawTexture (rarenes, epic);
				break;
			case Item.RareType.Legendary:
				GUI.DrawTexture (rarenes, legendary);
			break;	
		}
		GUI.DrawTexture (new Rect (208, 75, sombragente.width, sombragente.height), sombragente);

		if(gente.Length>0){
			GUI.DrawTexture (new Rect (206, 34, 64, 64), Resources.Load<Texture2D> ("buyarea/frontales/"+getFileName(gente[0].GetComponent<PeopleScript>())));
			GUI.DrawTexture (new Rect (262, 34, 64, 64), Resources.Load<Texture2D> ("buyarea/frontales/"+getFileName(gente[1].GetComponent<PeopleScript>())));
			GUI.DrawTexture (new Rect (233, 60, 64, 64), Resources.Load<Texture2D> ("buyarea/frontales/"+getFileName(gente[2].GetComponent<PeopleScript>())));
		}
		}else{
			GUIStyle stl = new GUIStyle(Resources.Load<GUISkin> ("pix2").GetStyle ("RealValue"));

			GUI.TextArea (new Rect (0, -10, size.x, size.y), "Hmmm... No soy\n capaz de\nidentificar\nese objeto...", stl);
		}
		GUILayout.EndArea ();

		GUI.matrix = bc;	
	}

	public static string getFileName(PeopleScript ps){
		string filename = "";
		
		switch (ps.genre) {
		case PeopleScript.Genre.Male:	filename+="h"; break;
		case PeopleScript.Genre.Female:	filename+="m"; break;
		}
		switch (ps.age) {
		case PeopleScript.Age.Young:	filename+="j"; break;
		case PeopleScript.Age.Adult:	filename+="a"; break;
		case PeopleScript.Age.Old:		filename+="an"; break;
		}
		switch (ps.socialClass) {
		case PeopleScript.SocialClass.Low:		filename+="p"; break;
		case PeopleScript.SocialClass.Middle:	filename+="n"; break;
		case PeopleScript.SocialClass.Gentry:	filename+="b"; break;
		}
		
		return filename;
	}

	public void show(){
		if (!shown) {
			shown =true;
			gente = Resources.Load<ItemScorer>("ItemScorer").getOrderedPeople(item);
			Go.to (this, 0.5f, new TweenConfig ().vector2Prop ("Scale", new Vector2(1f,1f)).setEaseType (EaseType.BackOut));
		}
	}
	
	public void hide(){
		if (shown) {
			shown = false;
			Go.to (this, 0.25f, new TweenConfig ().vector2Prop ("Scale", new Vector2(0.0001f,0.0001f)).setEaseType (EaseType.BackIn));
		}
	}
}
