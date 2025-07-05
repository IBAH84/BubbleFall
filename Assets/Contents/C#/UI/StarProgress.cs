using System.Collections;
using UnityEngine;

public class StarProgress : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 200 * Time.deltaTime);
    }

    public void f_Rect()
    {
        StartCoroutine("IEReact");
        StartCoroutine("IEReact");
    }

    private IEnumerator IEReact()
    {
        AnimationCurve _scale = A_Curve.f_GetCurve(transform.localScale.x, 1.2f, 1);

        float _step = 0;
        while (_step < 1)
        {
            _step += 3 * Time.deltaTime;
            transform.localScale = new Vector3(_scale.Evaluate(_step), _scale.Evaluate(_step), 1);
            yield return null;
        }
    }
}
