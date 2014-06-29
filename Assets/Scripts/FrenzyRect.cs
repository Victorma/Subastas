using UnityEngine;
using System.Collections;

public class FrenzyRect : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Rect group = new Rect(-150,-30,300,60);
	

	public Vector3 LocalScale{
		get{ return transform.localScale; }
		set{ transform.localScale = value; }
	}

	public void Show(){
		Go.to(this, 0.5f, new TweenConfig()
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

		float rx = (Screen.width / (float)targetScreenSize.x)*transform.localScale.x;
		float ry = (Screen.height/ (float)targetScreenSize.y)*transform.localScale.y;

		GUI.skin = Resources.Load<GUISkin>("pix");

		Vector2 point  = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		
		GUI.matrix = Matrix4x4.TRS (new Vector3(guipoint.x, guipoint.y, 0), Quaternion.identity, new Vector3 (rx, ry, 1));
		GUI.depth = 2;
		float frenzy = auct.Frenzy;

		GUILayout.BeginArea(group, GUI.skin.box);
			Rect cb = new Rect (0, 0, group.width, group.height);

			Rect cn = new Rect (10, 10, group.width-20, group.height-20);
			Rect cr = new Rect (10, 10, (group.width-20) * frenzy, group.height-20);

			GUILayout.BeginArea(cb, GUI.skin.box);
			GUILayout.EndArea();

			Color com = Color.red * frenzy + Color.blue * (1f - frenzy);
			drawQuad (cn, Color.black);
			drawQuad (cr, com );

			drawText (cb, "Frenzy");
		
		GUILayout.EndArea();

		GUI.matrix = bc;
	}

	void drawQuad(Rect position, Color color) {

		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();

		GUIStyle style = new GUIStyle(GUI.skin.box);
		style.normal.background = texture;
		GUILayout.BeginArea(position, style);
		GUILayout.EndArea();
		//GUI.Box(position, "", style);
	}

	void drawText(Rect position, string text) {

		GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
		centeredStyle.alignment = TextAnchor.MiddleCenter;
		GUI.Label (position, text, centeredStyle);

	}
}
