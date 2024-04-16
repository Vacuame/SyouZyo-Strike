using MoleMole;
using UnityEngine;

public class WorldHUD : BaseHUD
{
    private Transform cameraTrans;
    protected override void Init()
    {
        cameraTrans = Camera.main.transform;
    }
    protected virtual void Update()
    {
        transform.forward = cameraTrans.transform.forward * -1;
    }
}
