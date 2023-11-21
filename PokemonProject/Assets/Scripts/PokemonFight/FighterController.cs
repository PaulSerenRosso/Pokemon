
using System;
using UnityEngine;

public abstract class FighterController : MonoBehaviour
{
   public Action endTurnEvent;
   public Fighter fighter;
   public abstract void Activate();
   public abstract void Deactivate();
}
