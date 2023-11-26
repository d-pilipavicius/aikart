using aiKart.Models;

namespace aiKart.Dtos.DeckDtos;
public record UpdateDeckDto(string Name, string? Description, string? CreatorName, bool IsPublic, ICollection<Card>? Cards);
