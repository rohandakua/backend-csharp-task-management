﻿namespace PropVivo.Application.Common.Exceptions
{
    public sealed class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException(int userId)
            : base("Not Found", $"The user with the identifier {userId} was not found.")
        {
        }
    }
}