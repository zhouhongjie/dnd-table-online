namespace DndTable.Core.Entities
{
    public interface IEntity
    {
        int Id { get; }
        EntityTypeEnum EntityType { get; }
        Position Position { get; }
        double Angle { get; }
    }

    public enum EntityTypeEnum
    {
        Character, Wall
    }
}
