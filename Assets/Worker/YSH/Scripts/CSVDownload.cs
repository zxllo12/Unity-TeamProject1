using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class CSVDownload
{
    const string skillDataUrl = "https://docs.google.com/spreadsheets/d/1KKp21OkOGInFUfQvvE-ZlDCtzdnPYfzevhZbfoynPkQ/export?gid=367973711&format=csv";

    public static IEnumerator SkillDataDownloadRoutine()
    {
        // Web에 요청을 보내기 위한 UnityWebRequest 객체
        // urlPath를 통해 웹사이트에 요청
        UnityWebRequest skillDataRequest = UnityWebRequest.Get(skillDataUrl);

        // 요청이 완료될 때 까지 대기 (파일 다운로드)
        yield return skillDataRequest.SendWebRequest();

        // 다운로드가 완료된 상황
        string skillTableText = skillDataRequest.downloadHandler.text;
        if (skillTableText == null)
        {
            Debug.LogError("Skill Data Download Error!");
            yield break;
        }

        Debug.Log("Skill Data Download OK");
        yield return skillTableText;
    }
}
