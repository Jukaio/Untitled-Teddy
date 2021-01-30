using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Flyweight Collection", order = 1)]
public class FlyweightCollection : ScriptableObject
{
    [SerializeField] public GameObject[] items;
}
