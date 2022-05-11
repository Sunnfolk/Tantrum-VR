using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RadialSection
{
    [Header("We are changed by other scripts and the editor")]
    public Sprite icon = null;
    public SpriteRenderer iconRenderer = null;
    public UnityEvent onPress = new UnityEvent();
}
