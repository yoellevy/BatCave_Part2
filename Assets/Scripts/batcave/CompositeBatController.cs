using UnityEngine;

namespace BatCave {
/// <summary>
/// A scriptable object that allows getting control from several bat controllers.
/// </summary>
[CreateAssetMenu(menuName = "Bat Controller/Composite")]
public class CompositeBatController : BatController {
    [SerializeField] BatController[] controllers;

    /// <summary>
    /// Returns true if any of the controllers wants the bat to fly up.
    /// </summary>
    public override bool WantsToFlyUp() {
        foreach (var controller in controllers) {
            if (controller.WantsToFlyUp()) return true;
        }
        return false;
    }
}
}