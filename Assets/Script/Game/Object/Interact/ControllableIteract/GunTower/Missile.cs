using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    private float launchedTimer;
    [HideInInspector]public bool lauched;

    [SerializeField] private Collider explodeRange;
    [SerializeField] private LayerMask explodeMask;
    [SerializeField] private float dmg;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Transform parent,Vector3 localPos)
    {
        lauched = false;
        rb.velocity = Vector3.zero;
        transform.SetParent(parent);
        transform.localPosition = localPos;
        transform.localRotation = Quaternion.identity;
    }

    public void Launch(Vector3 dir,float spd)
    {
        transform.SetParent(null);
        rb.velocity = dir*spd;
        launchedTimer = 5f;
        lauched= true;
    }
    private void Update()
    {
        if(lauched)
        {
            if(launchedTimer.TimerTick())
            {
                gameObject.SetActive(true);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(lauched)
        {
            ParticleManager.Instance.PlayEffect("Explode", transform.position, Quaternion.identity);

            Collider[] cols = explodeRange.Overlap(explodeMask);
            foreach (var col in cols)
            {
                if (col == gameObject) continue;
                EventManager.Instance.TriggerEvent(Consts.Event.Hit + col.gameObject.GetInstanceID(),
                    new HitInfo(HitType.Explode, dmg));
            }
        }
    }

}
