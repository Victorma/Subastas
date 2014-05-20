using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AreaProvider))]
public class AreaEditor : Editor {


	private int selectingArea = 0;
	private Vector3 start;
	private Vector3 end;
	

	public override void OnInspectorGUI(){


		if(GUILayout.Button("Select Area")){
			Tools.current = Tool.None;
			selectingArea = 1;
		}

	}

	void OnSceneGUI (){
		
		AreaProvider area = target as AreaProvider;

		if(selectingArea != 0){
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		}

		if(selectingArea == 2){
			end = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
			end = new Vector3(end.x,end.y, -5);
			if(Event.current.type == EventType.mouseDown){
				selectingArea = 3;
			}
		}

		if(selectingArea == 1){
			if(Event.current.type == EventType.mouseDown){

				start = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
				start = new Vector3(start.x,start.y, -5);
				selectingArea = 2;
			}
		}
		

		
		if(selectingArea == 3){
			area.area = new Rect(start.x, start.y, end.x - start.x, end.y-start.y);
			selectingArea = 0;
		}

		if(selectingArea > 1){
			Vector3[] points = new Vector3[]{
				start,
				new Vector3(start.x, end.y, start.z),
				end,
				new Vector3(end.x, start.y, start.z)
			};

			Handles.DrawSolidRectangleWithOutline(points, new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f), Color.yellow);
			SceneView.currentDrawingSceneView.Repaint();
		}

		if(area.area!=new Rect() && selectingArea == 0){
			

			Vector3 start = new Vector3(area.area.x, area.area.y, area.transform.position.z);
			Vector3 end = new Vector3(area.area.x + area.area.width, area.area.y + area.area.height, area.transform.position.z);
			
			Vector3[] points = new Vector3[]{
				start,
				new Vector3(start.x, end.y, start.z),
				end,
				new Vector3(end.x, start.y, start.z)
			};
			
			Handles.DrawSolidRectangleWithOutline(points, new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f), Color.yellow);
			SceneView.currentDrawingSceneView.Repaint();
		}
	}
}
