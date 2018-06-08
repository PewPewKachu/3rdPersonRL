using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, 1.5f, 0);
        public float lookSmooth = 200f;
        public float distanceFromTarget = -8;
        public float zoomSmooth = 10;
        public float maxZoom = -2;
        public float minZoom = -15;
        public bool smoothFollow = false;
        public float smooth = 0.05f;

        [HideInInspector]
        public float newDistance = -8;
        [HideInInspector]
        public float adjustmentDistance = -8;
    }
    
    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation    = -20;
        public float yRotation    = -180;
        public float maxXrotation = 82;
        public float minXrotation = -82;
        public float vOrbitSmooth = 150;
        public float hOrbitSmooth = 150;
    }
    
    [System.Serializable]
    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL      = "OrbitHorizontal";
        public string ORBIT_VERTICAL        = "OrbitVertical";
        public string ZOOM                  = "Mouse ScrollWheel";
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines  = true;
        public bool drawAdjustedCollisionLines = true;
    }

    public CollisionHandler collisionHandler = new CollisionHandler();

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 camVel = Vector3.zero;

    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput;

    // Use this for initialization
    void Start()
    {
        MoveToTarget();

        collisionHandler.Initialize(Camera.main);
        collisionHandler.UpdateCameraClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(destination, transform.rotation, ref collisionHandler.desiredCameraClipPoints);

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        collisionHandler.UpdateCameraClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(destination, transform.rotation, ref collisionHandler.desiredCameraClipPoints);

        MoveToTarget();
        OrbitTarget();
        LookAtTarget();
        GetInput();
        ZoomInOnTarget();
        collisionHandler.CheckColliding(targetPos);
        position.adjustmentDistance = collisionHandler.GetAdjustedDistanceWithRayFrom(targetPos);


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 200f))
        {
            Debug.DrawLine(target.position, new Vector3(hit.point.x, hit.point.y, hit.point.z), Color.red);
        }

        target.rotation = Quaternion.Slerp(target.rotation, Quaternion.Euler(target.rotation.x, transform.eulerAngles.y, target.rotation.z), Time.deltaTime * 50);



        if (debug.drawDesiredCollisionLines || debug.drawAdjustedCollisionLines)
        {
            for (int i = 0; i < 5; i++)
            {
                if (debug.drawDesiredCollisionLines)
                {
                    Debug.DrawLine(targetPos, collisionHandler.desiredCameraClipPoints[i], Color.cyan);
                }

                if (debug.drawAdjustedCollisionLines)
                {
                    Debug.DrawLine(targetPos, collisionHandler.adjustedCameraClipPoints[i], Color.green);
                }

            }
        }



        Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);



    }

    void LateUpdate()
    {
        
    }

    void MoveToTarget()
    {
        targetPos = target.position + position.targetPosOffset; //Vector3.up * position.targetPosOffset.y + Vector3.forward * position.targetPosOffset.z + Vector3.right * position.targetPosOffset.x;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;

        if (collisionHandler.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;

            if (position.smoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, position.smooth);
            }
            else
            {
                transform.position = adjustedDestination;
            }
        }
        else
        {
            if (position.smoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, position.smooth);

            }
            else
            {
                transform.position = destination;
            }
        }
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }

    void GetInput()
    {
        vOrbitInput = Input.GetAxis("Mouse Y");
        hOrbitInput = Input.GetAxis("Mouse X");
        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
    }

    void OrbitTarget()
    {


        orbit.xRotation += vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += hOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;

        if (orbit.xRotation > orbit.maxXrotation)
        {
            orbit.xRotation = orbit.maxXrotation;
        }

        if (orbit.xRotation < orbit.minXrotation)
        {
            orbit.xRotation = orbit.minXrotation;
        }
    }

    void ZoomInOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth;

        if (position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }

        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints  = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera)
            {
                return;
            }

            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            //topleft
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; //added and rotated point relative to camera
            //topright
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;  //added and rotated point relative to camera
            //bottom left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; //added and rotated point relative to camera
            //bottom right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition; //added and rotated point relative to camera
            //camera's position
            intoArray[4] = cameraPosition - camera.transform.forward;
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }

            return false;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1)
                    {
                        distance = hit.distance;
                    }
                    else
                    {
                        if (hit.distance < distance)
                        {
                            distance = hit.distance;
                        }
                    }
                }
            }

            if (distance == -1)
            {
                return 0;
            }
            else
            {
                return distance;
            }

        }

        public void CheckColliding(Vector3 targetPosition)
        {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                colliding = true;
            }
            else
            {
                colliding = false;
            }
        }
    }
}
