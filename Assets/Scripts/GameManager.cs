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
    [SerializeField] private FPSController keeperController;
    [SerializeField] private CharacterSwitching characterSwitching;
    
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
        // cutscene switching
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

    public void CompleteStartSwitching()
    {
        switch (CurrentChapter)
        {
            case Chapter.C1A2:
                SetChapter(Chapter.C1A3);
                break;
            case Chapter.C1A4:
                SetChapter(Chapter.C2A1);
                break;
            case Chapter.C2A3:
                SetChapter(Chapter.C3A1);
                break;
            default:
                Debug.LogError("Wrong chapter to complete Start Switching!");
                return;
        }
    }
    
    public void CompleteEndSwitching()
    {
        switch (CurrentChapter)
        {
            case Chapter.C1A3:
                InputManager.Instance.ToggleMovementInput(true);
                InputManager.Instance.ToggleMouseMoveInput(true);
                UIManager.Instance.ShowIntertitle("又该下楼搬油了...");
                break;
            case Chapter.C2A1:
                
                break;
            case Chapter.C3A1:
                
                break;
            default:
                Debug.LogError("Wrong chapter to complete End Switching!");
                return;
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
                
                InputManager.Instance.ToggleMovementInput(false);
                InputManager.Instance.ToggleMouseMoveInput(false);
                
                keeperController.gameObject.SetActive(false);
                break;
            case Chapter.C1A1:
                boatController.ToggleCamera(true);
                
                playableDirector.enabled = true;
                playableDirector.Play(cutscenes[0]);
                break;
            case Chapter.C1A2:
                playableDirector.enabled = false;
                
                InputManager.Instance.ToggleMovementInput(true);
                InputManager.Instance.ToggleMouseMoveInput(true);
                
                boatController.ToggleSeaRocks(true);
                characterSwitching.EnableAndStartSwitching(boatController.CameraTransform);
                
                UIManager.Instance.ShowIntertitle("向着光开！");
                break;
            case Chapter.C1A3:
                boatController.gameObject.SetActive(false);
                keeperController.gameObject.SetActive(true);
                
                InputManager.Instance.ToggleMovementInput(false);
                InputManager.Instance.ToggleMouseMoveInput(false);
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