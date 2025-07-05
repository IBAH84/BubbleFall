using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMesh mt_text;

    public void f_Init()
    {
        gameObject.SetActive(false);
        mt_text.fontSize = 0;
    }

    public void f_Activ()
    {
        gameObject.SetActive(true);
        StartCoroutine("IEActiv");
    }

    public IEnumerator IEActiv()
    {
        AnimationCurve _scale = A_Curve.f_GetCurve(0, 80, 70);

        float _step = 0;
        while (_step < 1)
        {
            _step += 3 * Time.deltaTime;
            mt_text.fontSize = (int)_scale.Evaluate(_step);
            yield return null;
        }
    }
}
