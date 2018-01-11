using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class TestItem
{
    public string name;
    public int uses;
    public int health;

    public TestItem() {}
    public TestItem(string name, int uses, int health)
    {
        this.name = name;
        this.uses = uses;
        this.health = health;
    }
}