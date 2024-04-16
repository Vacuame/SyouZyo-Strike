using System.Collections;
using UnityEngine;


public static class UIExtend
{
    public static void SetSize(RectTransform rectTrans, Vector2 newSize)
    {
        Vector2 oldSize = rectTrans.rect.size;
        Vector2 offset = newSize - oldSize;
        rectTrans.sizeDelta = rectTrans.sizeDelta + offset;
    }

}