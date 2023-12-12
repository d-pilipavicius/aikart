using aiKart.States;

namespace aiKart.Dtos.CardDtos;
public record CardDto(int Id, int DeckId, string Question, string Answer, string State, int IntervalInDays, DateTime? LastRepetition);
