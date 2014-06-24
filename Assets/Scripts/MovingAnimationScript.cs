using UnityEngine;
using System.Collections;

public class MovingAnimationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		_myTransform = transform;
		_myAnimator = GetComponent<Animator>();
		lastPos = _myTransform.position;
	}
		
	// Update is called once per frame

	Animator _myAnimator;
	Transform _myTransform;
	Vector3 lastPos;
	void Update () {
		if(lastPos == _myTransform.position){
			// TODO parado
		}else{
			Vector3 diferencia = _myTransform.position - lastPos;
			if(Mathf.Abs(diferencia.x) > Mathf.Abs(diferencia.z)){
				if(diferencia.x > 0) _myAnimator.Play("east");
				else _myAnimator.Play("west");
			}else{
				if(diferencia.z > 0) _myAnimator.Play("north");
				else _myAnimator.Play("south");
			}
		}
		lastPos = _myTransform.position;
	}
}
