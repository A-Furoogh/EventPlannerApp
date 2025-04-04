﻿using EventPlannerApp.Application.Interfaces;
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

            // add ViewModels
            services.AddTransient<QrScanViewModel>();
            services.AddTransient<QrCodeViewModel>();
            services.AddTransient<SignupViewModel>();
            services.AddTransient<MyEventsViewModel>();
            services.AddTransient<ModifyEventViewModel>();
            services.AddTransient<AddEventViewModel>();
            services.AddTransient<AnalyticsViewModel>();
            services.AddTransient<ChatViewModel>();
            services.AddTransient<EventImageViewModel>();
            services.AddTransient<EventViewModel>();

            services.AddTransient<AddEventPage>();
            services.AddTransient<MainPage>();
            services.AddTransient<LoginPage>();
            services.AddTransient<SignupPage>();
            services.AddTransient<EventImagePage>();
            services.AddTransient<QrCodePage>();
            services.AddTransient<ChatsPage>();
            services.AddTransient<AppShell>();
            services.AddTransient<MyEventsPage>();
            services.AddTransient<EventPage>();
            services.AddTransient<ChatPage>();
            services.AddTransient<QrScanPage>();
            services.AddTransient<ModifyEventPage>();
            services.AddTransient<AnalyticsPage>();

            

            services.AddSingleton<IServiceProvider>(provider => provider);

            _ = services.AddSingleton<INavigation>(provider =>
            {
                INavigation? navigation = App.Current?.MainPage?.Navigation;
                return navigation ?? throw new InvalidOperationException("Navigation is not available.");
            });
        }
    }
}
