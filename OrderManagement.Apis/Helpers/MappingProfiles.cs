using AutoMapper;
using OrderManagement.Apis.DTOs;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;

namespace OrderManagement.Apis.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<CustomerDto, Customer>().ReverseMap();
            CreateMap<OrderDto, OrderToReturnDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.Status, o => o.MapFrom(S => S.Status))
                .ForMember(d => d.Paymentmethod, o => o.MapFrom(s => s.Paymentmethod))
                ;
        }
    }
}
