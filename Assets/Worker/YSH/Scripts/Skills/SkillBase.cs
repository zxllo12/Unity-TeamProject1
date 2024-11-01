using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBase : MonoBehaviour
{
    // 스킬 데이터
    protected SkillData _skillData;
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected ParticleSystem castEffect;

    protected ParticleSystem _castEffectInstance;
    protected Vector3 _castPos;

    protected Transform _startPos;
    protected GameObject _user;
    
    // 유저의 방향을 알 수 있는 변수 필요

    public Transform StartPos { get { return _startPos; } set { _startPos = value; } }
    public Vector3 CastPos { get { return _castPos; } }

    public GameObject User { get { return _user; } set { _user = value; } }

    public SkillData SkillData { get { return _skillData; } }

    // 고정된 데이터가 아닌 실제 진행중인 쿨타임
    protected float _currentCoolTime;
    public float CurrentCoolTime { get { return _currentCoolTime; } }

    public virtual void SetData(int id)
    {
        if (DataManager.Instance.SkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError($"Skill SetData Error! / ID : {id}");
            return;
        }

        _skillData = data;
    }

    private void Update()
    {
        if (_currentCoolTime > 0)
        {
            _currentCoolTime -= Time.deltaTime;
        }
    }

    public virtual void DoCast()
    {
        Vector3 dist = new Vector3(_startPos.forward.x * _skillData.Range, 0, 0);
        _castPos = StartPos.position + dist;

        if (castEffect == null)
            return;

        if (_castEffectInstance == null)
        {
            ParticleSystem particle = Instantiate(castEffect);
            _castEffectInstance = particle;
        }
        else
        {
            _castEffectInstance.gameObject.SetActive(true);
        }
    }

    public virtual void DoSkill(float attackPoint)
    {
        Debug.Log($"Do Skill : {_skillData.Name}");
        // 스킬 사용 시의 공통 행동
        _currentCoolTime = _skillData.CoolTime;
    }

    public virtual void StopCast()
    {
        _castEffectInstance?.Stop();
        _castEffectInstance?.gameObject?.SetActive(false);
    }

    public virtual void StopSkill()
    {
        // 스킬 중단 시의 공통 행동
    }
}
