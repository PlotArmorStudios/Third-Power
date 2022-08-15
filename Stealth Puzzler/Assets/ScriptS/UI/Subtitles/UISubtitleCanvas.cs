using TMPro;
using UnityEngine;

public class UISubtitleCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _letterbox;
    [SerializeField] private TMP_Text _subTitleText;

    public void SetSubtitle(SubtitleObject subtitleObject)
    {
        _subTitleText.fontStyle = subtitleObject.FontStyle;
        _subTitleText.text = subtitleObject.Subtitle;
    }

    public void ClearSubtitle()
    {
        _subTitleText.text = "";
    }

    public void TriggerLetterboxing()
    {
        _letterbox.GetComponent<Animator>().SetTrigger("Animate In");
    }
}