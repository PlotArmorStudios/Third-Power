using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class SubtitleObject : ScriptableObject
{
   [SerializeField] private string _subtitle;
   [SerializeField] public FontStyles FontStyle;

   public string Subtitle => _subtitle;
}