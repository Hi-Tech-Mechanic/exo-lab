namespace ExoLab.Assembly
{
    /// <summary>
    /// Determines whether the cursor is positioned within the assembly zone,
    /// enabling inspection interactions.
    /// </summary>
    public interface IItemInspectZoneDetector
    {
        /// <summary>
        /// Returns true if the cursor is currently inside the valid inspection zone.
        /// </summary>
        bool IsCursorInZone();
    }
}