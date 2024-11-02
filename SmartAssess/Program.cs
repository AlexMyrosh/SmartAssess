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
using FluentValidation.AspNetCore;
using Business_Logic_Layer.Models;
using Presentation_Layer.Filters;
using Presentation_Layer.FluentValidator.Account;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using Presentation_Layer.ViewModels.Account;
using Presentation_Layer.FluentValidator.Exam;
using Presentation_Layer.ViewModels.Exam.Shared;
using Presentation_Layer.FluentValidator.Course;
using Presentation_Layer.ViewModels.Course.Shared;
using Presentation_Layer.FluentValidator.ExamAssessment;
using Presentation_Layer.ViewModels.ExamAssessment;
using Presentation_Layer.ViewModels.ExamAssessment.Shared;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;

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

            //app.UseHsts();
            //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseDeveloperExceptionPage();
            app.UseRequestLocalization();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }

        [Obsolete]
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
                options.Lockout.MaxFailedAccessAttempts = 3;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Add this to enable password reset
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
                .AddEntityFrameworkStores<SqlContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromMinutes(15));

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PresentationLayerAndBusinessLayerModelsMapper());
                mc.AddProfile(new DataAccessAndBusinessLogicModesMapper());
            }).CreateMapper());

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-us"),
                    new CultureInfo("uk-ua")
                };

                options.DefaultRequestCulture = new RequestCulture("uk-ua");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });


            services.Configure<OpenAiConfig>(configuration.GetSection("OpenAiConfig"));
            services.Configure<EmailConfig>(configuration.GetSection("EmailSettings"));

            services.AddScoped<IValidator<ChangeEmailViewModel>, ChangeEmailViewModelValidator>();
            services.AddScoped<IValidator<ChangePasswordViewModel>, ChangePasswordViewModelValidator>();
            services.AddScoped<IValidator<ForgotPasswordViewModel>, ForgotPasswordViewModelValidator>();
            services.AddScoped<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddScoped<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
            services.AddScoped<IValidator<ResetPasswordViewModel>, ResetPasswordViewModelValidator>();
            services.AddScoped<IValidator<CourseViewModel>, CourseViewModelValidator>();
            services.AddScoped<IValidator<ViewModels.Exam.ExamViewModel>, ExamViewModelValidator>();
            services.AddScoped<IValidator<QuestionViewModel>, QuestionViewModelValidator>();
            services.AddScoped<IValidator<ExamManualEvaluationViewModel>, ExamManualEvaluationViewModelValidator>();
            services.AddScoped<IValidator<QuestionAnswerViewModel>, QuestionAnswerViewModelValidator>();
            services.AddScoped<IValidator<TakeExamViewModel>, TakeExamViewModelValidator>();

            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserExamPassService, UserExamPassService>();
            services.AddScoped<IOpenAiService, OpenAiService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICourseService, CourseService>();

            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IUserExamPassRepository, UserExamAttemptRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ErrorHandlingFilter>();

            services.AddControllersWithViews(options =>
                {
                    options.ModelValidatorProviders.Clear();
                })
                .AddMvcOptions(option => option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddFluentValidation()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();


            services.AddRazorPages();
        }
    }
}