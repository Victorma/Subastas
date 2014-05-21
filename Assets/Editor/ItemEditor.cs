using UnityEngine;
using UnityEditor;
using System.Collections;

public class ItemEditor : MonoBehaviour {

[MenuItem("Assets/Create/Item")]
	public static void createIsoTextureAsset(){
		Item asset = new Item();  //scriptable object
		ProjectWindowUtil.CreateAsset(asset, "Item.asset");
		//AssetDatabase.CreateAsset(asset, "MyAsset.asset");
		//EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;    
	}
}
