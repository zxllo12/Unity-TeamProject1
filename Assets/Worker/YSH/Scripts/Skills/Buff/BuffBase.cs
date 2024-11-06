using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
{
    // buff�� ����޴� ��ü
    protected GameObject _owner;

    // tick �� delay (tick���� ��)
    protected float _tickDelay;
    protected float _tickTimer;

    // buff ���� �ð�
    protected float _buffTimer;

    // 1ȸ ���ط�
    protected float _damagePerTick;

    public float TickTimer { get { return _tickTimer; } }
    public float BuffTimer { get { return _buffTimer; } }

    protected string _clipName;

    public void UpdateBuff()
    {
        // ���� �ð� ����
        if (_buffTimer > 0)
        {
            _buffTimer -= Time.deltaTime;
        }

        // ƽ ����
        if (_tickTimer > 0)
        {
            _tickTimer -= Time.deltaTime;
        }
        else
        {
            DoTick();
        }
    }

    public void SetInfo(GameObject owner, float length, int tickCount, float basicAttack, float skillDamage, string clipName)
    {
        _owner = owner;
        _tickTimer = 0;
        //_tickDelay = (length / (tickCount - 1));
        _tickDelay = (length / tickCount);
        _damagePerTick = (basicAttack * skillDamage) / tickCount;
        _buffTimer = length;

        _clipName = clipName;
    }

    public virtual void DoTick()
    {
        if (!string.IsNullOrEmpty(_clipName))
            SoundManager.Instance.Play(Enums.ESoundType.SFX, _clipName);

        _tickTimer = _tickDelay;
    }
}
