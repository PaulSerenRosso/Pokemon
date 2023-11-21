using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSendPopUp : MonoBehaviour
{
    public InteractionSO[] interationsToSend;
    public string name;
    private bool isInteractable = true;
    public bool IsInteractable => isInteractable;
}
