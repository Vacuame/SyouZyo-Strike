using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBasic;
using MoleMole;

/// <summary>
/// ʵ�ʵ���ң��������ƽ�ɫ�����б�������������
/// </summary>
public class PlayerController : Controller
{
    protected override void ControlPawn(Pawn pawn)
    {
        base.ControlPawn(pawn);

        if (pawn is PlayerCharacter)
            ControllPlayer(pawn as PlayerCharacter);
    }

    private void ControllPlayer(PlayerCharacter character)
    {
        //TODO ����UI�ͱ���֮���
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Tab))
            UIManager.Instance.SwitchOnPeek(new BaseContext(BagPanel.uiType));
    }

}


