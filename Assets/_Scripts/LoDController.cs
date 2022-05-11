using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoDController : MonoBehaviour
{
    public static float s_levelOfDestruction;
    [Space(10)]
    [Tooltip("how much level of destruction decrements every second")]
    [SerializeField] private float _decrementRate;
    [SerializeField] private Vector2 _lodLimit = new Vector2(0, 3);

    
    void OnEnable() 
    {
        s_levelOfDestruction = 0;
    }

    void Update()
    {
        if(s_levelOfDestruction > 0)
        {
            s_levelOfDestruction -= _decrementRate * Time.deltaTime;
        }

        s_levelOfDestruction = Mathf.Clamp(s_levelOfDestruction, _lodLimit.x, _lodLimit.y);
    }
}
