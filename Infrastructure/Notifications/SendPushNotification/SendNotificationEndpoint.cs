using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Infrastructure.Notifications.SendPushNotification;
public class SendNotificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPost("api/v{v:apiVersion}/send-notification", async (ISender sender, [FromBody] NotificationRequest notificationRequest) => {

            var sendNotificationCommand = new SendNotificationCommand{notificationRequest = notificationRequest};
            return Results.Ok(await sender.Send(sendNotificationCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("SendNotification")
        .WithOpenApi()
        .WithTags("Notification")
        .MapToApiVersion(1);
    }
}