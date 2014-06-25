using UnityEngine;
using System.Collections;

public class PubliDistributionGUI : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Vector2 size = new Vector2(400, 800);

	private Vector2 scale;
	public Vector2 Scale{ 
		get{ return scale; }
		set{ scale = value; }
	}
	// Use this for initialization
	void Start () {
		Go.to(this, 0.5f, new TweenConfig().vector2Prop("Scale", new Vector2(1,1)).setEaseType(EaseType.BackOut));
		scale = new Vector2(0.0001f, 0.0001f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){

		float rx = (Screen.width / (float)targetScreenSize.x)*scale.x;
		float ry = (Screen.height/ (float)targetScreenSize.y)*scale.y;

		GUISkin bcs = GUI.skin;
		Matrix4x4 bc = GUI.matrix;

		GUI.skin = Resources.Load<GUISkin>("pix");

		GUI.depth = 1;
		GUI.matrix = Matrix4x4.TRS (new Vector2(Screen.width/2f, Screen.height/2f), Quaternion.identity, new Vector3 (rx, ry, 1));

		GUI.Box(new Rect(-size.x /2f, -size.y /2f, size.x, size.y), "Publi distribution!");
		if(Event.current.isMouse)
			Event.current.Use();

		GUILayout.BeginArea(new Rect(-size.x /2f, -size.y /2f, size.x, size.y));
		GUILayout.BeginVertical();

		PubliTypes types = Resources.Load<PubliTypes>("PubliTypes");

		float space = size.y / (float)types.tipos.Length;

		for(int i = 0; i< types.tipos.Length; i++){
			PubliTypes.PubliType tipo = types.tipos[i];
			GUILayout.BeginArea(new Rect(0, space*i, size.x, size.y));
				GUILayout.BeginVertical();
					GUILayout.Label(tipo.name);
					GUILayout.BeginHorizontal();
						GUILayout.Box(tipo.imagen, GUILayout.ExpandWidth(false), GUILayout.Width(75), GUILayout.Height(75));
						GUILayout.HorizontalSlider(0,0,100, GUILayout.ExpandWidth(true));
					GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			GUILayout.EndArea();
		}



		GUILayout.EndVertical();
		GUILayout.EndArea();

		GUI.skin = bcs;
		GUI.matrix = bc;

	}
}
