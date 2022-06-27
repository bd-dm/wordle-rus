using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Letter))]
public class LetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Application.isPlaying)
        {
            return;
        }
        
        GUILayout.Space(20f);
            
        if (GUILayout.Button("Enter letter"))
        {
            ((Letter) target).EnterLetter('C');
        }
            
        if (GUILayout.Button("Delete letter"))
        {
            ((Letter) target).DeleteLetter();
        }
            
        if (GUILayout.Button("Shake"))
        {
            ((Letter) target).Shake();
        }
            
        if (GUILayout.Button("Correct state"))
        {
            ((Letter) target).SetState(LetterState.Correct);
        }
            
        if (GUILayout.Button("Incorrect"))
        {
            ((Letter) target).SetState(LetterState.Incorrect);
        }
            
        if (GUILayout.Button("Wrong location state"))
        {
            ((Letter) target).SetState(LetterState.WrongLocation);
        }
    }
}