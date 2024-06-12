using AutoMapper;
using CoreModel.Model;
using WpfApp.Model;

namespace WpfApp.Map
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Snapshot,SnifferData>();
        }
    }
}
