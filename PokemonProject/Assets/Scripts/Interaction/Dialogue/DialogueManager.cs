using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SequencerNS
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] public Image backgroundImage;
        [SerializeField] private TextMeshProUGUI textZone;
        
        [SerializeField] public float currentIntervalBetweenLetterDisplay;
        [SerializeField] readonly public float baseintervalBetweenLetterDisplay = 0.05f;
        [SerializeField] public float speedUpBetweenLetterDisplay => baseintervalBetweenLetterDisplay * 0.5f;
        
        [SerializeField] string tempText = null;
        [SerializeField] string fullTempText = null;
        [SerializeField] int indexLetter = 0;
        [SerializeField] float timer;

        [SerializeField] Color[] textColorByGender;
        [SerializeField] Sprite[] dialoguesBoxSprites;
        
        [SerializeField] bool isReading;
        public bool IsReading => isReading;
        
        public async Task ReadText(string text, Gender gender)
        {
            currentIntervalBetweenLetterDisplay = baseintervalBetweenLetterDisplay;
            if (backgroundImage.gameObject.activeSelf == false) backgroundImage.gameObject.SetActive(true);

            switch (gender)
            {
                case Gender.Annoucement: backgroundImage.sprite = dialoguesBoxSprites[1]; break;
                case Gender.Fight: backgroundImage.sprite = dialoguesBoxSprites[2]; break;
                default: backgroundImage.sprite = dialoguesBoxSprites[0]; break;
            }
            
            isReading = true;
            timer = 0;
            var textLenght = text.Length;
            tempText = "";
            fullTempText = text;
            indexLetter = 0;
            isReading = true;
            textZone.text = tempText;

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
    }
}