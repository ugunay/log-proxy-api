using AutoMapper;
using LogProxy.API.DTOs;
using LogProxy.API.Entities;

namespace LogProxy.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageDTO>()
                .ForMember(destination =>
                    destination.Summary,
                    opt => opt.MapFrom(source => source.Title))
                .ForMember(destination =>
                    destination.Message,
                    opt => opt.MapFrom(source => source.Text));

            CreateMap<MessageDTO, Message>()
                .ForMember(destination =>
                    destination.Title,
                    opt => opt.MapFrom(source => source.Summary))
                .ForMember(destination =>
                    destination.Text,
                    opt => opt.MapFrom(source => source.Message))
                .ForMember(destination =>
                    destination.ReceivedAt,
                    opt => opt.MapFrom(source => source.ReceivedAt.ToLocalTime()));
        }
    }
}