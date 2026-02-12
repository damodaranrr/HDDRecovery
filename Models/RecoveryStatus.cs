namespace HDDRecovery.Models
{
    /// <summary>
    /// Recovery status enumeration
    /// </summary>
    public enum RecoveryStatus
    {
        /// <summary>
        /// File was successfully recovered
        /// </summary>
        Success,
        
        /// <summary>
        /// File was skipped during recovery
        /// </summary>
        Skipped,
        
        /// <summary>
        /// File recovery failed
        /// </summary>
        Failed
    }
}
