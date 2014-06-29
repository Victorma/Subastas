using UnityEngine;
using System.Collections;

public class BidButtons : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);

	string valString;

	Rect group;

	float groupWidth;
	float groupHeight;
	
	float screenWidth;
	float screenHeight;

	float groupX;
	float groupY;

	public Vector3 LocalScale{
		get{ return transform.localScale; }
		set{ transform.localScale = value; }
	}


	AuctioningScript auctioning;

	// Use this for initialization
	void Start () {

		auctioning = Auction.currentAuction.GetComponent<AuctioningScript>();

		groupWidth = 300;
		groupHeight = 140;

		groupX = -(groupWidth/2f); 
		groupY = -(groupHeight/2f); 

		group = new Rect (groupX, groupY, groupWidth, groupHeight);

		valString ="0 $";
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

	// Update is called once per frame
	void OnGUI () {

		if(auctioning == null)
			auctioning = Auction.currentAuction.GetComponent<AuctioningScript>();

		if(auctioning == null)
			return;

		Matrix4x4 bc = GUI.matrix;

		float rx = (Screen.width / (float)targetScreenSize.x)*transform.localScale.x;
		float ry = (Screen.height/ (float)targetScreenSize.y)*transform.localScale.y;
		GUI.depth = 2;
		
		Vector2 point  = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		
		GUI.matrix = Matrix4x4.TRS (new Vector3(guipoint.x, guipoint.y, 0), Quaternion.identity, new Vector3 (rx, ry, 1));
		GUI.skin = Resources.Load<GUISkin>("pix");

		GUILayout.BeginArea(group, GUI.skin.box);

			GUIStyle myStyle = new GUIStyle(GUI.skin.label);
			myStyle.alignment = TextAnchor.MiddleCenter; 
			valString = auctioning.bid + " $";

			GUILayout.Label ("Bid", myStyle);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("-", GUILayout.ExpandHeight(true))) {
				auctioning.bid -= 25;
				if (auctioning.bid < 0) 
					auctioning.bid = 0;
			}
			myStyle = new GUIStyle(GUI.skin.textField);
			myStyle.alignment = TextAnchor.MiddleCenter; 

			GUILayout.TextField (valString, myStyle, GUILayout.Width(2*group.width / 5f), GUILayout.ExpandHeight(true));

			if (GUILayout.Button ("+", GUILayout.ExpandHeight(true)) )
				auctioning.bid += 25;

		GUILayout.EndHorizontal();

		GUILayout.EndArea();

		GUI.matrix = bc;

	}


}
