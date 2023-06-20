using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<Item.ItemName> itemNames;
    public List<int> slotNumbers;
    public List<Objective> objectives;
    public List<ObtainableItem.WorldItem> worldItems;
    public List<CutscenePlayer.CutsceneName> playedCutscenes;
    public List<GameEvent.MemorableEvent> memorableEvents;

    public int health;
    public int energy;
    public int saveNumber;
    public int[] quickSlotNumbers;
    public int[] crystalUpgrades = new int[4];

    public float timePlayed;
    public float[] position;

    public string scene;
    public string element;

    public bool[] inQuickSlots;
    public bool[] equippedCrystals = new bool[4];


    public PlayerData(List<Item.ItemName> itemNames, List<int> slotNumbers, List<Objective> objectives,
        List<ObtainableItem.WorldItem> worldItems, List<CutscenePlayer.CutsceneName> playedCutscenes, 
        List<GameEvent.MemorableEvent> memorableEvents, int health, int energy, int saveNumber,int[] quickSlotNumbers,
        int[] crystalUpgrades, float timePlayed, Vector3 position, string scene, string element, 
        bool[] inQuickSlots, bool[] equippedCrystals)
    {
        this.itemNames = itemNames;
        this.slotNumbers = slotNumbers;
        this.objectives = objectives;
        this.worldItems = worldItems;
        this.playedCutscenes = playedCutscenes;
        this.memorableEvents = memorableEvents;
        this.health = health;
        this.energy = energy;
        this.saveNumber = saveNumber;
        this.quickSlotNumbers = quickSlotNumbers;
        this.crystalUpgrades = crystalUpgrades;
        this.timePlayed = timePlayed;
        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;
        this.scene = scene;
        this.element = element;
        this.inQuickSlots = inQuickSlots;
        this.equippedCrystals = equippedCrystals;
    }
}
