using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{

    public DragAndRotate plainObject;

    private void Start()
    {
        //plainObject = GameObject.Find("Cube").GetComponent<DragAndRotate>();
        plainObject = GameObject.FindWithTag("Plain").GetComponent<DragAndRotate>();
    }
    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            plainObject.isActive = true;
        }
    }
}
