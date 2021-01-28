using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPing : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float disappearTimer = 0;
    private float disappearTimerMax = 1;
    private Color color = new Color(1, 1, 1, 1f);

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        disappearTimer += Time.deltaTime;

        color.a = Mathf.Lerp(disappearTimerMax, 0, disappearTimer / disappearTimerMax);
        spriteRenderer.color = color;

        if (disappearTimer >= disappearTimerMax)
        {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void SetDisappearTimer(float disappearTimerMax)
    {
        this.disappearTimerMax = disappearTimerMax;
        disappearTimer = 0;
    }
}
