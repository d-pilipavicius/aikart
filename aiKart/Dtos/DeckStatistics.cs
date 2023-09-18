namespace aikart.Dtos;
public record DeckStatistics
(
    int TotalCards,
    int AnsweredCount,
    int UnansweredCount,
    int PartiallyAnsweredCount
);