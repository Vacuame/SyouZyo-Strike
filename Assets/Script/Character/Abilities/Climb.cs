using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Climb;

public class Climb : AbstractAbility
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
        public ClimbSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            
        }

        public override void ActivateAbility(params object[] args)
        {
            
        }

        public override void CancelAbility()
        {
            
        }

        public override void EndAbility()
        {
            
        }

        protected override void SustainedTick()
        {
            climbable = ClimbCheck();
            if(climbable!=0)
                GameUI.Instance?.SetText("tip", "Press F to climb");
            else
                GameUI.Instance?.SetText("tip", null);
        }
        private int climbable;
        [SerializeField] private float minClimbHeight, midClimbHeight, maxClimbHeight, climbStep;
        [SerializeField] private float climbCheckDistance, climbOverDistance;
        private Vector3 climbEdge, toWallDire;
        private float wallHeight;
        [SerializeField] private LayerMask climbLayer;
        [SerializeField] private float climbTowardAngle;
        private int ClimbCheck()
        {
            RaycastHit climbHit;
            if (Physics.Raycast(owner.transform.position + Vector3.up * minClimbHeight, owner.transform.forward, out climbHit, climbCheckDistance, climbLayer))
            {
                toWallDire = -climbHit.normal;
                if (Vector3.Angle(owner.transform.forward, toWallDire) > climbTowardAngle) return 0;

                wallHeight = maxClimbHeight + climbStep;

                RaycastHit lastClimbHit = climbHit;

                for (float height = minClimbHeight + climbStep; height <= maxClimbHeight + climbStep; height += climbStep)
                {
                    if (Physics.Raycast(owner.transform.position + Vector3.up * height, toWallDire, out climbHit, climbCheckDistance, climbLayer))
                    {
                        lastClimbHit = climbHit;
                    }
                    else if (Physics.Raycast(lastClimbHit.point + Vector3.up * climbStep + toWallDire * 0.1f, Vector3.down, out climbHit, climbStep))
                    {
                        wallHeight = height - climbStep + climbHit.point.y - lastClimbHit.point.y;
                        climbEdge = climbHit.point;
                        break;
                    }
                }
                if (wallHeight > maxClimbHeight) return 0;
                if (wallHeight <= midClimbHeight)
                {
                    if (!Physics.Raycast(lastClimbHit.point + Vector3.up * climbStep + toWallDire * climbOverDistance, Vector3.down, climbStep))
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
