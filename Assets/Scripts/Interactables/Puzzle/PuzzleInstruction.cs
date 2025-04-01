using UnityEngine;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePanel; // Panel del puzzle
    public TMP_InputField puzzleInput; // Campo de texto para la respuesta
    public GameObject keyObject; // Objeto llave
    private string correctAnswer = "pasado";

    void Update()
    {
        // Detectar clic derecho sobre el objeto llave
        if (Input.GetMouseButtonDown(1)) // 1 = Clic derecho
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == keyObject)
                {
                    OpenPuzzlePanel();
                }
            }
        }
    }

    public void OpenPuzzlePanel()
    {
        puzzlePanel.SetActive(true);
    }

    public void CheckAnswer()
    {
        if (puzzleInput.text.ToLower() == correctAnswer)
        {
            puzzlePanel.SetActive(false);
        }
    }
}