using AutoMapper;
using CoreModel.Model;
using GrpcClient;
using WpfApp.Model;

namespace WpfApp.Map
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Snapshot, StreamingData>();
            CreateMap<Snapshot, streamingRequest>()
                .ForMember(dest => dest.SourcePort, opt => opt.MapFrom(src => src.source_port))
                .ForMember(dest => dest.DestPort, opt => opt.MapFrom(src => src.dest_port))
                .ForMember(dest => dest.SourceIp, opt => opt.MapFrom(src => src.source_ip))
                .ForMember(dest => dest.DestIp, opt => opt.MapFrom(src => src.dest_ip))
                .ForMember(dest => dest.SourceMac, opt => opt.MapFrom(src => src.source_mac))
                .ForMember(dest => dest.DestMac, opt => opt.MapFrom(src => src.dest_mac));
        }
    }
}
