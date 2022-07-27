using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelCounter : MonoBehaviour
{
   private void OnEnable()
   {
      GetComponent<TMP_Text>().text = FindObjectOfType<LevelData>().LevelIndex.ToString();
   }
}
