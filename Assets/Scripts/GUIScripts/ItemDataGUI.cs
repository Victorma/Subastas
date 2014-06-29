using UnityEngine;
using System.Collections.Generic;

public class ItemDataGUI : MonoBehaviour {

	int ImgWidth, ImgHeight, screenWidth, screenHeight;

	private bool shown = false;

	public Item item;

	public Font font;

	public Texture2D fondosmall;
	public Texture2D fondonormal;
	public Texture2D fondolarge;

	public Texture2D glass;

	public Texture2D unknown, common, uncommon, rare, epic, legendary;

	public List<PeopleScript> gente;

	public GameObject hidepoint, showpoint;

	public Vector3 LocalScale{
		get{ return transform.localScale; }
		set{ transform.localScale = value; }
	}

	public Vector3 LocalPosition{
		get{ return transform.position; }
		set{ transform.position = value; }
	}
	
	// Use this for initialization
	void Start () {

		ImgWidth = 480;
		ImgHeight = 255;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetItem(Item item){
		this.item = item;
	}

	void OnGUI () {

		Matrix4x4 bc = GUI.matrix;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;

		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;
		
		GUI.matrix = transform.localToWorldMatrix;

		switch (item.size) {
			case Item.Size.small: GUI.DrawTexture (new Rect (0, 0, ImgWidth, ImgHeight), fondosmall); break;
			case Item.Size.medium: GUI.DrawTexture (new Rect (0, 0, ImgWidth, ImgHeight), fondonormal); break;
			case Item.Size.large: GUI.DrawTexture (new Rect (0, 0, ImgWidth, ImgHeight), fondolarge); break;
		}

		GUI.DrawTexture (new Rect (68 - (item.image.texture.width/2) , 68 - (item.image.texture.height/2), item.image.texture.width, item.image.texture.height), item.image.texture);

		GUI.DrawTexture (new Rect (12, 12, 112, 112), glass);

		GUI.TextArea (new Rect (148, 8, 324, 26), item.name, Resources.Load<GUISkin> ("pix").GetStyle("ItemGUITittle"));

		GUI.TextArea (new Rect (153, 53, 314, 69), item.description, Resources.Load<GUISkin> ("pix").GetStyle("ItemGUIDescription"));

		GUI.TextArea (new Rect (32, 155, 176, 74), item.sellingPrice.ToString(), Resources.Load<GUISkin> ("pix").GetStyle("ItemGUIMoney"));

		switch (item.rareness) {
			case Item.RareType.Common:
				GUI.DrawTexture (new Rect (360, 132, 112, 112), common);
				break;
			case Item.RareType.Uncommon:
				GUI.DrawTexture (new Rect (360, 132, 112, 112), uncommon);
				break;
			case Item.RareType.Rare:
				GUI.DrawTexture (new Rect (360, 132, 112, 112), rare);
				break;
			case Item.RareType.Epic:
				GUI.DrawTexture (new Rect (360, 132, 112, 112), epic);
				break;
			case Item.RareType.Legendary:
				GUI.DrawTexture (new Rect (360, 132, 112, 112), legendary);
			break;	
		}

		GUI.DrawTexture (new Rect (236, 146, 64, 64), Resources.Load<Texture2D> ("buyarea/frontales/"+getFileName(gente[0])));
		GUI.DrawTexture (new Rect (292, 146, 64, 64), Resources.Load<Texture2D> ("buyarea/frontales/"+getFileName(gente[1])));
		GUI.DrawTexture (new Rect (263, 176, 64, 64), Resources.Load<Texture2D> ("buyarea/frontales/"+getFileName(gente[2])));
		
		GUI.matrix = bc;

		transform.position = pos;
		transform.localScale = scale;
	}


	private List<PeopleScript> getLikers(){
				List<PeopleScript> people = new List<PeopleScript> ();

				return people;
		}

	public void show(){
		if (!shown) {
			shown =true;
			Go.to (this, 1f, new TweenConfig ().vector3Prop ("LocalPosition", showpoint.transform.position).setEaseType (EaseType.CubicInOut));
		}
	}

	public void hide(){
		if (shown) {
			shown = false;
			Go.to (this, 1f, new TweenConfig ().vector3Prop ("LocalPosition", hidepoint.transform.position).setEaseType (EaseType.CubicInOut));
		}
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
}
