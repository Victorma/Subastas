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
			end = end - HandleUtility.GUIPointToWorldRay(HandleUtility.WorldToGUIPoint(area.transform.position)).origin;
			end = area.transform.position + end;

			end = area.transform.InverseTransformPoint(end);
			end = new Vector3(end.x, end.y, 0);
			if(Event.current.type == EventType.mouseDown){
				selectingArea = 3;
			}
		}

		if(selectingArea == 1){
			if(Event.current.type == EventType.mouseDown){

				start = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
				start = start - HandleUtility.GUIPointToWorldRay(HandleUtility.WorldToGUIPoint(area.transform.position)).origin;
				start = area.transform.position + start;

				start = area.transform.InverseTransformPoint(start);
				start = new Vector3(start.x,start.y, 0);
				selectingArea = 2;
			}
		}
		

		
		if(selectingArea == 3){

			area.start = start;
			area.end = end;
			selectingArea = 0;
		}

		if(selectingArea > 1){

			Vector3 wstart = area.transform.TransformPoint(start);
			Vector3 wend = area.transform.TransformPoint(end);

			Vector3[] points = new Vector3[]{
				wstart,
				new Vector3(wstart.x, wstart.y, wend.z),
				wend,
				new Vector3(wend.x, wend.y, wstart.z)
			};

			Handles.DrawSolidRectangleWithOutline(points, new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f), Color.yellow);
			SceneView.currentDrawingSceneView.Repaint();
		}

		if(area.start != new Vector3() && area.end != new Vector3() && selectingArea == 0){


			Vector3 start = area.transform.TransformPoint(area.start);
			Vector3 end = area.transform.TransformPoint(area.end);

			Vector3[] points = new Vector3[]{
				start,
				new Vector3(start.x, start.y, end.z),
				end,
				new Vector3(end.x, end.y, start.z)
			};
			
			Handles.DrawSolidRectangleWithOutline(points, new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f), Color.yellow);
			SceneView.currentDrawingSceneView.Repaint();
		}
	}
}
