using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Monologue", menuName = "Data/Monologue")]
public class Monologue : ScriptableObject{

	public string NpcName;
    public Sprite NpcImage;

	[TextArea(3, 10)]
	public string[] Sentences;

}
