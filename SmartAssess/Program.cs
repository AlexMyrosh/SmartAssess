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
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation_Layer.AutoMapperProfiles;
using Presentation_Layer.Extensions;
using Presentation_Layer.FluentValidator;
using Presentation_Layer.ViewModels;
using FluentValidation.AspNetCore;
using Business_Logic_Layer.Models;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using Presentation_Layer.Filters;

namespace Presentation_Layer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();
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

            // Configure Logger
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Warning()
                .WriteTo.MSSqlServer(
                    connectionString: sqlConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "LogEvents",
                        AutoCreateSqlTable = true
                    })
                .CreateLogger();

            services.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                // Password settings (if not already configured)
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Add this to enable password reset
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = "Default";
            })
                .AddEntityFrameworkStores<SqlContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(3));

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PresentationLayerAndBusinessLayerModelsMapper());
                mc.AddProfile(new DataAccessAndBusinessLogicModesMapper());
            }).CreateMapper());

            services.Configure<OpenAiConfig>(configuration.GetSection("OpenAiConfig"));
            services.Configure<EmailConfig>(configuration.GetSection("EmailSettings"));
            services.Configure<CommunicationServiceConfig>(configuration.GetSection("CommunicationServiceSettings"));

            services.AddScoped<IValidator<ExamQuestionViewModel>, ExamQuestionViewModelValidator>();
            services.AddScoped<IValidator<ExamViewModel>, ExamViewModelValidator>();
            services.AddScoped<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddScoped<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
            services.AddScoped<IValidator<UserViewModel>, UserViewModelValidator>();
            services.AddScoped<IValidator<ForgotPasswordViewModel>, ForgotPasswordViewModelValidator>();
            services.AddScoped<IValidator<ResetPasswordViewModel>, ResetPasswordViewModelValidator>();
            services.AddScoped<IValidator<ChangePasswordViewModel>, ChangePasswordViewModelValidator>();
            services.AddScoped<IValidator<ChangeEmailViewModel>, ChangeEmailViewModelValidator>();
            services.AddScoped<IValidator<CourseViewModel>, CourseViewModelValidator>();

            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserExamPassService, UserExamPassService>();
            services.AddScoped<IOpenAiService, OpenAiService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IPhoneMessageSender, PhoneMessageSender>();

            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IUserExamPassRepository, UserExamAttemptRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ErrorHandlingFilter>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.AddService(typeof(ErrorHandlingFilter));
            }).AddFluentValidation();
            services.AddRazorPages();
        }
    }
}