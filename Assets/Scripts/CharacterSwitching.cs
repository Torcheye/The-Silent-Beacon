using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CharacterSwitching : MonoBehaviour
{
    public bool startSwitching;
    public bool endSwitching;
    public Transform cameraTransform;

    [SerializeField] private bool isWinthinAngle;
    
    [SerializeField] private float switchStartDuration = 4;
    [SerializeField] private float switchMiddleDuration = 2;
    [SerializeField] private float switchEndDuration = 4;
    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private Transform lighthouseLightPosition;
    [SerializeField] private float lookingAngleThreshold = 15;
    
    private LiftGammaGain _lgg;
    private float _switchTimer;
    
    public void EnableAndStartSwitching(Transform ca)
    {
        cameraTransform = ca;
        enabled = true;
        startSwitching = true;
    }

    private void Start()
    {
        volumeProfile.TryGet<LiftGammaGain>(out _lgg);
        SetLift(0);
    }

    private void SetLift(float value)
    {
        _lgg.lift.value = new Vector4(0, 0, 0, value);
    }
    
    private float GetLift()
    {
        return _lgg.lift.value.w;
    }
    
    private IEnumerator WaitForMiddle()
    {
        yield return new WaitForSeconds(switchStartDuration);
        endSwitching = true;
        _switchTimer = 0;
    }
    
    private void Update()
    {
        if (startSwitching)
        {
            // if transform forward is facing the lighthouse light position
            isWinthinAngle = Vector3.Angle(cameraTransform.forward, lighthouseLightPosition.position - cameraTransform.position) <
                                  lookingAngleThreshold;
            if (!isWinthinAngle)
                return;

            if (!InputManager.Instance.MousePressed)
            {
                _switchTimer -= Time.deltaTime;
            }
            else
            {
                _switchTimer += Time.deltaTime;
                Debug.Log($"Switching in progress: {_switchTimer / switchStartDuration}");
            }
            _switchTimer = Mathf.Clamp(_switchTimer, 0, switchStartDuration);
            SetLift(Mathf.Lerp(0, 1, _switchTimer / switchStartDuration));
            
            if (_switchTimer >= switchStartDuration)
            {
                // switch completed
                startSwitching = false;
                GameManager.Instance.CompleteStartSwitching();
                StartCoroutine(WaitForMiddle());
                return;
            }
        }
        
        if (endSwitching)
        {
            if (_switchTimer < switchEndDuration)
            {
                _switchTimer += Time.deltaTime;
                SetLift(Mathf.Lerp(1, 0, _switchTimer / switchEndDuration));
            }
            else
            {
                // switch completed
                endSwitching = false;
                GameManager.Instance.CompleteEndSwitching();
                enabled = false;
            }
        }
    }
}