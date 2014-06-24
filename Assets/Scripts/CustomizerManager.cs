using UnityEngine;
using System.Collections.Generic;

public class CustomizerManager : MonoBehaviour {

	public ItemScorer scores;
	public static CustomizerManager current {
		get { return GameObject.FindObjectOfType<CustomizerManager> (); }
	}

	public void Awake(){
		DontDestroyOnLoad (this);
	}

	public List<CustomizerInterface> customizers = new List<CustomizerInterface>();

	public void customizeGO(GameObject npc) {
		foreach(CustomizerInterface ci in customizers)
			ci.customize (npc);

	}



}
