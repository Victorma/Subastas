using UnityEngine;
using System.Collections.Generic;

public class PubliTypes : ScriptableObject {

	[System.Serializable]
	public class PubliType {
		public string name;
		public Texture2D imagen;
		public Color color;
		
		public float Hombre, Mujer, Joven, Adulto, Anciano, Prole, Media, Burgues;
	}

	public PubliType[] tipos;

}
