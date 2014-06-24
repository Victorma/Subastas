using UnityEngine;
using System.Collections;

public abstract class CustomizerInterface : MonoBehaviour{

	public string customizerName;
	public Sprite sprite;
	public bool isLeveable;
	
	public abstract void customize(GameObject go);
	public abstract void setLevel(float level);

}