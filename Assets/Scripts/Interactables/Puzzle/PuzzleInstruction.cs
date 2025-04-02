using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PuzzleHandler : MonoBehaviour
{
    public GameObject puzzlePanel; // Panel del puzzle
    public TMP_InputField puzzleInput; // Campo de texto para la respuesta
    public Button checkButton; // Botón de comprobar respuesta
    
    private string correctAnswer = "pasado"; // Respuesta correcta

    void Start()
    {
        puzzlePanel.SetActive(false); // Asegurarse de que el panel empieza oculto
        checkButton.onClick.AddListener(CheckAnswer); // Asignar la función al botón
    }

    public void OpenPuzzle()
    {
        puzzlePanel.SetActive(true); // Mostrar el puzzle
    }

    public void CheckAnswer()
    {
        if (puzzleInput.text.ToLower() == correctAnswer)
        {
            puzzlePanel.SetActive(false); // Cerrar el panel solo si la respuesta es correcta
        }
        else
        {
            Debug.Log("Respuesta incorrecta. Inténtalo de nuevo."); // Mensaje en la consola
        }
    }
}