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
    //进入游戏后对象未激活射线轨迹不可见
    private void OnEnable()
    {
        rayLine.enabled = true;
    }
    private void OnDisable()
    {
        rayLine.enabled = false;
    }
}
