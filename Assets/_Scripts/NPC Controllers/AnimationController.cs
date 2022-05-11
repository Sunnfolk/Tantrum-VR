using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();

        //randomly set mood
        _anim.SetFloat("Mood", Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        //update animator level of destruction
        _anim.SetFloat("LoD", LoDController.s_levelOfDestruction);
    }

    public void Escape()
    {
        GetComponentInParent<OfficeWorkerAI>().Escape();
    }
}
