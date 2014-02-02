namespace DndTable.Core.Entities
{
    public interface IEntity
    {
        EntityTypeEnum EntityType { get; }
        Position Position { get; }
        double Angle { get; }
    }

    public enum EntityTypeEnum
    {
        Character, Wall
    }
}
