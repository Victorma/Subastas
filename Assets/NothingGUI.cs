using UnityEngine;
using System.Collections;

public class NothingGUI : MonoBehaviour {

	public Texture2D fondo;

	private NothingScript controller;
	public void setController(NothingScript controller){
		this.controller = controller;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI () {
		
		Matrix4x4 bc = GUI.matrix;
		
		Vector2 point  = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		
		float rx = Screen.width / (float)fondo.width;
		float ry = (Screen.height-guipoint.y) / (float)fondo.height;
		
		GUI.depth = 2;
		GUI.matrix = Matrix4x4.TRS (guipoint, Quaternion.identity, new Vector3 (rx, ry, 1));
		Rect guiArea = new Rect (0, 0, fondo.width, fondo.height);
		GUI.DrawTexture (guiArea, fondo);
		
		if(GUI.Button(new Rect(10, 10, guiArea.width - 20, 2*guiArea.height/5f - 15), "Upgrades!")){

		}
		
		float buttonSize = 95;
		
		GUILayout.BeginArea(new Rect(10, 2*guiArea.height/5f + 5, guiArea.width - buttonSize - 35, 3f*guiArea.height/5f - 10));
		GUILayout.BeginVertical();
		
		if(GUILayout.Button("Buy!", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))){
			controller.Buy();
			this.enabled = false;
		}

		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		GUI.matrix = bc;
	}
}
