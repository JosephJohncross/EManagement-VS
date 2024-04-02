namespace EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;

public abstract class EmployeeManagementException : Exception
{
    public EmployeeManagementException(string message) : base(message){}
}

public class EmployeeManagementNotFoundException : EmployeeManagementException
{
    public EmployeeManagementNotFoundException(string message) : base(message){}
}

public class EmployeeManagementBadRequestException : EmployeeManagementException
{
    public EmployeeManagementBadRequestException(string message) : base(message){}
}

public class EmployeeManagementUnAuthorizedException : EmployeeManagementException
{
    public EmployeeManagementUnAuthorizedException(string message) : base(message){}
}

public class EmployeeManagementForbiddenException : EmployeeManagementException
{
    public EmployeeManagementForbiddenException(string message) : base(message){}
}

public class EmployeeManagementServiceNotFound : EmployeeManagementException
{
    public EmployeeManagementServiceNotFound(string message) : base(message){}
}
