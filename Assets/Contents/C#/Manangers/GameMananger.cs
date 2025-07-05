using System.Collections;
using UnityEngine;

public class GameMananger : MonoBehaviour
{
    [SerializeField] private PointTouch c_pointTouch;
    [SerializeField] private BallController c_ballController;
    [SerializeField] private Progress c_progress;
    [SerializeField] private GameOver c_gameOver;

    public static GameMananger _get;

    void Start()
    {
        if (_get == null)
        {
            _get = this;
        }
        c_ballController.f_Init();
        c_pointTouch.f_Init();
        c_progress.f_Init();
        c_gameOver.f_Init();

        c_ballController.f_Activ();
    }

    public void f_DoneBallController()
    {
        c_progress.f_Activ();
        c_pointTouch.m_clik = false;
    }

    public void f_GameOver()
    {
        c_pointTouch.m_clik = false;
        c_gameOver.f_Activ();
        StartCoroutine("IEWait");
    }

    private IEnumerator IEWait()
    {
        yield return new WaitForSeconds(2);
        Start();
    }
}
