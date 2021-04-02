using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour
{

    [SerializeField] Color[] colors;
    [SerializeField] float LerpSpeed;
    [SerializeField] float Iterations;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LerpColor(0));
    }

    IEnumerator LerpColor(int _index)
    {
        float t = 0;
        while(t < 1)
        {
            yield return new WaitForSeconds(LerpSpeed/Iterations);
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, colors[_index], t);
            t += 1/Iterations;
        }
        _index++;
        StartCoroutine(LerpColor(_index < colors.Length ? _index : 0));
    }

}
