using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public float fadeDuration = 2;
    public float appearDuration = 2;
    public float hintFadeDuration = 1;
    public float hintAppearDuration = 1;
    public CanvasGroup startScreen;
    public GameObject intertitle;
    public TMP_Text intertitleText;
    public CanvasGroup hint;
    public TMP_Text hintText;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void ShowHint(string text)
    {
        if (hint.alpha > 0)
        {
            Debug.LogError("Hint already showing!");
            return;
        }
        
        hintText.text = text;
        hint.DOFade(1, hintAppearDuration);
    }
    
    public void HideHint()
    {
        if (hint.alpha == 0)
        {
            Debug.LogError("Hint already hidden!");
            return;
        }
        
        hint.DOFade(0, hintFadeDuration);
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