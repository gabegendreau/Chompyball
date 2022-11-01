using System.Text.RegularExpressions;
using System;
using UnityEngine;

public class StumpCheck : MonoBehaviour
{
    public Sprite activeStump;
    public Sprite deactiveStump;
 public void updateStumpUI() {
    GameObject[] tinyStumps = GameObject.FindGameObjectsWithTag("TinyStump");
    GameObject[] gameStumps = GameObject.FindGameObjectsWithTag("Stump");
    foreach(GameObject stump in tinyStumps)
    {
        string name = stump.gameObject.name;
        string nameNum = name.Replace("tStump", "");
        int slotNum = 0;
        Int32.TryParse(nameNum, out slotNum);
        if (gameStumps.Length > slotNum)
        {
            stump.GetComponent<SpriteRenderer>().sprite = activeStump;
        } else {
            stump.GetComponent<SpriteRenderer>().sprite = deactiveStump;
        }
    }
 }
}
