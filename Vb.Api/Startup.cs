using System.Reflection;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vb.Business.Cqrs;
using Vb.Business.Mapper;
using Vb.Data;
using FluentValidation.AspNetCore;
using FluentValidation.Validators;
using Vb.Business.Validators;

namespace AkbankBootCampTaskWeek1;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        string connection = Configuration.GetConnectionString("MsSqlConnection"); //Connection String'i yakalayalım
        services.AddDbContext<VbDbContext>(options => options.UseSqlServer(connection)); //Postgre için useNpgsql

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).GetTypeInfo().Assembly));

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(mapperConfig.CreateMapper());

        services.AddControllers(); //Controllers klasöründeki class'ları ekler
        
        //FluentValidators Install
        services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>(); // register validators
        services.AddFluentValidationAutoValidation(); // the same old MVC pipeline behavior
        services.AddFluentValidationClientsideAdapters(); // for client side
        
        services.AddEndpointsApiExplorer(); // EndPointleri keşfeder. Discovers endpoints
        services.AddSwaggerGen(); //Swagger için bir dökümantasyon hazırlar. Prepares documentation for Swagger
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment()) //If we are working in a development environment, UI is enabled.
        {
            app.UseDeveloperExceptionPage(); //Herhangi bir hata olursa göstersin bize
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(x => { x.MapControllers(); }); //?
    }
}