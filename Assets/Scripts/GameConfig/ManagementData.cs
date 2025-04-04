using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Audio;
using System;
using System.Linq;

public class ManagementData : MonoBehaviour
{
    public static ManagementData Instance {get; private set;}
    private string nameSaveData = "SaveData.json";
    public SaveDataInfo saveData = new SaveDataInfo();
    public AudioMixer audioMixer;
    public List<string[]> csvData = new List<string[]>();
    private void Awake()
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
        LoadData();
    }
    void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Language/Language");
        string[] lines = csvFile.text.Split('\n');
        List<String[]> textData = new List<string[]>();
        foreach (string line in lines)
        {
            string[] columns = line.Split(';');
            textData.Add(columns);
        }
        csvData = textData;
    }
    public string GetDialog(int id, string language)
    {
        int languageIndex = 0;
        for (int i = 0; i < csvData[0].Length; i++)
        {
            if (csvData[0][i] == language)
            {
                languageIndex = i;
                break;
            }
        }
        if (languageIndex != 0) return csvData[id][languageIndex];
        return null;
    }
    public void LoadData()
    {
        GameManager.Instance.SetInitialDevice();
        CheckFileExistance(DataPath());
        ReadDataFromJson();
        LoadCSV();
        if (GameManager.Instance.currentDevice == GameManager.TypeDevice.PC) SetResolutionData();
        Invoke("SetAudioMixerData", 0.1f);
    }
    [NaughtyAttributes.Button]
    public void SaveGameData()
    {
        WriteDataToJson();
    }
    public void SetStartingData()
    {
        SaveDataInfo dataInfo = new SaveDataInfo();
        dataInfo.configurationsInfo.currentLanguage = TypeLanguage.English;
        SetStartingDataSound(dataInfo);
        SetStartingPlayerData(dataInfo);
        SetStartingResolution(dataInfo);
        saveData = dataInfo;
        SaveGameData();
    }

    private void SetStartingResolution(SaveDataInfo dataInfo)
    {
        Resolution[] resolutions = Screen.resolutions;
        Array.Reverse(resolutions);
        foreach (Resolution res in resolutions){
            dataInfo.configurationsInfo.resolutionConfiguration.allResolutions.Add(new ResolutionsInfo(res.width, res.height));
        }
        Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
        dataInfo.configurationsInfo.resolutionConfiguration.isFullScreen = true;
        dataInfo.configurationsInfo.resolutionConfiguration.currentResolution = new ResolutionsInfo(
            resolutions[0].width,
            resolutions[0].height);
    }

    public void SetStartingPlayerData(SaveDataInfo dataInfo)
    {

    }
    public void SetStartingDataSound(SaveDataInfo dataInfo)
    {
        dataInfo.configurationsInfo.soundConfiguration.MASTERValue = 25;
        dataInfo.configurationsInfo.soundConfiguration.BGMalue = 25;
        dataInfo.configurationsInfo.soundConfiguration.SFXalue = 25;
    }
    public void SetAudioMixerData()
    {
        float decibelsMaster = 20 * Mathf.Log10(saveData.configurationsInfo.soundConfiguration.MASTERValue / 100);
        float decibelsBGM = 20 * Mathf.Log10(saveData.configurationsInfo.soundConfiguration.BGMalue / 100);
        float decibelsSFX = 20 * Mathf.Log10(saveData.configurationsInfo.soundConfiguration.SFXalue / 100);

        if (saveData.configurationsInfo.soundConfiguration.MASTERValue == 0)
        {
            decibelsMaster = -80;
        }
        if (saveData.configurationsInfo.soundConfiguration.BGMalue == 0)
        {
            decibelsBGM = -80;
        }
        if (saveData.configurationsInfo.soundConfiguration.SFXalue == 0)
        {
            decibelsSFX = -80;
        }
        audioMixer.SetFloat(ManagementOptions.TypeSound.BGM.ToString(), decibelsBGM);
        audioMixer.SetFloat(ManagementOptions.TypeSound.SFX.ToString(), decibelsSFX);
        audioMixer.SetFloat(ManagementOptions.TypeSound.Master.ToString(), saveData.configurationsInfo.soundConfiguration.isMute ? -80 : decibelsMaster);
        SaveGameData();
    }
    public void SetResolutionData()
    {
        Screen.SetResolution(
            saveData.configurationsInfo.resolutionConfiguration.currentResolution.width,
            saveData.configurationsInfo.resolutionConfiguration.currentResolution.height,
            saveData.configurationsInfo.resolutionConfiguration.isFullScreen);
    }
    public void CheckFileExistance(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            SetStartingData();
            string dataString = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, dataString);
        }
    }
    public void ReadDataFromJson()
    {
        string dataString;
        string jsonFilePath = DataPath();
        dataString = File.ReadAllText(jsonFilePath);
        saveData = JsonUtility.FromJson<SaveDataInfo>(dataString);
    }

    void WriteDataToJson()
    {
        string dataString;
        string jsonFilePath = DataPath();
        dataString = JsonUtility.ToJson(saveData);
        File.WriteAllText(jsonFilePath, dataString);
    }
    string DataPath()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            return Path.Combine(Application.persistentDataPath, nameSaveData);
        }
        return Path.Combine(Application.streamingAssetsPath, nameSaveData);
    }
    [Serializable]  public class SaveDataInfo
    {
        public GameInfo gameInfo = new GameInfo();
        public ConfigurationsInfo configurationsInfo = new ConfigurationsInfo();
    }
    [Serializable]  public class GameInfo
    {
        public CharacterInfo characterInfo = new CharacterInfo();
    }
    [Serializable]  public class CharacterInfo
    {
        public bool isInitialize = false;
        public string characterSelectedName;
    }

    [Serializable]  public class ConfigurationsInfo
    {
        public TypeLanguage currentLanguage = TypeLanguage.English;
        public ResolutionConfiguration resolutionConfiguration = new ResolutionConfiguration();
        public SoundConfiguration soundConfiguration = new SoundConfiguration();
    }
    [Serializable]  public class ResolutionConfiguration
    {
        public bool isFullScreen = false;
        public List<ResolutionsInfo> allResolutions = new List<ResolutionsInfo>();
        public ResolutionsInfo currentResolution;
    }
    [Serializable]  public class SoundConfiguration
    {
        public bool isMute = false;
        public float MASTERValue;
        public float BGMalue;
        public float SFXalue;
    }
    [Serializable]  public class ResolutionsInfo
    {
        public int width = 0;
        public int height = 0;
        public ResolutionsInfo(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
    public enum TypeLanguage
    {
        English = 0,
        Español = 1,
        Português = 2,
        Français = 3,
        Deutsch = 4,
        Русский = 5,
        Italiano = 6,
    }
}
