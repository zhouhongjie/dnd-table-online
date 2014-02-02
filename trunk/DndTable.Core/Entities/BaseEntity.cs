namespace DndTable.Core.Entities
{
    internal abstract class BaseEntity : IEntity
    {
        #region IEntity Members

        public abstract EntityTypeEnum EntityType { get; }

        public Position Position { get; internal set; }

        // Dummy implement
        public double Angle { get { return 0.0; } }

        #endregion
    }
}
