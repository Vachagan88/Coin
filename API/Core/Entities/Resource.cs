using Core.Enums;

namespace Core.Entities;

public class Resource : EntityBase
{
    public ResourceType Type { get; set; }
}
