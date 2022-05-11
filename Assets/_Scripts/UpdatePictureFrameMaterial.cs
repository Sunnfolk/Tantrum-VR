using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Destructible))]
public class UpdatePictureFrameMaterial : MonoBehaviour {
  private MeshRenderer _meshRenderer;
  private Destructible _destructible;

  private void Start() {
    _meshRenderer = GetComponent<MeshRenderer>();
    _destructible = GetComponent<Destructible>();

    _destructible.onDestroyed += OnDestructibleDestroyed;
  }

  private void OnDestroy() {
    _destructible.onDestroyed -= OnDestructibleDestroyed;
  }

  private void OnDestructibleDestroyed(GameObject newObject) {
    MeshRenderer[] newMeshRenderers = newObject.GetComponentsInChildren<MeshRenderer>();

    foreach (var newMeshRenderer in newMeshRenderers)
    {
        newMeshRenderer.materials = _meshRenderer.materials;
    }
  }
}