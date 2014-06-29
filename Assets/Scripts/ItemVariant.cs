using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemVariant {

	public string name;
	public Texture2D Image;
	public Rect area;

	public float percentdiscount;
	public float probability;
	
	bool checkValid(Vector3 point){
		return (area.x < point.x && point.x < area.x+area.width) && (area.y < point.y && point.y < area.y+area.height);
	}
}
