using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public static DialogManager instance { get; private set; }
    [Header("Dialog Reference")]
    [SerializeField] private DialogDatabaseSO dialogDatabase;

    [Header("UI Reference")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private Image protraitImage;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button NextButton;

    [Header("Dialog Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool useTypewriterEffect = true;

    [Header("DialogChoices")]
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private DialogSO currentDialog;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (dialogDatabase != null)
        {
            dialogDatabase.Initailize();
        }
        else
        {
            Debug.LogError("Dialog Database is not assinged to Dialog Manager");
        }
        if (NextButton != null)
        {
            NextButton.onClick.AddListener(NextDialog);
        }
        else
        {
            Debug.LogError("Next Button is not assigned");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseDiaog();
        StartDialog(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialog(int dialogId)
    {
        DialogSO dialog = dialogDatabase.GetDialogById(dialogId);
        if (dialog != null)
        {
            StartDialog(dialog);
        }
        else
        {
            Debug.LogError($"Dialog with ID {dialogId} not Found!");
        }

    }
    public void StartDialog(DialogSO dialog)
    {
        if (dialog == null) return;

        currentDialog = dialog;
        ShowDiaog();
        dialogPanel.SetActive(true);

    }
    public void ShowDiaog()
    {
        if (currentDialog == null) return;
        characterNameText.text = currentDialog.characterName;
        dialogText.text = currentDialog.text;
        if(useTypewriterEffect)
        {
            StartTypingEffect(currentDialog.text);
        }
        else
        {
            dialogText.text = currentDialog.text;
        }
        //초상화 설정 (새로 추가된 부분)
        if (currentDialog.portrait != null)
        {
            protraitImage.sprite = currentDialog.portrait;
            protraitImage.gameObject.SetActive(true);

        }
        else if (!string.IsNullOrEmpty(currentDialog.portraitPath))
        {
            Sprite portrait = Resources.Load<Sprite>(currentDialog.portraitPath);
            if (portrait != null)
            {
                protraitImage.sprite = portrait;
                protraitImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Portrait not found at path : {currentDialog.portraitPath}");
                protraitImage.gameObject.SetActive(false);
            }
        }
        ClearChoices();
        if (currentDialog.choices !=null && currentDialog.choices.Count > 0)
        {
            ShowChoices();
            NextButton.gameObject.SetActive(false);
        }
        else
        {
            NextButton.gameObject.SetActive(true);
        }


    }
    public void NextDialog()
    {
        if(isTyping)
        {
            StopTypingEffect();
            dialogText.text = currentDialog.text;
            isTyping = false;
            return;
        }
        if (currentDialog != null && currentDialog.nextld > 0)
        {
            DialogSO nextDialog = dialogDatabase.GetDialogById(currentDialog.nextld);
            if (nextDialog != null)
            {
                currentDialog = nextDialog;
                ShowDiaog();
            }
            else
            {
                CloseDiaog();
            }

        }
        else
        {
            CloseDiaog();
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        foreach(char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

    }
    private void StopTypingEffect()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    private void StartTypingEffect(string text)
    {
        isTyping = true;
        if (typingCoroutine != null)
        {
            StopCoroutine (typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    public void CloseDiaog()
    {
        dialogPanel.SetActive(false);
        currentDialog = null;
        StopTypingEffect();
    }
    private void ClearChoices()
    {
        foreach(Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        choicesPanel.SetActive(false);
    }
    public void SelectChoice(DialogChoiceSO choice)
    {
        if(choice != null && choice.nextId>0)
        {
            DialogSO nextDialog = dialogDatabase.GetDialogById(choice.nextId);
            if(nextDialog != null)
            {
                currentDialog = nextDialog;
                ShowDiaog();
            }
            else
            {
                CloseDiaog();
            }

        }
        else
        {
            CloseDiaog();
        }
    }

    private void ShowChoices()
    {
        choicesPanel.SetActive(true);
        foreach (var choice in currentDialog.choices)
        {
            GameObject choiceGO = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            TextMeshProUGUI buttonText = choiceGO.GetComponentInChildren<TextMeshProUGUI>();
            Button button = choiceGO.GetComponent<Button>();

            if (buttonText != null)
            {
                buttonText.text = choice.text;

            }
            if (button != null)
            {
                DialogChoiceSO choiceSO = choice;
                button.onClick.AddListener(() => SelectChoice(choiceSO));
            }
        }
    }
}
