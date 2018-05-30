using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    private const float Y_ANGLE__MIN = -80.0f;
    private const float Y_ANGLE__MAX = 80.0f;

    public Transform lookAt;

    private Camera cam;

    private float distance = 10.0f;
    [SerializeField]
    private float distY = 1.5f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensX = 2.0f;
    private float sensY = 2.0f;

    bool isBlocked = false;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(lookAt.transform.position, new Vector3(hit.point.x, hit.point.y - 1, hit.point.z), Color.red);
        }

        #region MouseLookInput
        currentX += Input.GetAxis("Mouse X") * sensX;
        currentY -= Input.GetAxis("Mouse Y") * sensY;
        currentY = Mathf.Clamp(currentY, Y_ANGLE__MIN, Y_ANGLE__MAX);
        #endregion

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * dir;
        transform.LookAt(new Vector3(lookAt.position.x, lookAt.position.y + distY, lookAt.position.z));

        if (!isBlocked)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                distance -= 1.0f;
                if (distance < 7.5f)
                {
                    distance = 7.5f;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                distance += 1.0f;
                if (distance > 15.0f)
                {
                    distance = 15.0f;
                }
            }
        }

        Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);
    }

    void LateUpdate()
    {
       
        
    }
}
