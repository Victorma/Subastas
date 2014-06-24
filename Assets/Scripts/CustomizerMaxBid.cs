using UnityEngine;
using System.Collections;

public class CustomizerMaxBid : CustomizerInterface {

	private float propertyValue;

	public float[] multipliers;

	public void Start(){
		CustomizerManager.current.customizers.Add (this);
	}

	public override void setLevel (float level)
	{
		return;
	}

	public override void customize(GameObject go){

		PujadorScript p = go.GetComponent<PujadorScript>();

		if(p!=null)
			p.maxBid *= multipliers[Mathf.FloorToInt(this.propertyValue)];
	}

}
