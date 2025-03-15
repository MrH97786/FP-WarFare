using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerWeapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set;}

    public AudioSource ShootingChannel;

    public AudioClip Pistol_D_Shot;
    public AudioClip M4A1_AssaultRifle_Shot;

    public AudioSource reloadingSound;
    public AudioSource reloadingSound_M4A1_AssaultRifle;
    
    public AudioSource emptyMagazineSound;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;

    public AudioSource enemyChannel;
    public AudioClip enemyAttacking;
    public AudioClip enemyHurt;
    public AudioClip enemyDeath;

    public AudioSource enemyLoopChannel;
    public AudioClip enemyWalking;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.Pistol_D:
                ShootingChannel.PlayOneShot(Pistol_D_Shot);
                break;
            case WeaponModel.M4A1_AssaultRifle:
                ShootingChannel.PlayOneShot(M4A1_AssaultRifle_Shot);
                break; 
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.Pistol_D:
                reloadingSound.Play();
                break;
            case WeaponModel.M4A1_AssaultRifle:
                reloadingSound_M4A1_AssaultRifle.Play();
                break;
        }
    }
}
