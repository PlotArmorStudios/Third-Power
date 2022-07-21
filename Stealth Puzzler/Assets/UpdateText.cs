using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateText : MonoBehaviour
{
  private void OnValidate()
  {
    var text = GetComponent<TMP_Text>().text;
    
    gameObject.name = text;
  }
}
