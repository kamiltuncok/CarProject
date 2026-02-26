// Resolve IResult ambiguity: .NET 7 introduces Microsoft.AspNetCore.Http.IResult
// which conflicts with the project's own Core.Utilities.Results.IResult.
global using IResult = Core.Utilities.Results.IResult;
