using UnityEngine;
using System.Collections;

public class CurrentMaxBid : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 LocalScale{
		get{ return transform.localScale; }
		set{ transform.localScale = value; }
	}

	public void Show(){
		Go.to(this, 0.5f, new TweenConfig()
		      .setDelay(2f)
		      .vector3Prop("LocalScale", new Vector3(1f,1f,1f))
		      .setEaseType(EaseType.BackOut));
	}
	
	public void Hide(){
		Go.to(this, 0.2f, new TweenConfig()
		      .vector3Prop("LocalScale", new Vector3(0.001f,0.001f,0.001f))
		      .setEaseType(EaseType.BackIn));
	}

	void OnGUI () {
		AuctioningScript auct = Auction.currentAuction.GetComponent<AuctioningScript>();
		if(auct == null)
			return;

		Matrix4x4 bc = GUI.matrix;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;

		GUI.skin = Resources.Load<GUISkin> ("pix");
		GUIContent texto = new GUIContent ("Price: " + auct.price + " $");

		Vector2 tam = GUI.skin.label.CalcSize (texto);

		Rect group = new Rect(-tam.x/2f,-tam.y/2f,tam.x + 10f,tam.y + 10f);
		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;
		
		GUI.matrix = transform.localToWorldMatrix;

		/*Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,new Color (0, 0, 0, 1));
		texture.Apply();*/
		//GUI.skin.box.normal.background = texture;

		GUILayout.BeginArea(group, GUI.skin.box);

			Color col_blanco = new Color (255,215,0);
			drawText (col_blanco, texto.text);
		
		GUILayout.EndArea();
		
		
		GUI.matrix = bc;
		
		transform.position = pos;
		transform.localScale = scale;
	}
	
	void drawText(Color color, string text) {
		GUI.contentColor = color;

		var centeredStyle = GUI.skin.GetStyle("Label");
		centeredStyle.alignment = TextAnchor.UpperLeft;
		centeredStyle.fontSize = 13;
		GUILayout.Label (text, centeredStyle);
	}
}
