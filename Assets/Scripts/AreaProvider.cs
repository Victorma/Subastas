using UnityEngine;
using System.Collections;

public class AreaProvider : MonoBehaviour {

	public Vector3 start;
	public Vector3 end;
	public Vector3 center {
		get { return (start + end) / 2f; }
	}
}
