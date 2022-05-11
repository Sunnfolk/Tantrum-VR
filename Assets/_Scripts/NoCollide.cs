using UnityEngine;

// For having a nocollide between two objects
[RequireComponent(typeof(Collider))]
public class NoCollide : MonoBehaviour
{
  public Collider otherCollider;

  private void Start() {
    Collider myCollider = GetComponent<Collider>();
    Physics.IgnoreCollision(myCollider, otherCollider);
  }
}
