using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtend
{
    public static void FitImgSize(Image img,float width, float height)
    {
        float spriteWidth = img.sprite.rect.width;
        float spriteHeight = img.sprite.rect.height;
        float blockWidthHeightRatio = (float)width / height;
        float spriteWidthHeightRatio = spriteWidth / spriteHeight;
        if (blockWidthHeightRatio > spriteWidthHeightRatio)//框更宽
        {
            spriteHeight = height;
            spriteWidth = spriteHeight * spriteWidthHeightRatio;
        }
        else
        {
            spriteWidth = width;
            spriteHeight = spriteWidth / spriteWidthHeightRatio;
        }
        RectTransform imgRect = img.transform as RectTransform;
        SetSize(imgRect, new Vector2(spriteWidth, spriteHeight));
    }
    public static void SetSize(RectTransform rectTrans, Vector2 newSize)
    {
        Vector2 oldSize = rectTrans.rect.size;
        Vector2 offset = newSize - oldSize;
        rectTrans.sizeDelta = rectTrans.sizeDelta + offset;
    }

}