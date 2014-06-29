using UnityEngine;
using System.Collections.Generic;

public class ItemBottomMenu : MonoBehaviour {

	SabioGUI sbg;
	BuyingScript controller;
	public void setController(BuyingScript controller){
		this.controller = controller;
	}

	private bool inspectionenabled = true;

	private Vector2 targetScreenSize = new Vector2(480, 850);
	public Vector2 size = new Vector2(480, 255);
	private Vector2 scale;

	public Vector2 publicscale;
	
	public List<Item> items;

	public Item showingitem;
	
	public Texture2D fondo, small, medium, large, glass;

	public Texture2D yes, yespress, yesdis, no, nopress, nodis;

	public Texture2D sabio, sabiodisabled, inspeccionar, inspeccionardisabled;

	private AudioClip cash;

	// Use this for initialization
	void Start () {
		scale = new Vector2(0.0001f, 0.0001f);
		cash = Resources.Load<AudioClip> ("cash");
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

		publicscale = new Vector2 (rx, ry);

		GUI.depth = 2;
		GUI.matrix = Matrix4x4.TRS (guipoint, Quaternion.identity, new Vector3 (rx, ry, 1));

		GUIStyle bottombutton = Resources.Load<GUISkin> ("pix2").GetStyle ("BottomButton");
		GUIStyle bottombuttondisabled = Resources.Load<GUISkin> ("pix2").GetStyle ("BottomButtonDisabled");

		GUI.DrawTexture (new Rect (0, 0, size.x, size.y), fondo);


		if (this.showingitem != null) {
			if(GUI.Button (new Rect (size.x-sabio.width-16-30,-sabio.height-12,sabio.width+16,sabio.height+12), sabio, bottombutton)){
				if(sbg==null)
					sbg = this.gameObject.AddComponent<SabioGUI>();

				sbg.readingLevel = controller.SabioReadingLevel;

				if(sbg.shown){
					sbg.hide();
				}else{
					sbg.item = showingitem;
					sbg.show();
				}
			}
			if(inspectionenabled){
				if(GUI.Button (new Rect (size.x-(2f*sabio.width)-32-45,-inspeccionar.height-12,inspeccionar.width+16,inspeccionar.height+12),inspeccionar, bottombutton)){
					ItemInspectorGUI inspector = this.gameObject.AddComponent<ItemInspectorGUI>();
					inspector.ibm = this;
					inspector.item = this.showingitem;
				}
			}else{
				GUI.Button (new Rect (size.x-(2f*sabio.width)-32-45,-inspeccionar.height-12,inspeccionar.width+16,inspeccionar.height+12),inspeccionardisabled, bottombuttondisabled);
			}
			GUI.TextArea (new Rect (16, 16, 220, 95), showingitem.buyingPrice.ToString(), Resources.Load<GUISkin> ("pix").GetStyle ("ItemGUIMoney"));
			if(GUI.Button (new Rect (255, 16, 95, 95), "", Resources.Load<GUISkin> ("pix").GetStyle ("NoButton"))){
				next ();
			}
			if(GUI.Button (new Rect (367, 16, 95, 95), "", Resources.Load<GUISkin> ("pix").GetStyle ("YesButton"))){
				for(int i = 0; i<items.Count; i++)
					if(items[i]==null){ items[i] = showingitem; break;}
				audio.PlayOneShot(cash);
				next ();
			}
		} else {
			GUI.Button (new Rect (size.x-sabio.width-16-30,-sabio.height-12,sabio.width+16,sabio.height+12), sabiodisabled, bottombuttondisabled);
			GUI.Button (new Rect (size.x-(2f*sabio.width)-32-45,-inspeccionar.height-12,inspeccionar.width+16,inspeccionar.height+12),inspeccionardisabled, bottombuttondisabled);
			GUI.TextArea (new Rect (16, 16, 220, 95), "", Resources.Load<GUISkin> ("pix").GetStyle ("ItemGUIMoney"));
			GUI.Button (new Rect (255, 16, 95, 95), "", Resources.Load<GUISkin> ("pix").GetStyle ("NoButtonDisabled"));
			GUI.Button (new Rect (367, 16, 95, 95), "", Resources.Load<GUISkin> ("pix").GetStyle ("YesButtonDisabled"));
		}

		for(int i=0; i<3; i++){
			if(items[i]!=null){
				switch (items[i].size) {
					case Item.Size.small: GUI.DrawTexture (new Rect (20 + 163*i, 123, 112, 112), small); break;
					case Item.Size.medium: GUI.DrawTexture(new Rect (20 + 163*i, 123, 112, 112), medium); break;
					case Item.Size.large: GUI.DrawTexture (new Rect (20 + 163*i, 123, 112, 112), large); break;
				}

				GUI.DrawTexture (new Rect (76 + 163*i - (items[i].image.texture.width/2) , 56+123 - (items[i].image.texture.height/2), items[i].image.texture.width, items[i].image.texture.height), items[i].image.texture);

				GUI.DrawTexture (new Rect (20 + 163*i, 123, 112, 112), glass);
			}
		}
		
		GUI.matrix = bc;
		
		transform.position = pos;
		transform.localScale = scale;
	}

	public void next(){
		if(sbg!=null)
			sbg.hide();
		inspectionenabled = true;
		controller.next();
	}

	public void correctInspection(){
		if (showingitem.type == Item.Type.normal) {
			showingitem.activateDiscounts();
			inspectionenabled = false;
		}else if (showingitem.type == Item.Type.art){
			controller.detener();
		}
	}
}
