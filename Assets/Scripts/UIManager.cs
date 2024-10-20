using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public float fadeDuration = 2;
    public float appearDuration = 2;
    public CanvasGroup startScreen;
    public GameObject intertitle;
    public TMP_Text intertitleText;
    
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
    
    public void ShowIntertitle(string text, float duration = 3)
    {
        intertitleText.text = text;
        intertitle.SetActive(true);
        StartCoroutine(HideIntertitle(duration));
    }
    
    private IEnumerator HideIntertitle(float delay)
    {
        yield return new WaitForSeconds(delay);
        intertitle.SetActive(false);
    }
}