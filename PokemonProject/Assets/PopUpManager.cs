using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager instance;
    public static PopUpManager Instance => instance;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI textZone;
    
    [SerializeField] private float intervalBetweenLetterDisplay = 0.05f;

    [SerializeField] [TextArea] public string TestText;

    [SerializeField] string tempText = null;
    [SerializeField] int indexLetter = 0;
    [SerializeField] float timer;
    [SerializeField] bool isReading;

    [SerializeField] Color[] textColorByGender;

    [SerializeField] private PopUpSO currentPopUp;
    
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
        ReadText(TestText, Gender.Female);
    }

    public async void SendPopUp(PopUpSO interaction) => currentPopUp = interaction;
    
    public async Task ReadText(string text, Gender gender)
    {
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
            if (timer > intervalBetweenLetterDisplay)
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
}

public enum Gender
{
    Male, 
    Female,
    Settings,
    Annoucement
}
