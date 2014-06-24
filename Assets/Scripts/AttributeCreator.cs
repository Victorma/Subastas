using UnityEngine;
using System.Collections;

public class AttributeCreator : MonoBehaviour {

	public CustomizerInterface type;
	public string property;

	private CustomizerInterface instance;

	void Start () {
		CustomizerManager.current.customizers.Add(this.instance);
	}
}
