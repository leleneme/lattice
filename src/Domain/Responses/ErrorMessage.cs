namespace Lattice.Domain.Dtos;

/// <summary>
///  Response type containing a error message. Made to be used in responses with a
///  non 200 (OK) status code
/// </summary>
public record struct ErrorMessage(string Message);