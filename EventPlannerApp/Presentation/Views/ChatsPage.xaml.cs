using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation.ViewModels;
using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class ChatsPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
public ChatsPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
    }
    private void OnUserSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        //var searchText = e.NewTextValue.ToLower();
        //FilteredUsers?.Clear();

        //if (String.IsNullOrWhiteSpace(searchText))
        //{
        //    UserSearchResults.IsVisible = false;
        //    return;
        //}

        //var results = Users.Where(u => u.Name.ToLower().Contains(searchText)).ToList();

        //foreach (var user in results)
        //{
        //    FilteredUsers.Add(user);
        //}

        UserSearchResults.IsVisible = true;
    }
}