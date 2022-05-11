using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class DrinkingBird : MonoBehaviour
{
  [Header("Grounded Check")]
  public Vector3 raycastCenter;
  public float raycastLength;

  private Animator _animator;

  private void Start() {
    _animator = GetComponent<Animator>();
  }

  private bool CheckIfGrounded() {
    return Physics.Raycast (transform.position + raycastCenter, Vector3.down, raycastLength);
  }
 
  private void Update () {
    var isGrounded = CheckIfGrounded();
    _animator.enabled = isGrounded;
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawRay(transform.position + raycastCenter, Vector3.down * raycastLength);
  }
}
