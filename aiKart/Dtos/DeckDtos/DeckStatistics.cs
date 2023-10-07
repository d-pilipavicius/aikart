namespace aiKart.Dtos.DeckDtos;
public record DeckStatistics
(
    int TotalCards,
    int AnsweredCount,
    int UnansweredCount,
    int PartiallyAnsweredCount
);