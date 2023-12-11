using aiKart.Dtos.CardDtos;

namespace aiKart.Dtos.DeckDtos;
public record DeckDto(
  int Id,
  string Name,
  int CreatorId,
  string? Description,
  string? CreatorName,
  bool IsPublic,
  bool UseAnki,
  DateTime CreationDate,
  DateTime LastEditDate,
  ICollection<CardDto>? Cards
);
