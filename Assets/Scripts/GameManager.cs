using System;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public Chapter CurrentChapter { get; private set; }
    
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private PlayableAsset[] cutscenes;
    [SerializeField] private BoatController boatController;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetChapter(Chapter.Start);
    }

    public void StartGame()
    {
        UIManager.Instance.ToggleStartScreen(false);
        SetChapter(Chapter.C1A1);
    }

    private void LateUpdate()
    {
        if (playableDirector.isActiveAndEnabled && playableDirector.state != PlayState.Playing)
        {
            switch (CurrentChapter)
            {
                case Chapter.C1A1:
                    SetChapter(Chapter.C1A2);
                    break;
                case Chapter.C1A4:
                    SetChapter(Chapter.C2A1);
                    break;
                case Chapter.C2A2:
                    SetChapter(Chapter.C2A3);
                    break;
                case Chapter.C3A1:
                    SetChapter(Chapter.C3A2);
                    break;
                default:
                    Debug.LogError("PlayableDirector finished but not at the end of a cutscene!");
                    return;
            }
        }
    }

    public void SetChapter(Chapter chapter)
    {
        CurrentChapter = chapter;
        Debug.Log($"Start Chapter: {chapter}");

        switch (chapter)
        {
            case Chapter.Start:
                playableDirector.enabled = false;
                boatController.ToggleCamera(false);
                boatController.UnregisterInput();
                break;
            case Chapter.C1A1:
                playableDirector.enabled = true;
                boatController.ToggleCamera(true);
                playableDirector.Play(cutscenes[0]);
                break;
            case Chapter.C1A2:
                playableDirector.enabled = false;
                boatController.RegisterInput();
                break;
            case Chapter.C1A3:
                break;
            case Chapter.C1A4:
                break;
            case Chapter.C2A1:
                break;
            case Chapter.C2A2:
                break;
            case Chapter.C2A3:
                break;
            case Chapter.C3A1:
                break;
            case Chapter.C3A2:
                break;
            case Chapter.C3A3:
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(chapter), chapter, null);
        }
    }
}