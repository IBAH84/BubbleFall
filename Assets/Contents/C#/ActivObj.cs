using System.Collections;
using UnityEngine;

public class ActivObj : MonoBehaviour
{
    public void f_Init()
    {
        transform.localScale = new Vector3(0, 0, 1);
        gameObject.SetActive(false);
    }

    public void f_Activ(bool _activ)
    {
        if (gameObject.activeSelf == _activ)
        {
            return;
        }
        gameObject.SetActive(true);
        StopCoroutine("IEActiv");
        StartCoroutine("IEActiv", _activ);
    }

    public virtual IEnumerator IEActiv(bool _activ)
    {
        AnimationCurve _scale = A_Curve.f_GetCurve(transform.localScale.x, _activ ? 1 : 0);

        float _step = 0;
        while (_step < 1)
        {
            _step += 3 * Time.deltaTime;
            transform.localScale = new Vector3(_scale.Evaluate(_step), _scale.Evaluate(_step), 1);
            yield return null;
        }
        gameObject.SetActive(_activ);
    }
}
