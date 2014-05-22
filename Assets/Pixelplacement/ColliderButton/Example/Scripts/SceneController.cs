using UnityEngine;
using System.Collections;

public class ColliderButtonSceneController : MonoBehaviour {
	
	//-----------------------------------------------------------
	// Public Variables
	//-----------------------------------------------------------
	
	public TorqueSpin car;
	public ColliderButton leftArrow;
	public ColliderButton rightArrow;
	
	//-----------------------------------------------------------
	// Event Registration
	//-----------------------------------------------------------
	
	void OnEnable(){
		leftArrow.OnPress += ArrowPressedHandler;
		rightArrow.OnPress += ArrowPressedHandler;
	}
	
	void OnDisable(){
		leftArrow.OnPress -= ArrowPressedHandler;
		rightArrow.OnPress -= ArrowPressedHandler;
	}
	
	//-----------------------------------------------------------
	// Event Handlers
	//-----------------------------------------------------------
	
	void ArrowPressedHandler( ColliderButton sender ){
		
		switch ( sender.name ) {
		
		case "LeftArrow":
			car.SpinLeft();
			break;
			
		case "RightArrow":
			car.SpinRight();
			break;
		}	
	}
}