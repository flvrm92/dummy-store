using Marten;

namespace DummyStore.Api.Extensions;

public static class DocumentSessionExtensions
{
  public static Task Add<T>(this IDocumentSession session, Guid id, object @event) where T : class
  {
    session.Events.StartStream<T>(id, @event);
    return session.SaveChangesAsync();
  }
}
