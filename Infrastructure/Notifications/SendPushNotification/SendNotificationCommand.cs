using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using FirebaseAdmin.Messaging;

namespace EManagementVSA.Infrastructure.Notifications.SendPushNotification;
public class SendNotificationCommand : IRequest<BaseResponse>
{
    public NotificationRequest notificationRequest { get; set; }
}

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, BaseResponse>
{
    public async Task<BaseResponse> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var message = new Message() 
        {
            Notification= new Notification
            {
                Title = request.notificationRequest.Title,
                Body = request.notificationRequest.Body,
            },
            Data = new Dictionary<string, string>()
            {
                ["FirstName"] = "John",
                ["LastName"] = "Doe",
            },
            Token = request.notificationRequest.DeviceToken,
        };

        var messaging = FirebaseMessaging.DefaultInstance;
        var result = await messaging.SendAsync(message, cancellationToken);

        if (!string.IsNullOrEmpty(result))
        {
            return new BaseResponse{
                 Status = true,
                 Message = "Message sent successfuly"
            };
        }
        else {
            throw new EmployeeManagementBadRequestException("Error sending messaage");
        }

    }
}
