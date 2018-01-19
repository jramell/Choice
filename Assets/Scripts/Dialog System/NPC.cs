using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data for a certain NPC. Is attached to any object. Could be a Scriptable Object, eventually.
/// </summary>
public class NPC : MonoBehaviour {
	public string _name;
	public AudioSource voiceSFX;
	public Animator animator;
}
