using UnityEngine;
using System.Collections;

public class CustomSliderClass {

	private static void drawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		
		GUIStyle style = new GUIStyle(GUI.skin.box);
		style.normal.background = texture;
		
		GUI.Box(position, "", style);
	}
	// new Rect(0,0,230,40)
	// Use this for initialization
	public static float Draw(Rect rect, float value, float min, float max, Color color){

		GUISkin gs = Resources.Load<GUISkin> ("pix2");

		GUILayout.BeginArea(rect);
		drawQuad(new Rect(8,6,(rect.width-16)*(value/max),rect.height-16),color);
		float r = GUILayout.HorizontalSlider (value, min, max, gs.GetStyle ("GoldSlider"), gs.GetStyle ("GoldSliderThumb"));
		GUILayout.EndArea ();

		return r;
	}
}
