using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    [SerializeField] LayerMask targetMask, obstacleMask;
    public GameObject visibleTarget;

    public bool FindTarget()
    {
        Collider[] targetInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetInView.Length; i++)
        {
            GameObject target = targetInView[i].gameObject;
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float disToTar = Vector3.Distance(transform.position, target.transform.position);

                if(!Physics.Raycast(transform.position, dirToTarget, disToTar, obstacleMask))
                {
                    visibleTarget = target;
                    return true;
                }
            }
        }

        return false;
    }

    public void setTargetNull()
    {
        visibleTarget = null;
    }
    
    public Vector3 dirFromAngle(float _angle, bool global)
    {
        if (!global)
            _angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }
}
