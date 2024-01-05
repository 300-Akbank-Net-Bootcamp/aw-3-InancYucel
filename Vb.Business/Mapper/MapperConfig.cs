using AutoMapper;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CustomerRequest, Customer>(); // We also use Request methods such as Post and Put, then we convert Customer.
        CreateMap<Customer, CustomerResponse>(); //Where we receive Select, we will convert the data received from customerAll into Response
        
        CreateMap<AccountTransactionRequest, AccountTransaction>();
        CreateMap<AccountTransaction, AccountTransactionResponse>();
        
        CreateMap<EftTransactionRequest, EftTransaction>();
        CreateMap<EftTransaction, EftTransactionResponse>();
        
        CreateMap<AddressRequest, Address>();
        CreateMap<Address, AddressResponse>()
            .ForMember(dest=> dest.CustomerName,
                src => src.
                    MapFrom(x=> x.Customer.FirstName + " " + x.Customer.LastName));
        //In AddressResponse, concatenate the CustomerName field with the FirstName and LastName of Customer in source.
        //We move the area in the sub model of the model up.
        
        CreateMap<ContactRequest, Contact>();
        CreateMap<Contact, ContactResponse>()
            .ForMember(dest=> dest.CustomerName,
                src => src.
                    MapFrom(x=> x.Customer.FirstName + " " + x.Customer.LastName));
        
        CreateMap<AccountRequest, Account>();
        CreateMap<Account, AccountResponse>()
            .ForMember(dest=> dest.CustomerName,
                src => src.
                    MapFrom(x=> x.Customer.FirstName + " " + x.Customer.LastName));
    }
}