using MongoDB.Bson.Serialization;

namespace SenseTowerEventAPI.Models;

public class ModelMapper
{
    public static void InitRegisterMap()
    {
        BsonClassMap.RegisterClassMap<Event>(cm =>
        {
            cm.AutoMap();
            cm.MapMember(p => p.Title).SetElementName("title_title");
            cm.MapMember(p => p.StartDate).SetElementName("startDate");
            cm.MapMember(p => p.EndDate).SetElementName("endDate");
            cm.MapMember(p => p.Description).SetElementName("description");
            cm.MapMember(p => p.ImageId).SetElementName("imageId");
            cm.MapMember(p => p.SpaceId).SetElementName("spaceId");
            cm.MapMember(p => p.Tickets).SetElementName("tickets");
        });

        BsonClassMap.RegisterClassMap<Ticket>(cm =>
        {
            cm.AutoMap();
            cm.MapMember(p => p.EventId).SetElementName("eventId_event");
            cm.MapMember(p => p.Owner).SetElementName("owner");
            cm.MapMember(p => p.PlaceNumber).SetElementName("placeNumber");
        });
    }
}