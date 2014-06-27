using UnityEngine;
using System.Collections.Generic;

public class PubliDistributionGUI : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Vector2 size = new Vector2(400, 700);

	private Vector2 scale;
	public Vector2 Scale{ 
		get{ return scale; }
		set{ scale = value; }
	}

	public PubliTypes.PubliType[] types;
	public Dictionary<PubliTypes.PubliType, float> values = new Dictionary<PubliTypes.PubliType, float>();
	
	void Start () {
		Go.to(this, 0.5f, new TweenConfig().vector2Prop("Scale", new Vector2(1,1)).setEaseType(EaseType.BackOut));
		scale = new Vector2(0.0001f, 0.0001f);
	}

	private bool closing = false;
	void Update () {
		if(closing && scale.x < 0.001f)
			GameObject.Destroy(this);
	}

	void OnGUI(){

		GUI.Box(new Rect(-10, -10, Screen.width+20, Screen.height+20), "");

		float rx = (Screen.width / (float)targetScreenSize.x)*scale.x;
		float ry = (Screen.height/ (float)targetScreenSize.y)*scale.y;

		GUISkin bcs = GUI.skin;
		Matrix4x4 bc = GUI.matrix;

		GUI.skin = Resources.Load<GUISkin>("pix");

		GUI.depth = 1;
		GUI.matrix = Matrix4x4.TRS (new Vector2(Screen.width/2f, Screen.height/2f), Quaternion.identity, new Vector3 (rx, ry, 1));

		GUI.Box(new Rect(-size.x /2f, -size.y /2f, size.x, size.y), "Publi distribution!");
		/*if(Event.current.isMouse)
			Event.current.Use();*/

		GUILayout.BeginArea(new Rect(-size.x /2f, -size.y /2f, size.x, size.y));
		GUILayout.BeginVertical();



		float publiSpace = size.y - 200;
		GUILayout.BeginArea(new Rect(20, 50, size.x - 40, size.y - 200));

		float space = publiSpace / (float)types.Length;

		for(int i = 0; i< types.Length; i++){
			PubliTypes.PubliType tipo = types[i];
			GUILayout.BeginArea(new Rect(0, space*i, size.x, size.y));
				GUILayout.BeginVertical();
					GUILayout.Label(tipo.name, GUILayout.Height(25));
					GUILayout.BeginHorizontal();

						GUIStyle customStyle = new GUIStyle();
						customStyle.normal.background = tipo.imagen;
						GUILayout.Box("",  customStyle, GUILayout.ExpandWidth(false), GUILayout.Width(75), GUILayout.Height(75));
						float lastVal = values[types[i]];
						Rect rect = new Rect(85,50,280,38);
						values[types[i]] = CustomSliderClass.Draw(rect, values[types[i]], 0f, 1f, types[i].color);
						//values[types[i]] = GUILayout.HorizontalSlider(values[types[i]],0f,1f, GUILayout.MaxWidth(280));
						if(lastVal != values[types[i]]){
							float dif = (values[types[i]] - lastVal);
							int vars = 0;
							for(int j = 0; j< types.Length; j++)
								if(((dif > 0 && values[types[j]] > 0) ||  (dif < 0 && values[types[j]] < 1)) && i != j)
									vars++;

							float eachDif = dif / (float)vars;

							for(int j = 0; j< types.Length; j++)
								if(i!=j && ((dif > 0 && values[types[j]] > 0) ||  (dif < 0 && values[types[j]] < 1))){
									values[types[j]]-=eachDif;
								}
						}
					GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			GUILayout.EndArea();

		}
		GUILayout.EndArea();
		GUILayoutUtility.GetRect(size.x, size.y - 120);

		if(GUILayout.Button("Cerrar", GUILayout.ExpandHeight(true))){
			Go.to(this, 0.3f, new TweenConfig().vector2Prop("Scale", new Vector2(0.00001f,0.00001f)).setEaseType(EaseType.BackIn));
			closing = true;
		}

		GUILayout.EndVertical();
		GUILayout.EndArea();

		/*float value = 0;
		for(int j = 0; j< types.Length; j++)
			value+=values[types[j]];
		Debug.Log(value);*/

		GUI.skin = bcs;
		GUI.matrix = bc;

		if(Event.current.isMouse)
			Event.current.Use();

	}
}
