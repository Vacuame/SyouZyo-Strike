using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实际的玩家，不仅控制角色，还有背包等其他功能
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
        //TODO 设置UI和背包之类的
    }

}
