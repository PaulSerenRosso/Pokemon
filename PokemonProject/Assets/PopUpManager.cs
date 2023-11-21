using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager instance;
    public static PopUpManager Instance => instance;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI textZone;

    [SerializeField] private float currentIntervalBetweenLetterDisplay;
    [SerializeField] private float baseintervalBetweenLetterDisplay = 0.05f;
    [SerializeField] private float speedUpBetweenLetterDisplay => baseintervalBetweenLetterDisplay * 0.5f;

    [SerializeField] [TextArea] public string TestText;

    [SerializeField] string tempText = null;
    [SerializeField] int indexLetter = 0;
    [SerializeField] float timer;
    
    [SerializeField] bool isReading;
    

    [SerializeField] Color[] textColorByGender;

    [SerializeField] private List<Task> stack;
    [SerializeField] private bool playingTaskOnStack;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        stack.Add(ReadText(TestText, Gender.Female));
        CheckStack();
        //ReadText(TestText, Gender.Female);
    }

    private async void CheckStack()
    {
        if (!playingTaskOnStack || !isReading)
        {
            Task.Run(stack[0].Start);
        }
    }

    public async void SendPopUp(TextInteractionSO text = null, ChoiceSO choice = null)
    {
        if (text != null)
        {
            stack.Add(ReadText(text.textToDraw, text.fromGender));
        }

        if (choice != null)
        {
            stack.Add(DisplayChoice(choice));
        }
    }
    
    public async Task ReadText(string text, Gender gender)
    {
        currentIntervalBetweenLetterDisplay = baseintervalBetweenLetterDisplay;
        if (backgroundImage.gameObject.activeSelf == false) backgroundImage.gameObject.SetActive(true);

        timer = 0;
        var textLenght = text.Length;
        tempText = null;
        indexLetter = 0;
        isReading = true;
        textZone.color = textColorByGender[(int)gender];
        
        while (indexLetter < textLenght)
        {
            timer += Time.deltaTime;
            if (timer > currentIntervalBetweenLetterDisplay)
            {
                timer = 0;
                tempText += text[indexLetter];
                textZone.text = tempText;
                indexLetter++;
            }

            await Task.Yield();
        }

        isReading = false;
    }

    public async Task DisplayChoice(ChoiceSO choice)
    {
        
    }

    public void OnClick()
    {
        if (isReading)
        {
            currentIntervalBetweenLetterDisplay = speedUpBetweenLetterDisplay;
        }
    }

    public void OnReleaseClick()
    {
        if (isReading)
        {
            currentIntervalBetweenLetterDisplay = baseintervalBetweenLetterDisplay;
        }
    }
}



public enum Gender
{
    Male, 
    Female,
    Settings,
    Annoucement
}
