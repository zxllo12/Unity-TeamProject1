using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    SkillBase[] _playerSkillSlot = new SkillBase[(int)Enums.PlayerSkillSlot.Length];

    Coroutine _castRoutine;

    public void EquipSkill(int skillID, Enums.PlayerSkillSlot slot)
    {
        // ID 검사
        if (DataManager.Instance.SkillDict.TryGetValue(skillID, out SkillData data) == false)
        {
            Debug.LogError($"SkillHandler EquipSkill failed... / ID : {skillID}");
            Debug.LogError("Please Check data");
            return;
        }

        SkillBase prefab = Resources.Load<SkillBase>($"Prefabs/Skills/{data.ClassName}");
        if (prefab == null)
        {
            Debug.LogError($"Can't find SkillBase Component! / ID : {skillID}");
            return;
        }

        SkillBase skill = Instantiate(prefab);
        skill.SetData(data.ID);
        skill.transform.SetParent(gameObject.transform);

        // 해당 슬롯에 스킬이 존재하면 해제한다.
        if (_playerSkillSlot[(int)slot] != null)
            UnEquipSkill(slot);

        _playerSkillSlot[(int)slot] = skill;
    }

    public void UnEquipSkill(Enums.PlayerSkillSlot slot)
    {
        if (_playerSkillSlot[(int)slot] = null)
            return;

        // slot에 있는 스킬을 튀어나오게 해야함

        // slot에서 삭제
        _playerSkillSlot[(int)slot] = null;
    }

    public void DoSkill(Enums.PlayerSkillSlot slot, Vector3 startPos)
    {
        // 슬롯에 스킬이 존재하는지 비교
        if (_playerSkillSlot[(int)slot] == null)
            return;

        // 쿨타임 체크
        if (_playerSkillSlot[(int)slot].CurrentCoolTime > 0)
            return;

        if (_castRoutine != null)
            return;

        Cast(slot, startPos);
    }

    public void Cast(Enums.PlayerSkillSlot slot, Vector3 startPos)
    {
        _playerSkillSlot[(int)slot].StartPos = startPos;
        _playerSkillSlot[(int)slot].User = gameObject;
        // 유저 방향 설정 필요
        _castRoutine = StartCoroutine(CastRoutine(slot));
    }

    IEnumerator CastRoutine(Enums.PlayerSkillSlot slot)
    {
        WaitForSeconds castTime = new WaitForSeconds(_playerSkillSlot[(int)slot].SkillData.CastTime);

        Debug.Log($"Start Cast : {_playerSkillSlot[(int)slot].SkillData.Name}");
        yield return castTime;
        _playerSkillSlot[(int)slot].DoSkill();
        _castRoutine = null;
    }

    public void StopSkill(Enums.PlayerSkillSlot slot)
    {
        // 슬롯에 스킬이 존재하는지 비교
        if (_playerSkillSlot[(int)slot] == null)
            return;

        if (_castRoutine != null)
        {
            StopCoroutine(_castRoutine);
            _castRoutine = null;
            return;
        }

        _playerSkillSlot[(int)slot].StopSkill();
    }
}
