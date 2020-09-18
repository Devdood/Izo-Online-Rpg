using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Data/Mob Data")]
public class MobData : ScriptableObject
{
    public int id;
    public string name;
    public Sprite icon;
    public GameObject characterModel;
}