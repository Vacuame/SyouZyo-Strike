using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetector : MonoBehaviour
{
    private Vector3 towardVector { get { return transform.forward; } }
    public float viewDistance;
    public LayerMask targetLayer;
    public LayerMask sightLayer;
    [Range(0,360)]public float viewAngle;
    public float halfViewAngle { get { return viewAngle * 0.5f; } }

    public bool TryDetect(out GameObject found)
    {
        found = null;
        Collider[] preCheck = Physics.OverlapSphere(transform.position, viewDistance, targetLayer);//����
        if (preCheck.Length > 0)
        {
            Transform overLapCenter = preCheck[0].transform.Find("PosPoint").Find("Center");
            Vector3 discovery = overLapCenter.position;

            Vector3 toDiscoverVector = discovery - transform.position;
            if(Vector3.Angle(toDiscoverVector, towardVector) <= halfViewAngle)//�Ƕ�
            {
                if (Physics.Raycast(transform.position, toDiscoverVector,out RaycastHit hit,viewDistance,sightLayer))//����
                {
                    if(((1<<hit.transform.gameObject.layer)&targetLayer)!=0)//Ŀ��
                    {
                        found = hit.transform.gameObject;
                    }
                }
            }
        }
        return found != null;
    }

    private void OnDrawGizmos()
    {
        DrawRange();
    }

    [SerializeField]private bool showPlaneRange;
    private void DrawRange()
    {
#if UNITY_EDITOR
        var color = Color.red;
        color.a = 0.07f;
        UnityEditor.Handles.color = color;

        Vector3 towardVector = transform.forward;
        //���ƿ�ʼ�ķ�����towardΧ��y����ת-halfFOV��
        Vector3 beginDirection =Quaternion.AngleAxis(-halfViewAngle,transform.up)* towardVector;
        if(showPlaneRange) 
            UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, beginDirection, viewAngle, viewDistance);
        else
            UnityEditor.Handles.DrawSolidArc(transform.position,transform.forward, beginDirection, 360, viewDistance);
#endif
    }
}
