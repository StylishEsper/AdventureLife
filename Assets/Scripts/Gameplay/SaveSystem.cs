using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    public static void SaveGame(List<Item.ItemName> itemNames, List<int> slotNumbers, List<Objective> objectives,
        List<ObtainableItem.WorldItem> worldItems, List<CutscenePlayer.CutsceneName> playedCutscenes, 
        List<GameEvent.MemorableEvent> memorableEvents, int health, int energy, int saveNumber, int[] quickSlotNumbers, 
        int[] crystalUpgrades, float timePlayed, Vector3 position, string scene, string element, 
        bool[] inQuickSlots, bool[] equippedCrystals)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/TwoSpiritSavedData" + saveNumber + ".txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(itemNames, slotNumbers, objectives, worldItems,
            playedCutscenes, memorableEvents, health, energy, saveNumber, quickSlotNumbers, crystalUpgrades, 
            timePlayed, position, scene, element, inQuickSlots, equippedCrystals);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadGame(int saveNumber)
    {
        string path = Application.persistentDataPath + "/TwoSpiritSavedData" + saveNumber + ".txt";

        Debug.Log(path);

        if (!File.Exists(path))
        {
            SaveGame(new List<Item.ItemName>(), new List<int>(), new List<Objective>(), 
                new List<ObtainableItem.WorldItem>(), new List<CutscenePlayer.CutsceneName>(), new List<GameEvent.MemorableEvent>(),
                100, 0, saveNumber, new int[0], new int[4]{0,0,0,0}, 0, new Vector3(-5.4f, -1.2f, 0), 
                "Home", "Water", new bool[0], new bool[4]{false,false,false,false});
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        PlayerData data = (PlayerData)formatter.Deserialize(stream);
        stream.Close();

        return data;
    }

    public static bool[] ExistentSaves()
    {
        bool[] saves = { true, true, true };

        string path1 = Application.persistentDataPath + "/TwoSpiritSavedData" + 1 + ".txt";
        string path2 = Application.persistentDataPath + "/TwoSpiritSavedData" + 2 + ".txt";
        string path3 = Application.persistentDataPath + "/TwoSpiritSavedData" + 3 + ".txt";

        if (!File.Exists(path1))
        {
            saves[0] = false;
        }

        if (!File.Exists(path2))
        {
            saves[1] = false;
        }

        if (!File.Exists(path3))
        {
            saves[2] = false;
        }

        return saves;
    }

    public static PlayerData GetGameData(int saveNumber)
    {
        string path = Application.persistentDataPath + "/TwoSpiritSavedData" + saveNumber + ".txt";

        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        PlayerData data = (PlayerData)formatter.Deserialize(stream);
        stream.Close();

        return data;
    }
}
