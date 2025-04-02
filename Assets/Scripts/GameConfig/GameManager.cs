using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Animator OpenCloseScene;
    public TypeDevice _currentDevice;
    public event Action<TypeDevice> OnDeviceChanged;
    public TypeDevice currentDevice
    {
        get => _currentDevice;
        set
        {
            if (_currentDevice != value)
            {
                _currentDevice = value;
                OnDeviceChanged?.Invoke(_currentDevice);
            }
        }
    }
    Coroutine fadeIn;
    Coroutine fadeOut;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _ = ValidateDevice();
    }
    public void ChangeSceneSelector(TypeScene typeScene)
    {
        switch (typeScene)
        {
            case TypeScene.OptionsScene:
                SceneManager.LoadScene("OptionsScene", LoadSceneMode.Additive);
                break;
            case TypeScene.GameOver:
                SceneManager.LoadScene(TypeScene.GameOver.ToString(), LoadSceneMode.Additive);
                break;
            case TypeScene.RestartScene:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                if (fadeOut != null)
                {
                    StopCoroutine(fadeOut);
                }
                fadeOut = StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(SceneManager.GetActiveScene().buildIndex));
                break;
            default:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                if (fadeOut != null)
                {
                    StopCoroutine(fadeOut);
                }
                fadeOut = StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
        }
    }
    public IEnumerator FadeIn()
    {
        if (fadeOut != null)
        {
            StopCoroutine(FadeOut());
        }
        float decibelsMaster = 20 * Mathf.Log10(ManagementData.Instance.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100);
        float currentVolumen = 0;
        float volume = 0;
        if (ManagementData.Instance.audioMixer.GetFloat(ManagementOptions.TypeSound.Master.ToString(), out volume))
        {
            currentVolumen = volume;
        }
        else
        {
            currentVolumen = -80f;
        }
        while (currentVolumen < decibelsMaster)
        {
            if (ManagementData.Instance.saveData.configurationsInfo.soundConfiguration.isMute) break;
            currentVolumen++;
            ManagementData.Instance.audioMixer.SetFloat(ManagementOptions.TypeSound.Master.ToString(), currentVolumen);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public IEnumerator ChangeScene(TypeScene typeScene)
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(2);
        if (typeScene != TypeScene.Exit)
        {
            SceneManager.LoadScene(typeScene.ToString());
        }
        else
        {
            Application.Quit();
        }
        OpenCloseScene.SetBool("Out", false);
        if (fadeOut != null)
        {
            StopCoroutine(fadeOut);
        }
        StartCoroutine(FadeIn());
    }
    public IEnumerator ChangeScene(int index)
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(2);
        if (SceneManager.GetSceneAt(index).name != TypeScene.Exit.ToString())
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Application.Quit();
        }
        OpenCloseScene.SetBool("Out", false);
        if (fadeOut != null)
        {
            StopCoroutine(fadeOut);
        }
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeOut()
    {
        if (fadeIn != null)
        {
            StopCoroutine(FadeIn());
        }
        float decibelsMaster = 20 * Mathf.Log10(ManagementData.Instance.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100);
        while (decibelsMaster > -80)
        {
            if (ManagementData.Instance.saveData.configurationsInfo.soundConfiguration.isMute) break;
            decibelsMaster -= 1;
            ManagementData.Instance.audioMixer.SetFloat(ManagementOptions.TypeSound.Master.ToString(), decibelsMaster);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public void PlayASound(AudioClip audioClip)
    {
        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }
    public void PlayASound(AudioClip audioClip, float initialRandomPitch)
    {
        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.pitch = UnityEngine.Random.Range(initialRandomPitch - 0.1f, initialRandomPitch + 0.1f);
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }
    internal void SetAudioMixerData()
    {
        ManagementData.Instance.SetAudioMixerData();
    }
    public void SetInitialDevice()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Touchscreen.current != null)
        {
            currentDevice = TypeDevice.MOBILE;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer ||
             Application.platform == RuntimePlatform.OSXPlayer ||
             Application.platform == RuntimePlatform.LinuxPlayer)
        {
            currentDevice = TypeDevice.PC;
        }
        else
        {
            currentDevice = TypeDevice.GAMEPAD;
        }
    }
    async Awaitable ValidateDevice()
    {
        while (true)
        {
            if (ValidateMobileInput())
            {
                currentDevice = TypeDevice.MOBILE;
            }
            else if (ValidateGamepadInput())
            {
                currentDevice = TypeDevice.GAMEPAD;
            }
            else if (ValidatePcInput())
            {
                currentDevice = TypeDevice.PC;
            }
            await Awaitable.WaitForSecondsAsync(0.01f);
        }
    }
    bool ValidateMobileInput()
    {
        return Touchscreen.current != null;
    }
    bool ValidatePcInput()
    {
        return Keyboard.current.anyKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame ||
            Mouse.current.scroll.ReadValue() != Vector2.zero ||
            Mouse.current.delta.ReadValue() != Vector2.zero;
    }
    bool ValidateGamepadInput()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null) return false;
        bool currentDeviceIsGamepad = Gamepad.current != null;
        bool validateAnyGamepadInput = gamepad.buttonSouth.wasPressedThisFrame ||
               gamepad.buttonNorth.wasPressedThisFrame ||
               gamepad.buttonEast.wasPressedThisFrame ||
               gamepad.buttonWest.wasPressedThisFrame ||
               gamepad.leftStick.ReadValue() != Vector2.zero ||
               gamepad.rightStick.ReadValue() != Vector2.zero ||
               gamepad.dpad.ReadValue() != Vector2.zero ||
               gamepad.leftTrigger.wasPressedThisFrame ||
               gamepad.rightTrigger.wasPressedThisFrame;
        return currentDeviceIsGamepad && validateAnyGamepadInput;
    }
    public enum TypeScene
    {
        HomeScene = 0,
        OptionsScene = 1,
        GameScene = 2,
        Exit = 3,
        GameOver = 4,
        WinScene = 5,
        RestartScene = 6,
    }
    public enum TypeDevice
    {
        None,
        PC,
        GAMEPAD,
        MOBILE,
    }
}
