using UnityEngine;
using System.Collections;

public class TorqueSpin : MonoBehaviour {
	
	//-----------------------------------------------------------
	// Public Variables
	//-----------------------------------------------------------
	
	public float spinForce = 300;
	
	//-----------------------------------------------------------
	// Private Variables
	//-----------------------------------------------------------
	
	Rigidbody cachedRigidBody;
	
	//-----------------------------------------------------------
	// Init
	//-----------------------------------------------------------
	
	void Awake(){
		cachedRigidBody = rigidbody;	
	}
	
	//-----------------------------------------------------------
	// Public Methods
	//-----------------------------------------------------------
	
	public void SpinLeft(){
		cachedRigidBody.AddTorque( Vector3.up * spinForce );
	}
	
	public void SpinRight(){
		cachedRigidBody.AddTorque( Vector3.up * -spinForce );
	}
}
