using UnityEngine;
using System.Collections;

public class PubliGUI : MonoBehaviour {

	private SelectingScript controller;
	public void setController(SelectingScript controller){
		this.controller = controller;
	}
	
	public Texture2D fondo;

	void Start () {

	}

	Object other;
	void Update () {
		
	}


	
	private bool dropped;
	private Vector2 droppedPos;
	private Item openItem;
	private int itemSelected;
	private float percentOpened;
	
	public float openingSpeed = 0.05f;
	public float anchoHueco = 174f;
	public float altoHueco = 123f;
	public float separacionInicial = 25f;
	public float moneyValue;
	
	void OnGUI () {
		
		Matrix4x4 bc = GUI.matrix;
		Vector3 pos, scale, rot;
		pos = transform.position;
		scale = transform.localScale;
		
		Vector2 point  = Camera.main.WorldToScreenPoint(pos);
		Vector2 guipoint = GUIUtility.ScreenToGUIPoint(point);
		guipoint.y = Screen.height - guipoint.y;
		transform.position = guipoint;
		
		float rx = Screen.width / (float)fondo.width;
		float ry = (Screen.height-guipoint.y) / (float)fondo.height;

		GUI.depth = 2;
		GUI.matrix = Matrix4x4.TRS (guipoint, Quaternion.identity, new Vector3 (rx, ry, 1));
		Rect guiArea = new Rect (0, 0, fondo.width, fondo.height);
		GUI.DrawTexture (guiArea, fondo);

		if(GUI.Button(new Rect(10, 10, guiArea.width - 20, guiArea.height/2f - 15), "Change distribution")){
			other = this.gameObject.AddComponent<PubliDistributionGUI>();
			//other.publiArgs = controlle;
		}

		float buttonSize = 95;



		GUILayout.BeginArea(new Rect(10, guiArea.height/2f + 5, guiArea.width - buttonSize - 5, guiArea.height/2f - 10));
		GUILayout.BeginVertical();
		moneyValue = GUILayout.HorizontalSlider( moneyValue, 50, 50000);
		GUILayout.Label(moneyValue+"", Resources.Load<GUISkin> ("pix").GetStyle ("ItemGUIMoney"));
		GUILayout.EndVertical();
		GUILayout.EndArea();


		GUI.Button (new Rect (guiArea.width - buttonSize - 10, guiArea.height/2f + 5, buttonSize, buttonSize), "", Resources.Load<GUISkin> ("pix").GetStyle ("YesButton"));

		
		GUI.matrix = bc;
		
		transform.position = pos;
		transform.localScale = scale;
	}
}
