using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowControls : MonoBehaviour
{
    // Referencias a los paneles
    public GameObject controlsPanel;
    public GameObject dialogPanel;

    // Textos de los diálogos
    public string[] dialogTexts;
    public TMP_Text dialogText;

    private int currentDialogIndex = 0;

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
        if (dialogPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextDialog();
        }
    }

    // Método para mostrar el siguiente diálogo
    private void ShowNextDialog()
    {
        if (currentDialogIndex < dialogTexts.Length)
        {
            dialogText.text = dialogTexts[currentDialogIndex];
            currentDialogIndex++;
        }
        else
        {
            dialogPanel.SetActive(false); // Ocultar el panel de diálogos cuando se terminen
        }
    }
}
