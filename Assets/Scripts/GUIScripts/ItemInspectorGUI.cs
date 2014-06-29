using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInspectorGUI : MonoBehaviour {

	public ItemBottomMenu ibm;
	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Vector2 size = new Vector2(400, 700);

	private Rect selectedarea;
	private Vector2 selectedpoint;
	private bool selected = false, disabled=false, dragging=false;

	private Texture2D area, areacorrect, areafail;

	private AudioClip correct, fail;
	
	private Vector2 scale;
	public Vector2 Scale{ 
		get{ return scale; }
		set{ scale = value; }
	}
	
	public Item item;
	
	void Start () {
		Go.to(this, 0.5f, new TweenConfig().vector2Prop("Scale", new Vector2(1,1)).setEaseType(EaseType.BackOut));
		scale = new Vector2(0.0001f, 0.0001f);

		area = Resources.Load<Texture2D> ("area");
		areacorrect = Resources.Load<Texture2D> ("areacorrect");
		areafail = Resources.Load<Texture2D> ("areafail");

		correct = Resources.Load<AudioClip> ("correct");
		fail = Resources.Load<AudioClip> ("fail");
		selected = false;

		selectedpoint = new Vector2 (-1f, -1f);
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
		GUISkin skin2 = Resources.Load<GUISkin>("pix2");
		
		GUI.depth = 1;
		GUI.matrix = Matrix4x4.TRS (new Vector2(Screen.width/2f, Screen.height/2f), Quaternion.identity, new Vector3 (rx, ry, 1));

		Texture2D itemimage = item.variants [item.currentVariant].Image;

		float proportion = ((size.x - 40+12) / (itemimage.width));
		float maxHeigth = (proportion*(itemimage.height))-12;

		GUI.Box(new Rect(-size.x /2f, -((maxHeigth+140)/2f), size.x, maxHeigth+140), "Inspeccionar Objeto");
		/*if(Event.current.isMouse)
			Event.current.Use();*/

		Rect total = new Rect (-size.x / 2f, -((maxHeigth + 140) / 2f), size.x, maxHeigth + 160);

		GUILayout.BeginArea(total);
		GUILayout.BeginVertical();
			
		Rect imagen = new Rect (20, 40, size.x - 40, maxHeigth);
		Rect suma = new Rect((targetScreenSize.x/2f) + (total.x+imagen.x+6), (targetScreenSize.y/2f) +(total.y+imagen.y+6),imagen.width-12,imagen.height-12);

			GUILayout.BeginArea(imagen);
				
				GUI.Box (new Rect(0,0,size.x - 40,maxHeigth),"", skin2.GetStyle("ItemInspectorBackground"));
				GUI.DrawTexture (new Rect(6,6,size.x - 52,maxHeigth-12),itemimage,ScaleMode.StretchToFill);
				
				if (Event.current.type == EventType.MouseDown) dragging = true;

				if (dragging && !disabled) {
					Vector2 mousepos = new Vector2 (Input.mousePosition.x/ rx, targetScreenSize.y -(Input.mousePosition.y)/ ry);
					if(suma.Contains(mousepos)){
						selected = true;
						float proportiony = (maxHeigth-12)/itemimage.height, proportionx = (size.x - 52)/itemimage.width;
						selectedpoint = new Vector2((mousepos.x-suma.x)/proportionx, (mousepos.y-suma.y)/proportiony);
						selectedarea = new Rect (mousepos.x -suma.x+6 - (area.width / 2f), mousepos.y-suma.y+6 - (area.height / 2f), area.width, area.height);
					}
				}

				if(selected)
					GUI.Box (selectedarea, area, new GUIStyle());

			GUILayout.EndArea();
			GUILayoutUtility.GetRect(size.x, maxHeigth+60);
			
			GUILayout.BeginHorizontal ();
			if(GUILayout.Button("Cancelar", GUILayout.ExpandHeight(true))&&!disabled){
				disabled=true;
				close ();
			}
			
			if(GUILayout.Button("Aceptar", GUILayout.ExpandHeight(true))&&!disabled){
				disabled=true;
				StartCoroutine(CheckPoint(selectedpoint,item.variants [item.currentVariant]));
			}
			GUILayout.EndHorizontal ();
		
		GUILayout.EndVertical();
		GUILayout.EndArea();

		if (Event.current.type == EventType.mouseUp) dragging = false;
		if (Event.current.isMouse)
			Event.current.Use ();

		GUI.skin = bcs;
		GUI.matrix = bc;		
	}

	public void close(){
		Go.to(this, 0.3f, new TweenConfig().vector2Prop("Scale", new Vector2(0.00001f,0.00001f)).setEaseType(EaseType.BackIn));
		closing = true;
	}

	IEnumerator CheckPoint(Vector2 point, ItemVariant variant){

		if(point.x!=-1f){
			RXCircle circulo = new RXCircle(point, 25);
			bool valido = circulo.CheckIntersectWithRect (variant.area);

			if (valido){ 
				area = areacorrect;
				audio.PlayOneShot(correct);
			}else{ 
				area = areafail;
				audio.PlayOneShot(fail);
			}

			yield return new WaitForSeconds(1f);

			if (valido) ibm.correctInspection ();
			else ibm.next();
		}
		close ();

		yield return new WaitForSeconds(0.1f);
	}
}
