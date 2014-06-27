using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSelectionGUI : MonoBehaviour {

	private SelectingScript controller;
	public void setController(SelectingScript controller){
		this.controller = controller;
	}

	public Texture2D fondo, small, medium, large, glass;


	
	// Use this for initialization
	void Start () {
		//ImgWidth = 480f;
		//ImgHeight = 255f;
	}
	
	// Update is called once per frame
	void Update () {
		if(openItem!=null && percentOpened < 1){
			percentOpened+=openingSpeed*Time.deltaTime;
			if(percentOpened >1)percentOpened = 1;
		}else if(openItem == null && percentOpened > 0){
			percentOpened-=openingSpeed*Time.deltaTime;
			if(percentOpened <0)percentOpened = 0;
		}
		  
		if(openItem == null && percentOpened == 0){
			itemSelected = -1;
			dropped = false;
		}
	}

	private bool dropped;
	private Vector2 droppedPos;
	private Item openItem;
	private int itemSelected;
	private float percentOpened;

	public float openingSpeed = 0.05f;
	public float anchoHueco = 174f;
	public float altoHueco = 123f;
	public float separacionInicial = 25f;

	void OnGUI () {
		
		Matrix4x4 bc = GUI.matrix;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;

		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;

		float rx = Screen.width / (float)fondo.width;
		float ry = (Screen.height-guipoint.y) / (float)fondo.height;

		GUI.matrix = Matrix4x4.TRS (guipoint, Quaternion.identity, new Vector3 (rx, ry, 1));
		Rect guiArea = new Rect (0, 0, fondo.width, fondo.height);
		GUI.DrawTexture (guiArea, fondo);

		Item[] items = Auction.currentAuction.items;

		for(int i=0; i<3; i++){
			if(items[i] != null){
				Rect placeRect = new Rect (separacionInicial + anchoHueco*i, altoHueco, 112, 112);
				switch (items[i].size) {
				case Item.Size.small: GUI.DrawTexture (placeRect, small); break;
				case Item.Size.medium: GUI.DrawTexture(placeRect, medium); break;
				case Item.Size.large: GUI.DrawTexture (placeRect, large); break;
				}

				if(Event.current.isMouse && Event.current.type == EventType.mouseDown &&
				   placeRect.Contains(Event.current.mousePosition) && itemSelected == -1 && percentOpened == 0){
					percentOpened = 0;
					itemSelected = i;
					openItem = items[i];
				}else if(openItem != null && Event.current.isMouse && Event.current.type == EventType.MouseUp && !dropped){
					droppedPos = Event.current.mousePosition;
					dropped = true;
					percentOpened = 1;
					if(guiArea.Contains(Event.current.mousePosition)){
						openItem = null; 
						 // TODO hacer que se mueva para atras y despues se cierre
					}else{
						controller.itemDropped(items[i]);
					}
				}

				Vector2 itemPos = new Vector2((anchoHueco/2f) + anchoHueco*i - (items[i].image.texture.width/2), (altoHueco/2f)+altoHueco - (items[i].image.texture.height/2));

				if(itemSelected!=i) {
					GUI.DrawTexture (new Rect (itemPos.x , itemPos.y, items[i].image.texture.width, items[i].image.texture.height), items[i].image.texture);
					GUI.DrawTexture (new Rect (separacionInicial + anchoHueco*i, altoHueco, 112, 112), glass);
				}

			}
		}

		if(itemSelected != -1){
			int i = itemSelected;
			Vector2 itemPos = new Vector2((anchoHueco/2f) + anchoHueco*i - (items[i].image.texture.width/2), (altoHueco/2f)+altoHueco - (items[i].image.texture.height/2));
			if(openItem!=null && !dropped){
				GUI.DrawTexture (new Rect ( Event.current.mousePosition.x, Event.current.mousePosition.y, items[i].image.texture.width, items[i].image.texture.height), items[i].image.texture);
			}else{
				Vector2 middlePos = Vector2.Lerp(itemPos, droppedPos, percentOpened);
				GUI.DrawTexture (new Rect ( middlePos.x, middlePos.y, items[i].image.texture.width, items[i].image.texture.height), items[i].image.texture);
			}
			float anchoExtra = 112 * 0.1f*percentOpened;
			GUI.DrawTexture (new Rect (separacionInicial + anchoHueco*i - (anchoExtra/2f), altoHueco*(1f + percentOpened), 112+anchoExtra, 112*(1f-percentOpened)), glass);
		}
		
		GUI.matrix = bc;
		
		transform.position = pos;
		transform.localScale = scale;
	}
}
