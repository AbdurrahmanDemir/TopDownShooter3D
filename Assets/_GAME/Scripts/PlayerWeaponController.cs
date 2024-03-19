using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Character.Fire.performed += context => Shoot();
    }
    public void Shoot()
    {
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
}
