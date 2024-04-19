using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RayView : MonoBehaviour
{
    public float weaponRange = 50f;
    Vector3 defaultHitPoint=> transform.position + transform.forward * weaponRange;
    private LineRenderer rayLine;
    private void Awake()
    {
        rayLine = GetComponent<LineRenderer>();
        
    }
    void Update()
    {
        rayLine.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, weaponRange))
            rayLine.SetPosition(1, hit.point);
        else
            rayLine.SetPosition(1, defaultHitPoint);
    }
    //������Ϸ�����δ�������߹켣���ɼ�
    private void OnEnable()
    {
        rayLine.enabled = true;
    }
    private void OnDisable()
    {
        rayLine.enabled = false;
    }
}
