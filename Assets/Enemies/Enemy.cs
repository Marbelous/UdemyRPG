using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{
    GameObject player = null;
    ThirdPersonCharacter thirdPersonCharacter = null;
    AICharacterControl aiCharacterControl = null;
    public float attackDistance = 5.0f;
    [SerializeField] float maxHealthPoints = 100f;

    float currentHealthPoints = 100f;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackDistance)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255f, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }
}
