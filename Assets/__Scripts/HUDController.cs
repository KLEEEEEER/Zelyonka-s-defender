using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Text EnemiesToKillText;

    public void UpdateEnemiesToKillText(int enemies)
    {
        EnemiesToKillText.text = "Enemies to kill: " + enemies;
    }
}
