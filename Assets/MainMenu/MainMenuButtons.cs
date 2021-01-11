using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void IntroCinematicCompleted()
    {
        animator.SetBool("IntroCinematic", true);            
    }
}
