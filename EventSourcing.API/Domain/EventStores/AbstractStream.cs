using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using System.Text;
using System.Text.Json;

namespace EventSourcing.API.Domain.EventStores
{
  public abstract class AbstractStream
  {
    protected readonly LinkedList<IEvent> Events = new LinkedList<IEvent>();
    private string _streamName { get; }

    public readonly IEventStoreConnection _eventStoreConnetion;

    protected AbstractStream(string streamName, IEventStoreConnection eventStoreConnection)
    {
      _streamName = streamName;
      _eventStoreConnetion = eventStoreConnection;
    }

    public async Task SaveAsync()
    {
      var newEvents = Events.ToList()
                            .Select(x =>  new EventData(Guid.NewGuid()
                          , x.GetType().Name
                          , true
                          , Encoding.UTF8.GetBytes(JsonSerializer.Serialize(x, inputType: x.GetType()))
                          , Encoding.UTF8.GetBytes(x.GetType().FullName))).ToList();
      await _eventStoreConnetion.AppendToStreamAsync(_streamName, ExpectedVersion.Any, newEvents);
      Events.Clear();
    }
  }
}