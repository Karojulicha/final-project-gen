using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowControls : MonoBehaviour
{
    // Referencias a los paneles
    public GameObject controlsPanel;
    public GameObject dialogPanel;
    public DialogsInfo[] dialogs;
    public TMP_Text dialogText;

    private int currentDialogIndex = 0;

    public ManagementCharacter managementCharacter;

    void Start()
    {
        foreach(var dialog in dialogs){
            dialog.dialog = ManagementData.Instance.GetDialog(dialog.id, ManagementData.Instance.saveData.configurationsInfo.currentLanguage.ToString());
        }
    }

    // Método para cerrar el panel de controles
    public void CloseControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }

        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
            ShowNextDialog(); // Mostrar el primer diálogo
        }
    }

    void Update()
    {
        if (dialogPanel.activeSelf && managementCharacter.managementCharacterInputs.characterActionsInfo.nextLine.triggered)
        {
            ShowNextDialog();
        }
    }

    // Método para mostrar el siguiente diálogo
    private void ShowNextDialog()
    {
        if (currentDialogIndex < dialogs.Length)
        {
            dialogText.text = dialogs[currentDialogIndex].dialog;
            currentDialogIndex++;
        }
        else
        {
            dialogPanel.SetActive(false);
            managementCharacter.canPlay = true;
        }
    }
    [System.Serializable] public class DialogsInfo{
        public int id;
        public string dialog;
        public DialogsInfo(int id, string dialog)
        {
            this.id = id;
            this.dialog = dialog;
        }
    }
}
