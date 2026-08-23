using UnityEngine;
using TMPro;
using System.Collections;

public class DamageTMP : MonoBehaviour
{
    GameObject Critical;
    private TextMeshProUGUI m_text;
    private RectTransform rectTransform;

    private Vector2 velocity;
    private float gravity = -1000.0f;
    public float lifeTime = 1.0f;

    private Color textColor;
    private void Awake()
    {
        Critical = transform.Find("Critical").gameObject;
        rectTransform = GetComponent<RectTransform>();
        m_text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initalize(Transform parent, Vector3 pos, string temp, Color color, bool isCritical = false)
    {
        Critical.SetActive(isCritical);
        transform.SetParent(parent);

        m_text.text = temp;
        m_text.color = color;

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        rectTransform.position = screenPosition;

        textColor = m_text.color;
        velocity = new Vector2(Random.Range(-50.0f, 50.0f), Random.Range(150.0f, 250.0f));

        StartCoroutine(MoveAndFade());
    }

    IEnumerator MoveAndFade()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < lifeTime)
        {
            velocity.y += gravity * Time.deltaTime;
            rectTransform.anchoredPosition += velocity * Time.deltaTime;

            textColor.a = Mathf.Lerp(1.0f, 0.0f, elapsedTime/lifeTime);
            m_text.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        MANAGER.poolManager.m_Pool_Dictionary["DamageFont"].Return(gameObject);
    }
}
