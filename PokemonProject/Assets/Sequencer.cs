using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SequencerNS
{
    public class Sequencer : MonoBehaviour
    {
        private static Sequencer instance;
        public static Sequencer Instance => instance;

        // Differents behaviors possible
        [SerializeField] DialogueManager dialogueManager;
        [SerializeField] ChoiceManager choiceManager;
        
        [SerializeField] private List<InteractionSO> stack = new();
        [SerializeField] private bool isPlayingSequence;
        [SerializeField] private InteractionSO testInteraction;
        [SerializeField] private SequenceType currentSequenceType = SequenceType.None;
        public SequenceType CurrentSequenceType => currentSequenceType;
        
        private int currentTextIndex;
        
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

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.forward);
        }

        private void Start()
        {
            if (testInteraction != null) AddPopInteraction(testInteraction);

            TryNextStack();
        }

        private async void TryNextStack()
        {
            if (!isPlayingSequence && stack.Count == 0) // Si y'a plus de sequence à jouer
            {
                // Disable l'UI
                dialogueManager.backgroundImage.gameObject.SetActive(false);
                Debug.Log("Plus rien à jouer");
                return;
            }

            if (!isPlayingSequence && stack.Count != 0)
            {
                if (stack[0].interactionType == SequenceType.Dialogue) // Set if next if dialogue
                {
                    StartDialogue();
                }
            }
        }

        private void StartDialogue()
        {
            currentSequenceType = SequenceType.Dialogue;
            currentTextIndex = 0;
            DisplayNextSentence();  
        }

        public async void AddPopInteraction(InteractionSO interactionSO)
        {
            stack.Add(interactionSO);
            if (!isPlayingSequence) TryNextStack();
        }

        private void DisplayNextSentence()
        {
            if (currentTextIndex < stack[0].textToDraw.Length)
            {
                dialogueManager.ReadText(stack[0].textToDraw[currentTextIndex], stack[0].fromGender);
            }
            else // Plus de textes -> Next interactions
            {
                OnEndInteraction();
            }
            
            currentTextIndex++;
        }

        private void OnEndInteraction()
        {
            currentSequenceType = SequenceType.None;
            stack.Remove(stack[0]);
            TryNextStack();
        }

        #region Input
        public void OnClick()
        {
            if (currentSequenceType == SequenceType.Dialogue)
            {
                if (!dialogueManager.IsReading)
                {
                    DisplayNextSentence();
                }
            }
        }
        
        public void OnPerformedClick()
        {
            if (currentSequenceType == SequenceType.Dialogue)
            {
                dialogueManager.currentIntervalBetweenLetterDisplay = dialogueManager.speedUpBetweenLetterDisplay;
            }
        }
        
        public void OnReleaseClick()
        {
            if (currentSequenceType == SequenceType.Dialogue)
            {
                dialogueManager.currentIntervalBetweenLetterDisplay = dialogueManager.baseintervalBetweenLetterDisplay;
            }
        }
        #endregion
    }
}

public enum Gender
{
    Male,
    Female,
    Settings,
    Annoucement
}

public enum SequenceType
{
    Dialogue,
    Choice,
    Movement,
    None
}