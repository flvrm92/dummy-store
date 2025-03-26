
namespace DummyStore.Data.Models;
public abstract class BaseEntity
{
  public BaseEntity()
  {
    if (Id == Guid.Empty) Id = Guid.CreateVersion7();
  }
  public Guid Id { get; }
}
