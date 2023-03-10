using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Dialogue", menuName = "UI/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string dialogue = "";
    public float charPerSecond = 7f;
    public Color color = Color.black;

    public IEnumerator Display(TextMeshProUGUI tmp, System.Action DialogueFinished){
        AudioManager.PlayPencil(dialogue.Length / charPerSecond);
        tmp.color = color;
        tmp.text = "";
        foreach(char c in dialogue){
            tmp.text += c;
            yield return new WaitForSeconds(1 / charPerSecond);
        }
        DialogueFinished();
        yield return null;
    }
}
