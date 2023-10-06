using aiKart.Dtos.CardDtos;
using aiKart.Dtos.DeckDtos;
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

        CreateMap<Card, CardDto>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()));
        CreateMap<AddCardDto, Card>();
        CreateMap<UpdateCardDto, Card>();

        CreateMap<CardStateDto, Card>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => Enum.Parse<CardState>(src.State)));

    }
}