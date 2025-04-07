using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Application.Services;
using EventPlannerApp.Infrastructure.Repositories;
using EventPlannerApp.Presentation;
using Firebase.Database;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Maui.LifecycleEvents;
using CommunityToolkit.Maui;
using EventPlannerApp.Presentation.ViewModels;
using Plugin.LocalNotification;

namespace EventPlannerApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.ConfigureSyncfusionCore();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .UseMauiCommunityToolkit()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            ConfigureServices(builder.Services);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var firebaseUrl = "https://event-planner-app-ef8b2-default-rtdb.europe-west1.firebasedatabase.app/";
            services.AddSingleton(new FirebaseClient(firebaseUrl));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IChatService, ChatService>();

            services.AddSingleton<NotificationService>();

            services.AddTransient<AddEventPage>();
            services.AddTransient<AddEventViewModel>();

            services.AddTransient<MainPage>();
            services.AddTransient<MainViewModel>();

            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();

            services.AddTransient<SignupPage>();
            services.AddTransient<SignupViewModel>();

            services.AddTransient<EventImagePage>();
            services.AddTransient<EventImageViewModel>();

            services.AddTransient<QrCodePage>();
            services.AddTransient<QrCodeViewModel>();

            services.AddTransient<ChatsPage>();
            services.AddTransient<ChatsViewModel>();

            services.AddTransient<AppShell>();

            services.AddTransient<MyEventsPage>();
            services.AddTransient<MyEventsViewModel>();

            services.AddTransient<EventPage>();
            services.AddTransient<EventViewModel>();

            services.AddTransient<ChatPage>();
            services.AddTransient<ChatViewModel>();

            services.AddTransient<QrScanPage>();
            services.AddTransient<QrScanViewModel>();

            services.AddTransient<ModifyEventPage>();
            services.AddTransient<ModifyEventViewModel>();

            services.AddTransient<AnalyticsPage>();
            services.AddTransient<AnalyticsViewModel>();



            services.AddSingleton<IServiceProvider>(provider => provider);

            _ = services.AddSingleton<INavigation>(provider =>
            {
                INavigation? navigation = App.Current?.MainPage?.Navigation;
                return navigation ?? throw new InvalidOperationException("Navigation is not available.");
            });
        }
    }
}
