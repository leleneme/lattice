namespace Lattice.Domain.Dtos;

/// <summary>
///  Generic User Dto to be sent in reponses and services.
/// </summary>
public record struct TeamUpdateDto
{
    public string Name { get; set; }
}