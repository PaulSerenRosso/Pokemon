using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonWithXpTurn
{
    public Pokemon pokemon;
    public int xpTurn;

    public PokemonWithXpTurn(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        xpTurn = 1;
    }
}
