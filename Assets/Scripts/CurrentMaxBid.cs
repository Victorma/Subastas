using UnityEngine;
using System.Collections;

public class CurrentMaxBid : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);

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

	void OnGUI () {
		AuctioningScript auct = Auction.currentAuction.GetComponent<AuctioningScript>();
		if(auct == null)
			return;

		Matrix4x4 bc = GUI.matrix;

		float rx = (Screen.width / (float)targetScreenSize.x)*transform.localScale.x;
		float ry = (Screen.height/ (float)targetScreenSize.y)*transform.localScale.y;

		Vector2 point  = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;

		GUI.matrix = Matrix4x4.TRS (new Vector3(guipoint.x, guipoint.y, 0), Quaternion.identity, new Vector3 (rx, ry, 1));
		GUI.depth = 2;
		
		GUI.skin = Resources.Load<GUISkin> ("pix");
		GUIContent texto = new GUIContent (auct.price+"");

		GUIStyle s = GUI.skin.GetStyle("ItemGUIMoney");
		Vector2 tam =  s.CalcSize (texto);
		if(tam.x < 100)
			tam.x = 100;

		Rect group = new Rect((-tam.x/2f)-10f,(-tam.y/2f)-10f,tam.x+20f,tam.y+20f);
		GUILayout.BeginArea(group);

		GUILayout.Label (texto,s, GUILayout.ExpandWidth(true)) ;
		
		GUILayout.EndArea();

		GUI.matrix = bc;
	}

}
