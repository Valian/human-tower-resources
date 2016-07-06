using UnityEngine;
using System.Collections;

public class ShowOnTap : MonoBehaviour {

    private int taps;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //GetComponent<TextMesh>().text = "Update tap - " + ++taps;
        }
	}

    public void OnTap()
    {
        GetComponent<TextMesh>().text = "Event tap - " + ++taps;
    }
}
