using UnityEngine;
using System.Collections;

public class AuctionEndGUI : MonoBehaviour {

	private Vector2 targetScreenSize = new Vector2(480, 850);
	private Vector2 size = new Vector2(400, 400);
	
	private Vector2 scale;
	public Vector2 Scale{ 
		get{ return scale; }
		set{ scale = value; }
	}

	private PujadorScript ganador;
	private PeopleScript persona;
	private AuctioningScript script;

	void Start () {
		script = Auction.currentAuction.GetComponent<AuctioningScript>();
		ganador = script.ganadorActual;
		persona = (ganador!=null)?ganador.GetComponent<PeopleScript>():null;

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
		
		GUI.Box(new Rect(-size.x /2f, -size.y /2f, size.x, size.y), "Auction Results!");
		/*if(Event.current.isMouse)
			Event.current.Use();*/
		
		GUILayout.BeginArea(new Rect(-size.x /2f, -size.y /2f, size.x, size.y));
		GUILayout.BeginVertical();
		
		
		
		float publiSpace = size.y - 200;
		GUILayout.BeginArea(new Rect(20, 50, size.x - 40, size.y - 200));
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();

		// CASE WIN
		GUILayout.BeginVertical();
		float ganado = 0;
		if(ganador != null && ganador.state == PujadorScript.PujadorState.Waiting){


			GUILayout.Label("Ganador", GUILayout.Height(25));

			GUIStyle customStyle = new GUIStyle();
			customStyle.normal.background = Resources.Load<Texture2D> ("buyarea/frontales/"+SabioGUI.getFileName(persona));
			GUILayout.Box("",  customStyle, GUILayout.ExpandWidth(false), GUILayout.Width(75), GUILayout.Height(75));

			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			Item i = Auction.currentAuction.items[Auction.currentAuction.currentSlot];
			GUILayout.Label(i.name, GUILayout.Height(25));

			customStyle.normal.background = i.image.texture;
			GUILayout.Box("",  customStyle, GUILayout.ExpandWidth(false), GUILayout.Width(75), GUILayout.Height(75));
			ganado = script.price;

		}else{
			GUILayout.Label("Has perdido...", GUILayout.Height(25));

			string lose = "";
			if( script.price == 0)
				lose = "No hubo pujas...";
			else if( ganador == null || ganador.state == PujadorScript.PujadorState.Fighting)
				lose = "El ganador murio en una pelea...";

			GUILayout.Box(lose, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

			ganado = 0;
		}

		GUILayout.Label(ganado+"", GUI.skin.FindStyle("ItemGUIMoney"));

		GUILayout.EndVertical();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUILayoutUtility.GetRect(size.x, size.y - 120);
		
		if(GUILayout.Button("Cerrar", GUILayout.ExpandHeight(true))){
			StartCoroutine(Close ());
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

	IEnumerator Close(){
		Go.to(this, 0.3f, new TweenConfig().vector2Prop("Scale", new Vector2(0.00001f,0.00001f)).setEaseType(EaseType.BackIn));
		//yield return new WaitForSeconds(0.3f);
		GameObject.Destroy(this);
		Debug.Log ("Voy a llamar a end");
		script.End();
		yield return null;
	}
}
