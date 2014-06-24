using UnityEngine;
using System.Collections;

public class FrenzyRect : MonoBehaviour {

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

	public float frenzy;

	void OnGUI () {
		AuctioningScript auct = Auction.currentAuction.GetComponent<AuctioningScript>();
		if(auct == null)
			return;

		Matrix4x4 bc = GUI.matrix;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;

		Rect group = new Rect(0,0,170,30);
		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;
		
		GUI.matrix = transform.localToWorldMatrix;


		float frenzy = auct.Frenzy;

		GUILayout.BeginArea(group, GUI.skin.box);
		Rect cb = new Rect (0, 0, 170, 30);
			Color col_blanco = new Color (250, 250, 250);
		Rect cn = new Rect (5, 5, 160, 20);
			Color col_negro = new Color (0, 0, 0);
		Rect cr = new Rect (5, 5, 160 * frenzy, 20);
			Vector3 col_rojo = new Vector3 (255f, 0, 0);
			Vector3 col_azul = new Vector3 (0, 0, 255f);

			Color com = Color.red * frenzy + Color.blue * (1f - frenzy);

			drawQuad (cb, col_blanco);
			drawQuad (cn, col_negro);
			drawQuad (cr, com );

		Rect ct= new Rect (5, 5, 160, 20);
			drawText (ct, "MoneyText", "FRENZY");
		
		GUILayout.EndArea();
		
		
		GUI.matrix = bc;
		
		transform.position = pos;
		transform.localScale = scale;
	}

	void drawQuad(Rect position, Color color) {

		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();

		GUIStyle style = new GUIStyle(GUI.skin.box);
		style.normal.background = texture;

		GUI.Box(position, "", style);
	}

	void drawText(Rect position, string style, string text) {

		GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
		centeredStyle.alignment = TextAnchor.UpperCenter;
		centeredStyle.fontSize = 13;
		GUI.Label (position, text, centeredStyle);

	}
}
