using UnityEngine;

public class ActivateObject : MonoBehaviour
{

    public DragAndRotate plainObject;

    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && GameManager.Instance.isGameActive)
        {
            plainObject.isActive = true;
        }
    }
}
