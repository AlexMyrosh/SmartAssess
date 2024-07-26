using AutoMapper;
using Business_Logic_Layer.AutoMapperProfiles;
using Business_Logic_Layer.Services.Implementations;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Implementations;
using Data_Access_Layer.Repositories.Interfaces;
using Data_Access_Layer.UnitOfWork.Implementations;
using Data_Access_Layer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation_Layer.AutoMapperProfiles;
using Presentation_Layer.Extensions;

namespace Presentation_Layer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            await app.InitializeWebApplicationAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Configure connection to sql server
            var sqlConnectionString = configuration.GetConnectionString("SmartAssessConnection");
            services.AddDbContext<SqlContext>(options => options.UseSqlServer(sqlConnectionString));

            services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<SqlContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PresentationLayerAndBusinessLayerModelsMapper());
                mc.AddProfile(new DataAccessAndBusinessLogicModesMapper());
            }).CreateMapper());

            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserExamPassService, UserExamPassService>();

            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IUserExamPassRepository, UserExamAttemptRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }
    }
}