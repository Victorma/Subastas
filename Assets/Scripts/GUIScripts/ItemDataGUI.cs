using UnityEngine;
using System.Collections.Generic;

public class ItemDataGUI : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Vector2 size = new Vector2(480, 144);
	private Vector2 scale;

	public bool shown = false;

	public Item item;

	public Font font;

	public Texture2D fondo;

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
		scale = new Vector2(0.0001f, 0.0001f);
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
		
		float rx = (Screen.width / (float)targetScreenSize.x)*scale.x;
		float ry = (Screen.height/ (float)targetScreenSize.y)*scale.y;
		
		GUI.depth = 2;
		GUI.matrix = Matrix4x4.TRS (guipoint, Quaternion.identity, new Vector3 (rx, ry, 1));

		GUI.DrawTexture (new Rect (0, 0, size.x, size.y), fondo);

		switch (item.size) {
		case Item.Size.small: GUI.DrawTexture (new Rect (12, 12, 112, 112), fondosmall); break;
		case Item.Size.medium: GUI.DrawTexture (new Rect (12, 12, 112, 112), fondonormal); break;
		case Item.Size.large: GUI.DrawTexture (new Rect (12, 12, 112, 112), fondolarge); break;
		}

		GUI.DrawTexture (new Rect (68 - (item.image.texture.width/2) , 68 - (item.image.texture.height/2), item.image.texture.width, item.image.texture.height), item.image.texture);

		GUI.DrawTexture (new Rect (12, 12, 112, 112), glass);

		GUI.TextArea (new Rect (148, 8, 324, 26), item.name, Resources.Load<GUISkin> ("pix").GetStyle("ItemGUITittle"));

		GUI.TextArea (new Rect (153, 53, 314, 69), item.description, Resources.Load<GUISkin> ("pix").GetStyle("ItemGUIDescription"));

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
}
