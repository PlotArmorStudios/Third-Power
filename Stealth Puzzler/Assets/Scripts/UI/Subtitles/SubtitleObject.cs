using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SubtitleObject : ScriptableObject
{
   [SerializeField] private string _subtitle;

   public string Subtitle => _subtitle;
}