using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;


public class NamePromptController : MonoBehaviour
{
    [Header("Title input")]
    [SerializeField] private TitleScreenManager titleScreenManager;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_Text textPrompt;

    [SerializeField] private int minLength = 1;
    [SerializeField] private int maxLength = 10;
    void OnEnable()
    {
        if (nameInput != null)
        {
            nameInput.characterLimit = maxLength;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Submit();
        }
    } 
    private void Submit()
    {
        Debug.Log("[Start Screen] Submit started!");
        if(titleScreenManager == null || nameInput == null) return;
        string playerName = nameInput.text.Trim();

        if (playerName.Length < minLength || playerName.Length > maxLength)
        {
            if (textPrompt != null) textPrompt.text = "Please enter a valid name";
            nameInput.ActivateInputField();
            nameInput.Select();
            return;
        }
        titleScreenManager.OnNameSubmitted(playerName);
    }
}