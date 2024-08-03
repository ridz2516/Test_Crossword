using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    public GameStates ScreenState;

    [SerializeField] private CanvasGroup m_CanvasGroup;

    public void OpenScreen(float i_Delay = 0.2f)
    {
        this.gameObject.SetActive(true);
        m_CanvasGroup.DOFade(1, i_Delay).From(0);
    }

    public void CloseScreen(float i_Delay = 0.2f)
    {
        m_CanvasGroup.DOFade(1, i_Delay).OnComplete(()=> this.gameObject.SetActive(false));
    }
}
