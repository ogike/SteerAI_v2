using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public Transform toControl;
    public Camera cam;

    Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GameManagerScript.Instance.SetPlayerPosition(cam.ScreenToWorldPoint(Input.mousePosition));
    }
}
