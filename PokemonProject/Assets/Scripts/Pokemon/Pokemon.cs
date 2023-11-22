using System;


[Serializable]
public class Pokemon
{
    public PokemonSO so;
    private int hp;
    private int maxHp;
    private int level;
    private bool isDied = false; 
    public PokemonCapacity[] capacities;
    
    public Pokemon(PokemonSO pokemonSo)
    {
        so = pokemonSo;
        maxHp = so.maxHp;
        level = so.startLevel;
        SetHp(maxHp);
        capacities = new PokemonCapacity[4];
        for (var index = 0; index < so.pokemonCapacitySO.Length; index++)
        {
            var capacitySo = so.pokemonCapacitySO[index];
            capacities[index] =(PokemonCapacity) Activator.CreateInstance(capacitySo.GetType());
            capacities[index].Init(so.pokemonCapacitySO[index]);
        }
    }

    public int Hp => hp;
    public int MaxHp => maxHp;
    public int Level => level;
    public bool IsDied => isDied;

    public int GetActiveCapacityCount()
    {
        int currentCount = 0;
        for (int i = 0; i < capacities.Length; i++)
        {
            if (capacities[i] != null)
            {
                currentCount++;
            }
        }

        return currentCount;
    }
    public void DecreaseHp(int amount)
    {
        if ((hp-amount) <= 0)
        {
            SetHp(0);
            Die();
        }
        else
        {
            hp -= amount;
        }
    }

    private void Die()
    {
        isDied = true;
    }

    public void Revive()
    {
        isDied = false; 
        SetHp(maxHp);
    }

    public void IncreaseHp(int amount)
    {
        if ((hp+amount) > maxHp)
        {
            SetHp(maxHp);
        }
        else
        {
            hp += amount;
        }
    }

    public void SetHp(int amount)
    {
        hp = amount;
    }
    
}
