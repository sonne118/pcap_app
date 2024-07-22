using AutoMapper;
using GrpcClient;
using Server.Model;

namespace Server.Map
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<streamingRequest, Snapshot>()
                .ForMember(dest => dest.source_port, opt => opt.MapFrom(src => src.SourcePort))
                .ForMember(dest => dest.dest_port, opt => opt.MapFrom(src => src.DestPort))
                .ForMember(dest => dest.source_ip, opt => opt.MapFrom(src => src.SourceIp))
                .ForMember(dest => dest.dest_ip, opt => opt.MapFrom(src => src.DestIp))
                .ForMember(dest => dest.source_mac, opt => opt.MapFrom(src => src.SourceMac))
                .ForMember(dest => dest.dest_mac, opt => opt.MapFrom(src => src.DestMac));
        }
    }
}
