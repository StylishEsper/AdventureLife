using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    public Name name;

    public int currentCount;
    public int maxCount;

    public bool complete;
    public bool requiresCount;


    public Objective(Name name, bool requiresCount, int currentCount, int maxCount)
    {
        this.name = name;
        this.requiresCount = requiresCount;

        if (requiresCount)
        {
            this.currentCount = currentCount;
            this.maxCount = maxCount;
        }

        complete = false;
    }

    [SerializeField]
    public enum Name
    {
        FindTheCure,
        Witchcraft
    }
}
