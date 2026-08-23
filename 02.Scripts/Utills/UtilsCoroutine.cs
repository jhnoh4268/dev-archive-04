using System;
using System.Collections;
using UnityEngine;

public class UtilsCoroutine
{
    public static IEnumerator Delay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static IEnumerator ParabolMove(Transform obj, Vector3 start, Vector3 end, float height, float duration, Action action = null)
    {
        float time = 0.0f;
        while (time < duration)
        {
            float t = time / duration;

            //수평보간
            Vector3 flatpos = Vector3.Lerp(start, end, t);

            //위로 솟는 y값
            float y = Mathf.Sin(Mathf.PI * t) * height;

            obj.position = new Vector3(flatpos.x, flatpos.y + y, flatpos.z);

            time += Time.deltaTime;

            yield return null;
        }

        action?.Invoke();
        obj.position = end;
    }
}
