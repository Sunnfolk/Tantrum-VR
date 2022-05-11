using UnityEngine;
using UnityEngine.Events;

public class OnJointBreakHandler : MonoBehaviour
{
    public UnityEvent onJointBreak;

    void OnJointBreak()
    {
      onJointBreak.Invoke();
    }
}
