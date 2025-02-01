using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes creating a new weapon easier
[CreateAssetMenu(fileName ="New Weapon", menuName =("Scriptable Objects/Weapon"))]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] public GameObject weaponPrefab;
    //currently using this to track which weapons we have equipped 
}
