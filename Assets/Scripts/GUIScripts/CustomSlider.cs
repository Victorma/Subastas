using UnityEngine;
using System.Collections;

public class CustomSlider: MonoBehaviour {

	public Color color;
	private float value = 50f; 
	private Rect area;

	// Use this for initialization
	void Start () {
	}

	void drawQuad(Rect position, Color color) {
		
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		
		GUIStyle style = new GUIStyle(GUI.skin.box);
		style.normal.background = texture;
		
		GUI.Box(position, "", style);
	}

	void Update () {

	}

	void OnGUI(){

		Matrix4x4 bc = GUI.matrix;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;

		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;
		
		GUI.matrix = transform.localToWorldMatrix;
		
		GUILayout.BeginArea(new Rect(0,0,230,40));
		drawQuad(new Rect(8,6,(230-16)*(value/100f),22),color);
		value = GUILayout.HorizontalSlider (value, 0, 100, Resources.Load<GUISkin> ("pix2").GetStyle ("GoldSlider"), Resources.Load<GUISkin> ("pix2").GetStyle ("GoldSliderThumb"));
		GUILayout.EndArea ();

		GUI.matrix = bc;
		
		transform.position = pos;
		transform.localScale = scale;
	}
}
