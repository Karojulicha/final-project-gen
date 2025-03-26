using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ManagementLanguage : MonoBehaviour
{
    private TMP_Text dialogText;
    public int id = 0;
    public ManagementData.TypeLanguage currentLanguage = ManagementData.TypeLanguage.English;
    void OnEnable()
    {
        dialogText = GetComponent<TMP_Text>();
        ValidateChangeText();
    }

    public void ValidateChangeText()
    {
        if (dialogText == null) dialogText = GetComponent<TMP_Text>();
        if (ManagementData.Instance.csvData.Count != 0)
        {
            currentLanguage = ManagementData.Instance.saveData.configurationsInfo.currentLanguage;
            dialogText.text = GetDialog(id, ManagementData.Instance.saveData.configurationsInfo.currentLanguage.ToString());
        }
    }
    public string GetDialog(int id, string language)
    {
        int languageIndex = 0;
        for (int i = 0; i < ManagementData.Instance.csvData[0].Count(); i++)
        {
            if (ManagementData.Instance.csvData[0][i] == language)
            {
                languageIndex = i;
                break;
            }
        }
        if (languageIndex != 0) return ManagementData.Instance.csvData[id][languageIndex];
        return null;
    }
}