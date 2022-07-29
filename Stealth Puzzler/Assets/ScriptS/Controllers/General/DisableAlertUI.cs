using System.Collections;
using UnityEngine;

public class DisableAlertUI : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AlertUI>().enabled = false;
    }
}