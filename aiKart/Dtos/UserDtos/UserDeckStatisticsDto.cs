namespace aiKart.Dtos.UserDtos;

public record UserDeckStatisticsDto(
  int TimesSolved,
  int CorrectAnswers,
  int PartiallyCorrectAnswers,
  int IncorrectAnswers
);