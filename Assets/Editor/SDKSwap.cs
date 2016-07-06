using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class SDKSwap : EditorWindow {

	static string _cardboardPath;
	static string _OVRPath;

	static string _manifestPath = "/Plugins/Android/AndroidManifest.xml";

	[MenuItem ("Window/Tools/SDK Switch")]
	static void Init() {
		SDKSwap window = (SDKSwap)EditorWindow.GetWindow(typeof(SDKSwap));
		window.Show();
	}

	void OnGUI() {
		GUILayout.Label("Manifest Paths", EditorStyles.boldLabel);

		GUILayout.Space(10f);
	
		GUILayout.Label("Cardboard Manifest", EditorStyles.label);
		GUILayout.TextField(_cardboardPath);

		if (GUILayout.Button("Set Cardboard Manifest")) {
			foreach (Object o in Selection.objects) {
				_cardboardPath = AssetDatabase.GetAssetPath(o);
				EditorPrefs.SetString("cardboardPath", _cardboardPath);
				break;
			}
		}

		GUILayout.Label("OVR Manifest", EditorStyles.label);
		GUILayout.TextField(_OVRPath);

		if (GUILayout.Button ("Set OVR Manifest")) {
			foreach (Object o in Selection.objects) {
				_OVRPath = AssetDatabase.GetAssetPath(o);
				EditorPrefs.SetString("OVRPath", _OVRPath);
				break;
			}
		}

		GUILayout.Space(10f);

		GUILayout.Label("Swap SDKs", EditorStyles.boldLabel);

		if (GUILayout.Button("OVR Mode")) {
			SwapOVR();
		}

		if (GUILayout.Button("Cardboard Mode")) {
			SwapCardboard();
		}
	}

	void OnEnable() {

		_cardboardPath = EditorPrefs.GetString("cardboardPath", null);
		_OVRPath = EditorPrefs.GetString("OVRPath", null);
	}
	
	//[MenuItem ("Window/Tools/Cardboard")]
	static void SwapCardboard() {
		string trimPath = _cardboardPath.Substring(6, _cardboardPath.Length - 6);
		string fullPath = Application.dataPath + trimPath;
		Debug.Log("FP: " + fullPath + " to " + GetManifestPath());
		FileUtil.ReplaceFile(fullPath, GetManifestPath());

		AssetDatabase.Refresh();
	}

	//[MenuItem("Window/Tools/OVR Mobile")]
	static void SwapOVR() {
		string trimPath = _OVRPath.Substring(6, _OVRPath.Length - 6);
		string fullPath = Application.dataPath + trimPath;
		Debug.Log("FP: " + fullPath + " to " + GetManifestPath());
		FileUtil.ReplaceFile(fullPath, GetManifestPath());

		AssetDatabase.Refresh();
	}

	static string GetManifestPath() {

		string path = Application.dataPath + _manifestPath;

		return(path);
	}

}
