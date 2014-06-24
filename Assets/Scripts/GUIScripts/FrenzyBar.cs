using UnityEngine;
using System.Collections;

public class FrenzyBar : MonoBehaviour {

	private PujadorScript[] pujadores; 
	private Rect area;
	private Material m_Material;
	// Use this for initialization
	void Start () {
		pujadores = GameObject.FindObjectsOfType<PujadorScript>();
		//area = GetComponent<AreaProvider>().area;
		m_Material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
		StartCoroutine(DrawFrenzyBar());
	}

	private void DrawQuad(Color aColor,float aAlpha, float percent)
	{
		aColor.a = aAlpha;
		m_Material.SetPass(0);
		GL.Color(aColor);
		GL.PushMatrix();
		GL.LoadProjectionMatrix(transform.localToWorldMatrix);
		GL.Begin(GL.QUADS);

		float z = 0;
		GL.Vertex3(area.x, area.y, z);
		GL.Vertex3(area.x, area.y+area.height, z);
		GL.Vertex3(area.x+area.width, area.y+area.height, z);
		GL.Vertex3(area.x+area.width, area.y, z);
		GL.End();
		GL.PopMatrix();
	}

	private float frenzy;

	private IEnumerator DrawFrenzyBar(){
		while(true){
			yield return new WaitForEndOfFrame();
			DrawQuad(Color.white,1,frenzy);
		}
	}

	// Update is called once per frame
	void Update () {
	
		frenzy = 0;
		foreach(PujadorScript p in pujadores)
			if(p.enabled)
				frenzy+=p.frenzy;

		frenzy /= (float)pujadores.Length;


	}
}
