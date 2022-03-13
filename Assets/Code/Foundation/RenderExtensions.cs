using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

internal static class RenderExtensions
{
    #region Graphic
    internal static void SetColorLerp(this Graphic graphic, Color targetColor, float time = .1f)
    {
        CoroutineHandler.Start(ChangeColorLerp(graphic, targetColor, time));
    }
    internal static IEnumerator ChangeColorLerp(this Graphic graphic, Color targetColor, float time)
    {
        Color startColor = graphic.color;
        float timer = 0f;
        
        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            Color setColor = Color.Lerp(startColor, targetColor, timer / time);
            graphic.color = setColor;
            yield return null;
        }

        graphic.color = targetColor;
    }
    
    internal static IEnumerator MoveLerp(this Graphic graphic, Vector2 startPos, Vector2 endPos, float moveTime, Action<bool> ended)
    {
        float timer = 0f;

        while (timer < moveTime)
        {
            timer += Time.unscaledDeltaTime;
            Vector2 setPos = Vector2.Lerp(startPos, endPos, timer / moveTime);
            graphic.rectTransform.localPosition = setPos;
            yield return null;
        }
        
        ended.Invoke(true);
    }
    
    #endregion

    #region Image
    internal static void FillAmountLerp(this Image image, float value, float time = .1f)
    {
        CoroutineHandler.Start(ChangeFillAmountLerp(image, value, time));
    }
    internal static IEnumerator ChangeFillAmountLerp(this Image image, float targetValue, float time)
    {
        float startValue = image.fillAmount;
        float timer = 0f;
        
        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            float setValue = Mathf.Lerp(startValue, targetValue, timer / time);
            image.fillAmount = setValue;
            yield return null;
        }

        image.fillAmount = targetValue;
    }
    #endregion
    
}