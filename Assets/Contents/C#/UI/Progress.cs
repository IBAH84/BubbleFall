using System.Collections;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField] private StarProgress c_star;
    [SerializeField] private TextMesh mt_text;
    private int m_count;
    public static Progress _get;

    public void f_Init()
    {
        if (_get == null)
        {
            _get = this;
        }
        m_count = 0;
        f_SetCount(m_count);
        transform.localPosition = new Vector3(0, 1.2f, transform.localPosition.z);
    }

    public void f_Activ()
    {
        StartCoroutine("IEActiv");
    }

    private IEnumerator IEActiv()
    {
        AnimationCurve _muveY = A_Curve.f_GetCurve(transform.localPosition.y, 0.8f, 0.9f);
        float _step = 0;
        while (_step < 1)
        {
            _step += 1 * Time.deltaTime;
            transform.localPosition = new Vector3(0, _muveY.Evaluate(_step), transform.localPosition.z);
            yield return null;
        }
    }

    public void f_AddProgress()
    {
        m_count += 10;
        StopCoroutine("IEAddCount");
        StartCoroutine("IEAddCount");
    }

    private IEnumerator IEAddCount()
    {
        int _count;
        int.TryParse(mt_text.text, out _count);
        AnimationCurve _addCount = A_Curve.f_GetCurve(_count, m_count);
        float _step = 0;
        while (_step < 1)
        {
            _step += 2 * Time.deltaTime;
            f_SetCount((int)_addCount.Evaluate(_step));
            yield return null;
        }
    }

    private void f_SetCount(int _count)
    {
        mt_text.text = _count.ToString();
    }

    public Transform f_GetStar() { return c_star.transform; }
}
