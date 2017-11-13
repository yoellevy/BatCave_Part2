using UnityEngine;

namespace BatCave {
/// <summary>
/// A base class that allows to get input for the bat.
/// </summary>
public abstract class BatController : ScriptableObject {
    /// <summary>
    /// Returns true if the controller wants the bat to fly up.
    /// </summary>
    public abstract bool WantsToFlyUp();
}
}