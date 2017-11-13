using UnityEngine;
using Infra.Gameplay;

namespace BatCave {
/// <summary>
/// Contains a ceiling and a floor of the cave.
/// </summary>
public class CaveChunk : MonoBehaviour {
    public PolygonColliderWithMesh ceiling;
    public PolygonColliderWithMesh floor;
}
}
