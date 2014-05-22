using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Vignette : MonoBehaviour {
	
	//-----------------------------------------------------------
	// Public Variables
	//-----------------------------------------------------------
	
	public Texture2D vignetteTexture;
	
	//-----------------------------------------------------------
	// Unity GUI
	//-----------------------------------------------------------
	
	void OnGUI(){
		GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height ), vignetteTexture );	
	}
}
