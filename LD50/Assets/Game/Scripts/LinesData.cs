using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_line_data", menuName = "Datas/New Lines", order = 0)]
public class LinesData : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public int Number;
        public string Line;
        public AudioClip Sound;
        public Monologue.StagingEffect Staging;
    }

    public Info[] LineInfos;
    public string[] Lines;
    public int CurrentLine { get; set; } = 0;
    public bool IsLast => CurrentLine >= Lines.Length;

    public void Reset()
    {
        CurrentLine = 0;
    }

    public bool TryGetNextLine(out string line)
    {
        line = string.Empty;

        bool b = !IsLast;
        if (b)
        {
            line = Lines[CurrentLine];
            CurrentLine++;
        }
        return b;
    }
}
