using UnityEngine;

public class DragAndRotate : MonoBehaviour
{
    public bool isActive = false;

    public float rotationSpeed;

    public float minRotationX;
    public float maxRotationX;
    public float minRotationZ;
    public float maxRotationZ;

    private void Start()
    {
        rotationSpeed = 10f;

        minRotationX = -30f;
        maxRotationX = 30f;
        minRotationZ = -15f;
        maxRotationZ = 30f;
}

    void Update()
    {
        if (isActive)
        {

            if (Input.touchCount == 1)
            {
                Touch screenTouch = Input.GetTouch(0);

                if (screenTouch.phase == TouchPhase.Moved)
                {
                    float rotationX = -screenTouch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                    float rotationZ = -screenTouch.deltaPosition.y * rotationSpeed * Time.deltaTime;

                    Vector3 currentRotation = transform.eulerAngles;

                    currentRotation.x = (currentRotation.x > 180) ? currentRotation.x - 360 : currentRotation.x;
                    currentRotation.z = (currentRotation.z > 180) ? currentRotation.z - 360 : currentRotation.z;

                    currentRotation.x = Mathf.Clamp(currentRotation.x + rotationX, minRotationX, maxRotationX);
                    currentRotation.z = Mathf.Clamp(currentRotation.z + rotationZ, minRotationZ, maxRotationZ);

                    transform.eulerAngles = new Vector3(currentRotation.x, 0f, currentRotation.z);

                }

                if (screenTouch.phase == TouchPhase.Ended)
                {
                    isActive = false;
                }
            }
        }

        Vector3 clampedRotation = transform.eulerAngles;
        clampedRotation.y = 0f;
        transform.eulerAngles = clampedRotation;

    }
}
