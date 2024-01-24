using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFighter : Fighter
{
    public override void Init(Fighter enemyFighter, SpriteRenderer fighterSpriteRenderer, SpriteRenderer enemySpriteRenderer,
        Slider hpSlider, TextMeshProUGUI nameText, TextMeshProUGUI statusText, Image statusBackground,
        MonoBehaviour coroutineHandler, Animator sceneAnimator, bool backsprite)
    {
        base.Init(enemyFighter, fighterSpriteRenderer, enemySpriteRenderer, hpSlider, nameText, statusText, statusBackground, coroutineHandler, sceneAnimator, backsprite);
        chooseAnotherPokemonEvent = endTurnEvent; 
    }
}
