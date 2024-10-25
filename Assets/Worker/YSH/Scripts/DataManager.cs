using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    Dictionary<int, SkillData> _skillDict = new Dictionary<int, SkillData>();

    public Dictionary<int, SkillData> SkillDict { get { return _skillDict; } }

    protected override void Init()
    {
        LoadData();
    }

    void LoadData()
    {
        LoadSkillData();

        Debug.Log("Data Load Complete");
    }

    void LoadSkillData()
    {
        if (CSVParser.GetDataString("SkillData_Table", out string[] lines) == false)
        {
            Debug.Log("SkillData Load Error!");
            return;
        }

        for (int line = 1; line < lines.Length; line++)
        {
            SkillData skillData = new SkillData();
            skillData.Load(lines[line].Split(','));

            _skillDict.Add(skillData.ID, skillData);
        }

        Debug.Log("SkillData Load OK!");
        Debug.Log(lines);
    }
}

public interface IDataLoader
{
    public void Load(string[] fields);
}