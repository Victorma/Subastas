using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {
	private GUISkin skin;
	private bool reanima = false;
	public Vector3 LocalScale{
		get{ return transform.localScale; }
		set{ transform.localScale = value; }
	}

	public Color CameraColor{
		get{ return RenderSettings.ambientLight; }
		set{ RenderSettings.ambientLight = value; }
	}
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(0.001f,0.001f,1f);
		Go.to(this, 0.5f, new TweenConfig()
		      .setDelay(2f)
		      .vector3Prop("LocalScale", new Vector3(1f,1f,1f))
		      .setEaseType(EaseType.BackOut));
		skin = (GUISkin) Resources.Load("pix");
	}

	IEnumerator Home() {
		yield return new WaitForSeconds(0.2f);
		AutoFade.LoadLevel("Home", 0.2f,0.5f, Color.black);
	}

	// Update is called once per frame
	void Update () {
		if(reanima){
			transform.localScale = new Vector3(1f,1f,1f);
			Go.to(this, 0.2f, new TweenConfig()
			      .vector3Prop("LocalScale", new Vector3(0.001f,0.001f,0.001f))
			      .setEaseType(EaseType.BackIn));
			Go.to(this, 0.2f, new TweenConfig()
			      .colorProp("CameraColor", new Color(0,0,0))
			      .setEaseType(EaseType.BackOut));
			reanima = false;
			StartCoroutine(Home());
		}
	}

	private float native_width = 480;
	private float native_height = 854;


	void OnGUI(){
		Matrix4x4 bc = GUI.matrix;
		GUI.skin = skin;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;
			
		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;

		float rx = Screen.width / native_width;
		float ry = Screen.height / native_height;

		GUI.matrix = Matrix4x4.TRS (new Vector3(Screen.width/2f, 2*Screen.height/3f, 0), Quaternion.identity, new Vector3 (rx*scale.x, ry*scale.y, 1));
		/*
		Rect area = GetComponent<AreaProvider>().area;
		Vector3 wcorner = transform.TransformPoint(new Vector3(area.x,area.y,0));
		Vector3 wothercorner = transform.TransformPoint(new Vector3(area.x + area.width,area.y+area.height,0));

		Vector2 corner = Camera.main.WorldToScreenPoint(wcorner);
		Vector3 otherCorner = Camera.main.WorldToScreenPoint(wothercorner);
		GUIUtility.ScreenToGUIRect(new Rect(corner.x, -corner.y, otherCorner.x - corner.x, - (otherCorner.y - corner.y))
*/

		if(GUI.Button(new Rect(-100, -60, 200, 120),"Start!")){
				reanima = true;
			}

		GUI.matrix = bc;

		transform.position = pos;
		transform.localScale = scale;
	}
}
