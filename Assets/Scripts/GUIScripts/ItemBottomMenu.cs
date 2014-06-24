using UnityEngine;
using System.Collections.Generic;

public class ItemBottomMenu : MonoBehaviour {

	BuyingScript controller;
	public void setController(BuyingScript controller){
		this.controller = controller;
	}

	int ImgWidth, ImgHeight, screenWidth, screenHeight;
	
	public List<Item> items;

	public Item showingitem;
	
	public Texture2D fondo, small, medium, large, glass;

	public Texture2D yes, yespress, yesdis, no, nopress, nodis;

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

		GUI.DrawTexture (new Rect (0, 0, ImgWidth, ImgHeight), fondo);

		if (this.showingitem != null) {
			GUI.TextArea (new Rect (21, 35, 215, 59), showingitem.buyingPrice.ToString(), Resources.Load<GUISkin> ("pix").GetStyle ("ItemGUIMoney"));
			if(GUI.Button (new Rect (255, 16, 95, 95), "", Resources.Load<GUISkin> ("pix").GetStyle ("NoButton"))){
				this.showingitem=null;
				controller.next();
			}
			if(GUI.Button (new Rect (367, 16, 95, 95), "", Resources.Load<GUISkin> ("pix").GetStyle ("YesButton"))){
				for(int i = 0; i<items.Count; i++)
					if(items[i]==null){ items[i] = showingitem; break;}
				this.showingitem=null; 
				controller.next();
			}
		} else {
			GUI.TextArea (new Rect (21, 35, 215, 59), "", Resources.Load<GUISkin> ("pix").GetStyle ("ItemGUIMoney"));
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
}
