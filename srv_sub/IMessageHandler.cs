﻿namespace srv_sub
{
    public interface IMessageHandler
    {
        Task HandleAsync(Message message, CancellationToken cancellationToken);
    }
}