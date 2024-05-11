using UnityEngine;


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

    [SerializeField] private AudioClip lunchSound, ExplodeSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Transform parent,Vector3 localPos)
    {
        lauched = false;
        gameObject.SetActive(true);
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

        SoundManager.Instance.PlaySound(SoundPoolType.SFX, lunchSound, transform.position);
    }
    private void Update()
    {
        if(lauched)
        {
            if(launchedTimer.TimerTick())
            {
                gameObject.SetActive(false);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(lauched)
        {
            ParticleManager.GetOrCreateInstance()?.PlayEffect("Explode", transform.position, Quaternion.identity);

            SoundManager.Instance.PlaySound(SoundPoolType.SFX, ExplodeSound, transform.position);

            Collider[] cols = explodeRange.Overlap(explodeMask);
            foreach (var col in cols)
            {
                if (col == gameObject) continue;
                EventManager.Instance.TriggerEvent(Consts.Event.Hit + col.gameObject.GetInstanceID(),
                    new HitInfo(HitType.Explode, dmg));
            }
            gameObject.SetActive(false);
        }
    }

}
