using Fleet_Management_System.Exceptions;

namespace Fleet_Management_System.Endpoints;

public static class ApiExceptionHandler
{
    public static async Task<IResult> ExecuteAsync(Func<Task<IResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return ToResult(ex);
        }
    }

    public static IResult ToResult(Exception ex) => ex switch
    {
        BusinessException => Results.BadRequest(new { error = ex.Message }),
        KeyNotFoundException => Results.NotFound(new { error = ex.Message }),
        _ => Results.Problem(ex.Message)
    };
}
