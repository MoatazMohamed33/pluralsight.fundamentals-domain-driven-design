# Pluralsight // Domain-Driven Design Fundamentals

Personal repository for following along with the "Domain-Driven Design Fundamentals" course offered by _Pluralsight_. (https://www.pluralsight.com/courses/fundamentals-domain-driven-design)

_This course will teach you the fundamentals of Domain- Driven Design (DDD) through a demonstration of customer interactions and a complex demo application, along with advice from renowned DDD experts._

# Services

| Service  | Url                                 | Description    |
| -------- | ----------------------------------- | -------------- |
| BaGet    | http://localhost:5555/v3/index.json | NuGet server   |
| Papercut | http://localhost:37408              | Mail server    |
| Seq      | http://localhost:5566/#/events      | Logging server |
| RabbitMQ | http://localhost:15672/             | Message broker |

# Messages

Exchange: `frontdesk-vetclinicpublic`  
RoutingKey: `appointment-scheduled`

```json
{
  "EventType": "AppointmentScheduledIntegrationEvent",
  "AppointmentId": "398df79b-41b5-4488-aef1-b5c1bf74fba1",
  "AppointmentType": "Test",
  "ClientEmailAddress": "rabbit@test.com",
  "ClientName": "Mr. Test",
  "DoctorName": "Dr. Test",
  "PatientName": "Lil' Test",
  "AppointmentStart": "2021-11-01T21:45:00"
}
```
