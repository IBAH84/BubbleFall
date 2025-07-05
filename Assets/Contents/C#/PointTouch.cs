using UnityEngine;

public class PointTouch : MonoBehaviour
{
    [SerializeField] private ActivObj[] c_activ;
    [SerializeField] private Transform mt_pointParent;
    [SerializeField] private BallConteiner c_ballConteiner;
    [SerializeField] private Lazer c_lazer;
    private float m_timeWait;
    [HideInInspector] public bool m_clik;
    public static PointTouch _get;

    public void f_Init()
    {
        if (_get == null)
        {
            _get = this;
        }
        m_clik = true;
        c_ballConteiner.f_Init();
        f_ResetPoints(false);
    }

    void Update()
    {
        if (m_clik)
        {
            return;
        }
        if (!Input.GetMouseButton(0))
        {
            f_PointWork();
        }

        if (Input.GetMouseButtonDown(0))
        {
            f_ResetPoints(true);
        }
    }

    private void OnMouseDown()
    {
        if (m_clik)
        {
            return;
        }
        m_clik = true;
        c_ballConteiner.f_Rotate();
    }

    private void OnMouseUp()
    {
        m_clik = false;
    }

    private void f_PointWork()
    {
        if (m_timeWait > 0)
        {
            m_timeWait -= 1 * Time.deltaTime;
            if (m_timeWait <= 0)
            {
                mt_pointParent.localEulerAngles = new Vector3(0, 0, 0);
                for (int i = 0; i < c_activ.Length; i++)
                {
                    c_activ[i].f_Activ(true);
                }
            }
        }
        else
        {
            mt_pointParent.Rotate(Vector3.forward * 100 * Time.deltaTime);
        }
    }

    private void f_ResetPoints(bool _clik)
    {
        m_timeWait = 5;

        if (_clik)
        {
            for (int i = 0; i < c_activ.Length; i++)
            {
                c_activ[i].f_Activ(false);
            }
            return;
        }
        for (int i = 0; i < c_activ.Length; i++)
        {
            c_activ[i].f_Init();
        }
    }
}
