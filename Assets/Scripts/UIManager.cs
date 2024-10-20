using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public float fadeDuration = 2;
    public float appearDuration = 2;
    public CanvasGroup startScreen;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void ToggleStartScreen(bool toggle)
    {
        
        startScreen.DOFade(toggle ? 1 : 0, toggle ? appearDuration : fadeDuration);
        startScreen.interactable = toggle;
        startScreen.blocksRaycasts = toggle;
    }
}