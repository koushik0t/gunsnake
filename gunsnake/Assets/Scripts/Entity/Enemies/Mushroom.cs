﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    [Header("Mushroom")]
    [Tooltip("1 = 4 game ticks")]
    public int attackSpeed;
    private int ticksTillAttack;

    public GameObject mushroomAttackEffect;

    public MushroomEnabler mushRoomEnabler;
    public bool enableOnAwake;
    public bool disableOnDeath;

    protected override void Awake()
    {
        base.Awake();
        myName = "misc_mushroom";

        if (randomizeStartingVars)
            ticksTillAttack = Random.Range(1, attackSpeed);
        else
            ticksTillAttack = attackSpeed;

        if (mushRoomEnabler != null && enableOnAwake)
        {
            mushRoomEnabler.SetEnable(true);
        }
    }

    public override void EnemyTick(int tick)
    {
        if (tick % 4 == 0)
        {
            ticksTillAttack -= 1;

            // visuals
            if (animator != null)
                SetAnimatorBool("isIdle", true);

            switch (ticksTillAttack)
            {
                case 2:
                case 1:
                    if (animator != null)
                        SetAnimatorBool("isPrep", true);
                    break;
                default:
                    if (ticksTillAttack <= 0)
                    {
                        // visuals
                        if (animator != null)
                            SetAnimatorBool("isAttack", true);

                        AudioManager.Play("misc_mushroom_attack");

                        ticksTillAttack = attackSpeed;
                        Attack();
                    }
                    break;
            }
        }
    }

    public override void Die()
    {
        base.Die();

        if (mushRoomEnabler != null && disableOnDeath)
        {
            mushRoomEnabler.SetEnable(false);
        }
    }

    private void Attack()
    {
        Collider2D h = Physics2D.OverlapBox(transform.position, new Vector2(1.5f, 1.5f), 0, playerLayerMask);
        // or use hitbox.OverlapCollider();
        if (h != null && h.tag == "Player")
        {
            h.GetComponent<PlayerSegmentHealth>().TakeDamage(damage);
        }

        // boonk gang bingo bongo hard coder
        Instantiate(mushroomAttackEffect, transform.position + Vector3.up, Quaternion.identity);
        Instantiate(mushroomAttackEffect, transform.position + Vector3.right, Quaternion.identity);
        Instantiate(mushroomAttackEffect, transform.position + Vector3.down, Quaternion.identity);
        Instantiate(mushroomAttackEffect, transform.position + Vector3.left, Quaternion.identity);
        Instantiate(mushroomAttackEffect, transform.position + Vector3.up + Vector3.right, Quaternion.identity);
        Instantiate(mushroomAttackEffect, transform.position + Vector3.right + Vector3.down, Quaternion.identity);
        Instantiate(mushroomAttackEffect, transform.position + Vector3.down + Vector3.left, Quaternion.identity);
        Instantiate(mushroomAttackEffect, transform.position + Vector3.left + Vector3.up, Quaternion.identity);
    }
}
