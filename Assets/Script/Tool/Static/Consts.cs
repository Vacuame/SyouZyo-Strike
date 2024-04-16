using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public readonly static Vector2 NullV2 = new Vector2(-99999, -99999);
    public readonly static Vector3 NullV3 = new Vector3(-99999, -99999, -99999);

    public static class BodyPart
    {
        public readonly static string Head = "Head";
        public readonly static string Body = "Body";
        public readonly static string LeftArm = "LeftArm";
        public readonly static string RightArm = "RightArm";
    }

    public static class Event
    {
        public readonly static string Hit = "Hit";
    }

}
