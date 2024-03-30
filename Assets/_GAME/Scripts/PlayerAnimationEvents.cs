using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInChildren<WeaponVisualController>();
    }
    void ReloadIsOver()
    {
        visualController.ReturnRigWeightToOne();
    }
}
