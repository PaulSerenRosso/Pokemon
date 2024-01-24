using System.Collections.Generic;
using UnityEngine;
using System;

namespace SequencerNS
{
    public class Sequencer : MonoBehaviour
    {
        private static Sequencer instance;
        public static Sequencer Instance => instance;

        // Differents behaviors possible
        public DialogueManager dialogueManager;
        [SerializeField] ChoiceManager choiceManager;
        public PlayerCharacter character;
        
        [SerializeField] private List<InteractionSO> stack = new();
        [SerializeField] private bool isPlayingSequence;
        [SerializeField] private InteractionSO testInteraction;
        [SerializeField] private SequenceType currentSequenceType = SequenceType.None;
        public Action OnEndSequence;
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
            if (testInteraction != null)
            {
                Debug.Log("Not null : " + testInteraction.name);
                AddPopInteraction(testInteraction);
            }

            TryNextStack();
        }

        private void TryNextStack()
        {
            if (!isPlayingSequence && stack.Count == 0) // Si y'a plus de sequence à jouer
            {
                // Disable l'UI
                Debug.Log("disable ui ");
                dialogueManager.backgroundImage.gameObject.SetActive(false);
                OnEndSequence?.Invoke();
                OnEndSequence = null;
                return;
            }

            if (!isPlayingSequence && stack.Count != 0)
            {
                Debug.Log("Check SequenceType Interaction");
                if (stack[0].interactionType == SequenceType.Dialogue) // Set if next if dialogue
                {
                    
                    StartDialogue();
                }
                else if (stack[0].interactionType == SequenceType.Choice) // Set if next if dialogue
                {
                    StartChoice();
                }
            }
        }

        private async void StartChoice()
        {
            isPlayingSequence = true;
            currentSequenceType = SequenceType.Choice;
            currentTextIndex = 0;
            await dialogueManager.ReadText(stack[0].textToDraw[0], stack[0].fromGender);
            choiceManager.PopChoiceInteraction((ChoiceSO)stack[0]);
        }

        private void StartDialogue()
        {
            isPlayingSequence = true;
            currentSequenceType = SequenceType.Dialogue;
            currentTextIndex = 0;
            DisplayNextSentence();  
        }

        public void AddPopInteraction(InteractionSO interactionSO)
        {
            stack.Add(interactionSO);
            if (!isPlayingSequence) TryNextStack();
        }
        
        public void AddCombatInteraction(string textToDraw, Action callback)
        {
            InteractionSO combatInteraction = ScriptableObject.CreateInstance<TextInteractionSO>();
            combatInteraction.interactionType = SequenceType.Dialogue;
            combatInteraction.fromGender = Gender.Annoucement;
            combatInteraction.textToDraw = new[] { textToDraw };
            stack.Add(combatInteraction);
            OnEndSequence = callback;
            Debug.Log(isPlayingSequence);
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

        public void OnEndInteraction()
        {
            currentSequenceType = SequenceType.None;
            isPlayingSequence = false;
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

            if (currentSequenceType == SequenceType.Choice)
            {
                Debug.Log("On Click");
            }
        }
        
        public void OnPerformedClick()
        {
            if (currentSequenceType == SequenceType.Dialogue)
            {
                dialogueManager.currentIntervalBetweenLetterDisplay = dialogueManager.speedUpBetweenLetterDisplay;
            }
            
            if (currentSequenceType == SequenceType.Choice)
            {
                Debug.Log("OnPerformedClick");
                dialogueManager.currentIntervalBetweenLetterDisplay = dialogueManager.speedUpBetweenLetterDisplay;
            }
        }
        
        public void OnReleaseClick()
        {
            if (currentSequenceType == SequenceType.Dialogue)
            {
                dialogueManager.currentIntervalBetweenLetterDisplay = dialogueManager.baseintervalBetweenLetterDisplay;
            }
            
            if (currentSequenceType == SequenceType.Choice)
            {
                Debug.Log("OnReleaseClick");
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