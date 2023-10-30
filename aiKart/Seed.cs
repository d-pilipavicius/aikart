using aiKart.Models;
using aiKart.Data;

namespace aiKart;

public class Seed
{
    private readonly DataContext dataContext;
    public Seed(DataContext context)
    {
        dataContext = context;
    }
    public void SeedDataContext()
    {
        if (true)
        {
            var adminUser = new User { Name = "admin" };

            var decks = new List<Deck>()
            {
                new Deck()
                {
                    Name = "Math",
                    Description = "Math things",
                    Cards = new List<Card>
                    {
                        new Card
                        {
                            Question = "What is 2 + 2?",
                            Answer = "4",
                        },
                        new Card
                        {
                            Question = "What is the square root of 16?",
                            Answer = "4",
                        },
                        new Card
                        {
                            Question = "What is 5 * 5?",
                            Answer = "25",
                        }
                    }
                },

                new Deck()
                {
                    Name = "Science",
                    Description = "Science stuff",
                    Cards = new List<Card>
                    {
                        new Card
                        {
                            Question = "What is the chemical symbol for oxygen?",
                            Answer = "O",
                        },
                        new Card
                        {
                            Question = "What is the atomic number of hydrogen?",
                            Answer = "1",
                        },
                        new Card
                        {
                            Question = "What is the largest planet in our solar system?",
                            Answer = "Jupiter",
                        },
                        new Card
                        {
                            Question = "What is the chemical formula for water?",
                            Answer = "H2O",
                        }
                    }
                },

                new Deck()
                {

                    Name = "History",
                    Description = "Historical events and figures",
                    Cards = new List<Card>
                    {
                        new Card
                        {
                            Question = "Who was the first President of the United States?",
                            Answer = "George Washington",
                        },
                        new Card
                        {
                            Question = "In which year did Christopher Columbus first voyage to the Americas?",
                            Answer = "1492",
                        },
                        new Card
                        {
                            Question = "What was the capital of the Roman Empire?",
                            Answer = "Rome",
                        }
                    }
                }
            };

            var userDecks = decks.Select(deck => new UserDeck
            {
                User = adminUser,
                Deck = deck
            }).ToList();

            dataContext.Users.Add(adminUser);
            dataContext.Decks.AddRange(decks);
            dataContext.UserDecks.AddRange(userDecks);
            dataContext.SaveChanges();
        }
    }
}

