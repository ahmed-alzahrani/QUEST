using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSetup : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
        //transform.localScale = new Vector3(10, 10, 1);	
        transform.localScale = new Vector3(Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height, Camera.main.orthographicSize * 2.0f, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
