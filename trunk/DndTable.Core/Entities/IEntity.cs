namespace DndTable.Core.Entities
{
    public interface IEntity
    {
        int Id { get; }
        EntityTypeEnum EntityType { get; }
        Position Position { get; }
        double Angle { get; }

        /// <summary>
        /// Is this entity blocking the way for other entities?
        /// </summary>
        bool IsBlocking { get; } 
    }

    public enum EntityTypeEnum
    {
        Character, Wall
    }
}
