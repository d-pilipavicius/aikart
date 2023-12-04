using aiKart.Dtos.CardDtos;
using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.UserDtos;
using aiKart.Models;
using aiKart.States;
using AutoMapper;

namespace aiKart.Helper;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Deck, DeckDto>();
        CreateMap<AddDeckDto, Deck>();
        CreateMap<UpdateDeckDto, Deck>();

        CreateMap<Card, CardDto>();
        CreateMap<CardDto, Card>();

        CreateMap<AddCardDto, Card>();
        CreateMap<UpdateCardDto, Card>();

        CreateMap<CardStateDto, Card>();
        CreateMap<Card, CardStateDto>();

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();

        CreateMap<UserDeck, UserDeckDto>();
        CreateMap<UserDeckDto, UserDeck>();

        CreateMap<User, UserResponseDto>();

        CreateMap<Card, UserCardDto>();
    }
}