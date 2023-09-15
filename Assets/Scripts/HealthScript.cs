using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] int health = 50;
    [SerializeField] int score = 50;
    [SerializeField] ParticleSystem hitEffect;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;

    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;

    void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        DamageDiealer damageDiealer = other.GetComponent<DamageDiealer>();

        if (damageDiealer != null)
        {
            TakeDamage(damageDiealer.GetDamage());
            PlayHitEffect();
            audioPlayer.PlayDamageClip();
            ShakeCamera();
            damageDiealer.Hit();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    private void ShakeCamera()
    {
        if(cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            audioPlayer.PlayDestroyedClip();
            Die();
        }
    }

    private void Die()
    {
        if(!isPlayer)
        {
            scoreKeeper.CalculateScore(score);
        }
        else
        {
            levelManager.LoadGameOver();
        }
        Destroy(gameObject);
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    
}
