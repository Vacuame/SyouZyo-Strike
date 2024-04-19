using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class RayView : MonoBehaviour
{
    public float weaponRange = 50f;
    Vector3 defaultHitPoint=> transform.position + transform.forward * weaponRange;
    private LineRenderer rayLine;
    [SerializeField]private Transform redPoint;
    private Vector3 redPointScale;
    private void Awake()
    {
        rayLine = GetComponent<LineRenderer>();
        redPointScale = redPoint.localScale;
    }
    void Update()
    {
        rayLine.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, weaponRange))
        {
            redPoint.localScale = redPointScale;
            redPoint.position = hit.point;
            rayLine.SetPosition(1, hit.point);
        }
        else
        {
            redPoint.localScale = redPointScale * 0.1f;
            redPoint.position = defaultHitPoint;
            rayLine.SetPosition(1, defaultHitPoint);
        }
            
    }
    //进入游戏后对象未激活射线轨迹不可见
    private void OnEnable()
    {
        rayLine.enabled = true;
    }
    private void OnDisable()
    {
        rayLine.enabled = false;
        redPoint.localScale = Vector3.zero;
    }
}
