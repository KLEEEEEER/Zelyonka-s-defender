using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    Transform player;
    PlayerController playerController;
    public Animator animator;
    public int Health = 10;
    public int MaxHealth = 10;
    public int damage = 5;

    public RectTransform healthbar;

    GameState gameState;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        gameState = Camera.main.GetComponent<GameState>();
        StartCoroutine(SetRandomSpeed());
        //agent.Warp(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
        animator.SetFloat("speed", agent.speed);
        //agent.Warp(player.position);
    }

    IEnumerator SetRandomSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            agent.speed = Random.Range(1f, 5f);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        healthbar.sizeDelta = new Vector2(Health * 20, healthbar.sizeDelta.y);
    }

    void Die()
    {
        if (gameState != null)
        {
            gameState.OnEnemyKilled();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // https://www.youtube.com/watch?v=thA3zv0IoUM
        if (other.CompareTag("Player"))
        {
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
        /*switch (other.transform.tag)
        {
            case "Player":
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                }
                break;
        }*/
    }

}
