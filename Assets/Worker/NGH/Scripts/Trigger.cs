using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] bool spawnMonsterOnEnter = false; // Ʈ���ſ� ���� �� ���� ���� ����
    [SerializeField] bool spawnMonsterWhenClear = false; // ���Ͱ� ���� �׾����� �������� ��������
    [SerializeField] bool openDoorWhenClear = false; // ���� ���� ���Ž� �� ���� ����

    [Header("Trigger Components")]
    [SerializeField] GameObject[] monsters; // ù ��° ���� ���� �迭
    [SerializeField] GameObject[] nextMonsters; // ���� ���̺� ���� �迭
    [SerializeField] GameObject door; // ���� �� ��ü
    public UnityAction<Trigger> triggerDestroyed; // Ʈ���� ��Ȱ��ȭ �� ����� �̺�Ʈ

    private int remainingMonsters;

    private void Awake()
    {
        GameManager.Instance.SetTrigger(this);
        remainingMonsters = 0;
    }

    private void OnDestroy()
    {
        triggerDestroyed?.Invoke(this);
        OpenDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && spawnMonsterOnEnter == true || openDoorWhenClear == true)
        {
            if (spawnMonsterOnEnter)
            {
                SpawnMonsters(monsters);
                monsters = new GameObject[0];
            }
            else if (openDoorWhenClear == true && remainingMonsters == 0 && nextMonsters.Length == 0)
            {
                OpenDoor();
            }

            // Ʈ���� ��Ȱ��ȭ ȣ���� �� �������� �������� ����
        }
    }

    // ������ ���� �迭�� �����ϴ� �޼���
    public void SpawnMonsters(GameObject[] monstersToSpawn)
    {
        if (monstersToSpawn.Length > 0)
        {
            remainingMonsters = monstersToSpawn.Length;
            foreach (GameObject monster in monstersToSpawn)
            {
                monster.SetActive(true);
                var monsterState = monster.GetComponent<MonsterState>();
                if (monsterState != null)
                {
                    monsterState.OnDead += OnMonsterDeath;
                }
            }
        }
    }

    // ���Ͱ� ����� �� ȣ��Ǵ� �޼���
    private void OnMonsterDeath(MonsterState monster)
    {
        remainingMonsters--;
        monster.OnDead -= OnMonsterDeath;
        // ��� ���Ͱ� �׾��� �� ó��
        if (remainingMonsters <= 0)
        {
            if (openDoorWhenClear == true && spawnMonsterWhenClear == false)
            {
                OpenDoor();
                Destroy(gameObject);
            }
            else if (spawnMonsterWhenClear == true && nextMonsters.Length > 0)
            {
                // ���� ���̺� ���� ����
                SpawnMonsters(nextMonsters);
                nextMonsters = new GameObject[0]; // ���� ���Ͱ� �ٽ� �������� �ʵ��� �� �迭�� ����
            }
            else if (openDoorWhenClear == true && nextMonsters.Length == 0)
            {
                OpenDoor();
                Destroy(gameObject);
            }
        }
    }

    public void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false);
        }
    }
}
