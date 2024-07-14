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
            CreateMap<Snapshot, SnifferData>();
            CreateMap<Snapshot, streamingRequest>();
        }
    }
}
