using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using aiKart.Dtos.TriviaDtos;
using aiKart.Dtos.DeckDtos;
using aiKart.Interfaces;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using aiKart.Migrations;
using aiKart.Models;
using aiKart.Services;

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriviaController : ControllerBase
    {
        private readonly IDeckService _deckService;
        private readonly IUserDeckService _userDeckService;
        private readonly ICardService _cardService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public TriviaController(IDeckService deckService, IUserDeckService userDeckService, ICardService cardService, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _deckService = deckService;
            _userDeckService = userDeckService;
            _cardService = cardService;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> GetTriviaDeck([FromBody] TriviaDeckRequestDto requestDto)
        {
            if (requestDto.CategoryId < 9 || requestDto.CategoryId > 32)
                return BadRequest("Invalid category.");

            if (requestDto.NumberOfCards < 1 || requestDto.NumberOfCards > 50)
                return BadRequest("Invalid number of cards.");

            var triviaQuestions = await GetTriviaQuestionsAsync(requestDto);

            if (triviaQuestions == null || triviaQuestions.Count == 0)
            {
                return BadRequest("Failed to fetch trivia questions.");
            }

            DecodeHtmlEntities(triviaQuestions);

            var deck = GenerateDeckFromTrivia(triviaQuestions, requestDto.CreatorId);

            return CreatedAtAction(nameof(GetTriviaDeck), new { deckId = deck.Id }, _mapper.Map<DeckDto>(deck));
        }

        private async Task<List<TriviaQuestionDto>> GetTriviaQuestionsAsync(TriviaDeckRequestDto requestDto)
        {
            try
            {
                var apiUrl = $"https://opentdb.com/api.php?amount={requestDto.NumberOfCards}&category={requestDto.CategoryId}&type=multiple";
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetStringAsync(apiUrl);

                var triviaApiResponse = JsonConvert.DeserializeObject<TriviaApiResponseDto>(response);

                if (triviaApiResponse == null || triviaApiResponse.Results == null)
                {
                    return null;
                }

                return triviaApiResponse.Results;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return null;
            }
        }

        private void DecodeHtmlEntities(List<TriviaQuestionDto> triviaQuestions)
        {
            foreach (var question in triviaQuestions)
            {
                question.Category = System.Net.WebUtility.HtmlDecode(question.Category);
                question.Question = System.Net.WebUtility.HtmlDecode(question.Question);
                question.CorrectAnswer = System.Net.WebUtility.HtmlDecode(question.CorrectAnswer);
            }
        }

        private Deck GenerateDeckFromTrivia(List<TriviaQuestionDto> triviaQuestions, int creatorId)
        {
            var deck = new Deck
            {
                Name = $"{triviaQuestions[0].Category}",
                CreatorId = creatorId,
                Description = $"{triviaQuestions.Count} questions about {triviaQuestions[0].Category}",
                IsPublic = false,
            };

            _deckService.AddDeck(deck);
            var userDeck = new UserDeck { UserId = creatorId, DeckId = deck.Id };
            _userDeckService.AddUserDeck(userDeck);

            foreach (var question in triviaQuestions)
            {
                var card = new Card
                {
                    Question = question.Question,
                    Answer = question.CorrectAnswer,
                    DeckId = deck.Id
                };

                _cardService.AddCardToDeck(card);
            }

            return deck;
        }
    }
}
