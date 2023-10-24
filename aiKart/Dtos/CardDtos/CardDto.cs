using aiKart.States;

namespace aiKart.Dtos.CardDtos;
public record CardDto(int Id, int DeckId, string Question, string Answer, CardState State);