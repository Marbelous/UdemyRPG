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
    public float attackRadius = 5.0f;
    public float moveRadius = 10.0f;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;
    [SerializeField] float defaultDamage = 9f;
    [SerializeField] float secondsBetweenShots = .5f;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

    float currentHealthPoints = 100f;
    bool isAttacking = false;

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

        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots);
        }
        if (distanceToPlayer > attackRadius)
        {
            isAttacking = false;
            CancelInvoke();
        }
        if (distanceToPlayer <= moveRadius)
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
        // Draw red attack radius.
        Gizmos.color = new Color(255f, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        // Draw blue move radius.
        Gizmos.color = new Color(0, 0, 255f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, moveRadius);
    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(defaultDamage);

        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;

        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }
}
