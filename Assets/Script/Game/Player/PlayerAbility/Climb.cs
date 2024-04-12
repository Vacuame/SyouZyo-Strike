using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Climb;

public class Climb : AbstractAbility<Climb_SO>
{
    public Climb(AbilityAsset abilityAsset) : base(abilityAsset)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new ClimbSpec(this,owner);
    }


    public class ClimbSpec : AbilitySpec
    {
        Climb climb;
        Climb_SO asset => climb.AbilityAsset;

        private int climable;
        private int climbType;
        private Vector3 climbEdge;
        public Vector3 toWallDire;
        private float wallHeight;
        private Vector3 climb_leftHand, climb_rightHand, climb_rightLeg, climb_root, climbDir;

        private int armAnimLayerIndex;

        Animator anim;
        CharacterController cc;
        Transform transform;

        public ClimbSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            climb = ability as Climb;
        }

        protected override bool CheckOtherCondition()
        {
            return climable != 0;
        }

        public override void ActivateAbility(params object[] args)
        {
            anim = args[0] as Animator;
            cc = args[1] as CharacterController;
            transform = args[2] as Transform;

            cc.enabled = false;
            HUDManager.GetHUD<AimHUD>()?.SetText("tip", null);

            armAnimLayerIndex = anim.GetLayerIndex("Arm");
            anim.SetLayerWeight(armAnimLayerIndex, 0);
            climbType = climable;
            anim.SetInteger("climbType", climbType);

            climbDir = toWallDire;
            float climbHeight = wallHeight;
            switch (climbType)
            {
                case 1:
                    climb_leftHand = climbEdge + Vector3.Cross(climbDir, Vector3.up) * 0.3f;
                    transform.position = climbEdge + Vector3.down * climbHeight - climbDir * 0.5f;
                    break;
                case 2:
                    climb_rightHand = climbEdge;
                    break;
                case 3:
                    transform.position = climbEdge + Vector3.down * climbHeight - climbDir * 0.8f;
                    climb_rightHand = climbEdge + Vector3.Cross(climbDir, Vector3.down) * 0.3f;
                    climb_rightLeg = climbEdge + Vector3.down * 1.2f;
                    break;
            }
            
        }

        public override void EndAbility()
        {
            cc.enabled = true;
            climbType = 0;
            anim.SetLayerWeight(armAnimLayerIndex, 1);
            anim.SetInteger("climbType", climbType);
        }

        public override void AnimatorMove()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Climb") && !anim.IsInTransition(0))
                EndSelf();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(toWallDire), 0.5f);

            anim.ApplyBuiltinRootMotion();

            MatchTargetWeightMask weightMask = new MatchTargetWeightMask(Vector3.one, 0);
            switch (climbType)
            {
                case 1:
                    anim.MatchTarget(climb_leftHand, Quaternion.identity, AvatarTarget.LeftHand, weightMask, 0, 0.1f);
                    anim.MatchTarget(climb_leftHand + Vector3.up * 0.25f + climbDir * 0.2f, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.up, 0f), 0.1f, 0.3f);
                    break;
                case 2:
                    anim.MatchTarget(climb_rightHand, Quaternion.identity, AvatarTarget.RightHand, weightMask, 0.1f, 0.18f);
                    anim.MatchTarget(climb_rightHand + Vector3.up * 0.1f, Quaternion.identity, AvatarTarget.RightHand, weightMask, 0.35f, 0.45f);
                    break;
                case 3:
                    anim.MatchTarget(climb_rightLeg, Quaternion.identity, AvatarTarget.RightFoot, weightMask, 0.01f, 0.13f);
                    anim.MatchTarget(climb_rightHand, Quaternion.identity, AvatarTarget.RightHand, weightMask, 0.2f, 0.32f);
                    anim.MatchTarget(climb_rightHand + climbDir * 0.5f, Quaternion.identity, AvatarTarget.RightFoot, weightMask, 0.65f, 1f);
                    break;
            }
        }

        protected override void SustainedTick()
        {
            if (climbType != 0) return;
            climable = ClimbCheck();

            if(CanActivate())
                HUDManager.GetHUD<AimHUD>()?.SetText("tip", "Press F to climb");
            else
                HUDManager.GetHUD<AimHUD>()?.SetText("tip", null);
        }
        
        private int ClimbCheck()
        {
            RaycastHit climbHit;
            if (Physics.Raycast(owner.transform.position + Vector3.up * asset.minClimbHeight,
                owner.transform.forward, out climbHit, asset.climbCheckDistance, asset.climbLayer))
            {
                toWallDire = -climbHit.normal;
                if (Vector3.Angle(owner.transform.forward, toWallDire) > asset.climbTowardAngle) return 0;

                wallHeight = asset.maxClimbHeight + asset.climbStep;

                RaycastHit lastClimbHit = climbHit;

                for (float height = asset.minClimbHeight + asset.climbStep; height <= asset.maxClimbHeight + asset.climbStep; height += asset.climbStep)
                {
                    if (Physics.Raycast(owner.transform.position + Vector3.up * height, toWallDire, out climbHit, asset.climbCheckDistance, asset.climbLayer))
                    {
                        lastClimbHit = climbHit;
                    }
                    else if (Physics.Raycast(lastClimbHit.point + Vector3.up * asset.climbStep + toWallDire * 0.1f, Vector3.down, out climbHit, asset.climbStep))
                    {
                        wallHeight = height - asset.climbStep + climbHit.point.y - lastClimbHit.point.y;
                        climbEdge = climbHit.point;
                        break;
                    }
                }
                if (wallHeight > asset.maxClimbHeight) return 0;
                if (wallHeight <= asset.midClimbHeight)
                {
                    if (!Physics.Raycast(lastClimbHit.point + Vector3.up * asset.climbStep + toWallDire * asset.climbOverDistance, Vector3.down, asset.climbStep))
                        return 2;
                    return 1;
                }
                else
                    return 3;

            }
            return 0;
        }


    }

}
