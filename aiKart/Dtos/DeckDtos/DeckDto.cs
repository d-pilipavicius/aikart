using aiKart.Dtos.CardDtos;

namespace aiKart.Dtos.DeckDtos;
public record DeckDto(
  int Id,
  string Name, 
  string? Description, 
  string? CreatorName,
  DateTime CreationDate,
  DateTime LastEditDate,
  ICollection<CardDto>? Cards
);
