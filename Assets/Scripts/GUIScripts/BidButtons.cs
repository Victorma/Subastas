using UnityEngine;
using System.Collections;

public class BidButtons : MonoBehaviour {

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

		groupWidth = 170;
		groupHeight = 60;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		groupX = -(groupWidth/2f); //( screenWidth - groupWidth ) / 2f;
		groupY = -(groupHeight/2f); //3.25f*( screenHeight - groupHeight ) / 4f;

		group = new Rect (groupX, groupY, groupWidth, groupHeight);
		/*box = new Rect (0, 0, groupWidth, groupHeight);
		buttonPlus = new Rect (130, 25, 40, 30);
		text = new Rect (60, 25, 60, 30);
		buttonMinus = new Rect (5, 25, 40, 30);*/
		valString ="0 $";
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

	// Update is called once per frame
	void OnGUI () {

		if(auctioning == null)
			auctioning = Auction.currentAuction.GetComponent<AuctioningScript>();

		Matrix4x4 bc = GUI.matrix;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;
		
		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;
		
		GUI.matrix = transform.localToWorldMatrix;


		GUILayout.BeginArea(group, GUI.skin.box);

		GUIStyle myStyle = new GUIStyle(GUI.skin.label);
		myStyle.alignment = TextAnchor.MiddleCenter; 

		GUILayout.Label ("Bid", myStyle);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button ("-")) {
			auctioning.bid -= 25;
			if (auctioning.bid >= 0) {
				valString = auctioning.bid.ToString() + " $";
			}
			else {
				auctioning.bid = 0;
				valString = auctioning.bid.ToString() + " $";
			}
			
		}
		myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleCenter; 
		// Prrrrfect
		GUILayout.TextField (valString, myStyle, GUILayout.Width(2*group.width / 5f));

		if (GUILayout.Button ("+")) {
			auctioning.bid += 25;
			valString = auctioning.bid.ToString() + " $";
		}
		GUILayout.EndHorizontal();


		GUILayout.EndArea();

		
		GUI.matrix = bc;
		
		transform.position = pos;
		transform.localScale = scale;
	}


}
