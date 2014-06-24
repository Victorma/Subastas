using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {


	string texto;

	// Use this for initialization

	public Vector3 LocalScale{
		get{ return transform.localScale; }
		set{ transform.localScale = value; }
	}


	IEnumerator IntroSecuence(){

		texto = "Wait...";
			Go.to(this, 1f, new TweenConfig().vector3Prop("LocalScale", new Vector3(2f,2f,2f)).setEaseType(EaseType.BackOut));
			yield return new WaitForSeconds(1.2f);		
			
			Go.to(this, 0.2f, new TweenConfig() .vector3Prop("LocalScale", new Vector3(0.001f,0.001f,0.001f)).setEaseType(EaseType.BackIn));
			yield return new WaitForSeconds(3.7f);	

		texto = "Auction...";
			Go.to(this, 0.8f, new TweenConfig().vector3Prop("LocalScale", new Vector3(2f,2f,2f)).setEaseType(EaseType.BackOut));
			yield return new WaitForSeconds(1f);		
			
			Go.to(this, 0.2f, new TweenConfig().vector3Prop("LocalScale", new Vector3(0.001f,0.001f,0.001f)).setEaseType(EaseType.BackIn));
			yield return new WaitForSeconds(0.3f);	

		texto = "is Going...";
			Go.to(this, 0.8f, new TweenConfig().vector3Prop("LocalScale", new Vector3(2f,2f,2f)).setEaseType(EaseType.BackOut));
			yield return new WaitForSeconds(1f);		
			
			Go.to(this, 0.2f, new TweenConfig().vector3Prop("LocalScale", new Vector3(0.001f,0.001f,0.001f)).setEaseType(EaseType.BackIn));
			yield return new WaitForSeconds(0.3f);	

		texto = "to Start!";
			Go.to(this, 0.8f, new TweenConfig().vector3Prop("LocalScale", new Vector3(2f,2f,2f)).setEaseType(EaseType.BackOut));
			yield return new WaitForSeconds(1f);		
			
			Go.to(this, 0.2f, new TweenConfig().vector3Prop("LocalScale", new Vector3(0.001f,0.001f,0.001f)).setEaseType(EaseType.BackIn));
			yield return new WaitForSeconds(0.5f);	


		int count = 3;

		while(count >= 0){
			if(count == 0)
				texto = "GO!";
			else
				texto = count.ToString();

			Go.to(this, 0.1f, new TweenConfig()
			      .vector3Prop("LocalScale", new Vector3(2f,2f,2f))
			      .setEaseType(EaseType.BackOut));

			yield return new WaitForSeconds(0.2f);		

			Go.to(this, 0.05f, new TweenConfig()
			      .vector3Prop("LocalScale", new Vector3(0.001f,0.001f,0.001f))
			      .setEaseType(EaseType.BackIn));

			yield return new WaitForSeconds(0.05f);	
			count --;
		}

	}

	public void DoIntroSecuence(int count){
		StartCoroutine(IntroSecuence());
	}
	
	void OnGUI () {

		
		Matrix4x4 bc = GUI.matrix;
		Vector3 pos = transform.position;
		
		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;
		
		GUI.matrix = transform.localToWorldMatrix;

		GUI.skin = Resources.Load<GUISkin>("pix");
		GUI.Label (new Rect(-10,-10,20,20), texto, GUI.skin.GetStyle("IntroStyle"));

		
		GUI.matrix = bc;
		
		transform.position = pos;

	}
		

}
