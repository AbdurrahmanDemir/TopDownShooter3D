using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    Animator anim;
    Rig rig;
    [SerializeField] private Transform[] guns;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    [SerializeField] private float rigIncreaseStep;
    public bool rigShouldBeIncreased;

    private void Start()
    {
        rig= GetComponentInParent<Rig>();
        anim= GetComponentInParent<Animator>();
        SwitchOn(pistol);
    }

    private void Update()
    {
        SwitchGuns();
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Reload");
            //rig.weight = .15f;
        }
        //if (rigShouldBeIncreased)
        //{
        //    rig.weight+= rigIncreaseStep*Time.deltaTime;

        //    if(rig.weight>=1)
        //        rigShouldBeIncreased=false;
        //}

    }

    public void ReturnRigWeightToOne() => rigShouldBeIncreased = true;

    private void SwitchGuns()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwichAnimationLayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwichAnimationLayer(1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwichAnimationLayer(1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwichAnimationLayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
            SwichAnimationLayer(3);
        }
    }

    void SwitchOn(Transform gunTransform)
    {
        SwitchOff();
        gunTransform.gameObject.SetActive(true);
    }
    void SwitchOff()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].gameObject.SetActive(false);
        }
    }
    private void SwichAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(1, 0);
    }
}
