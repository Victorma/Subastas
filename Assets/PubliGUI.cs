using UnityEngine;
using System.Collections.Generic;

public class PubliGUI : MonoBehaviour {

	private PubliScript controller;
	public void setController(PubliScript controller){
		this.controller = controller;
	}
	
	public Texture2D fondo;

	public PubliTypes.PubliType[] types;
	public Dictionary<PubliTypes.PubliType, float> values = new Dictionary<PubliTypes.PubliType, float>();

	void Start () {

		types = Resources.Load<PubliTypes>("PubliTypes").tipos;
		foreach(PubliTypes.PubliType t in types)
			values.Add(t, 1f/(float)types.Length);

	}

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

	public float moneyValue = 300;
	public float minMoney = 50;
	public float maxMoney = 5000;
	
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

		if(GUI.Button(new Rect(10, 10, guiArea.width - 20, 2*guiArea.height/5f - 15), "Change distribution")){
			PubliDistributionGUI distr = this.gameObject.AddComponent<PubliDistributionGUI>();
			distr.types = this.types;
			distr.values = this.values;
		}

		float buttonSize = 95;

		GUILayout.BeginArea(new Rect(10, 2*guiArea.height/5f + 5, guiArea.width - buttonSize - 35, 3f*guiArea.height/5f - 10));
		GUILayout.BeginVertical();


		//moneyValue = GUILayout.HorizontalSlider( moneyValue, 50, 50000);
		GUILayout.Space(40);
		Rect r = new Rect(0,0,350, 38);
		moneyValue = CustomSliderClass.Draw(r, Mathf.RoundToInt(moneyValue), minMoney, maxMoney, Color.yellow);
		GUILayout.Label(moneyValue+"", Resources.Load<GUISkin> ("pix").GetStyle ("ItemGUIMoney"));
		GUILayout.EndVertical();
		GUILayout.EndArea();


		if(GUI.Button (new Rect (guiArea.width - buttonSize - 10, guiArea.height/2f + 5, buttonSize, buttonSize), "", Resources.Load<GUISkin> ("pix").GetStyle ("YesButton"))){
			controller.SelectPubli(values, moneyValue);
		}

		GUI.matrix = bc;
	}
}
