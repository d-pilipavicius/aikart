namespace aiKart.Dtos.UserDtos;

public record UserCardDto(
  int Id,
  string Question,
  string Answer,
  int IntervalInDays,
  DateTime? LastRepetition
);

