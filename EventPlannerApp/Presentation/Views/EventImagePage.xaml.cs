using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class EventImagePage : ContentPage
{
	public ObservableCollection<String> Images { get; set; }

    public delegate void DataReturnedEventHandler(int? data);
    public event DataReturnedEventHandler? OnDataReturned;
    public EventImagePage()
	{
		InitializeComponent();

        Images = new ObservableCollection<string>
        {
            "event0.png",
            "event1.png",
            "event2.png",
            "event3.png",
            "event4.png",
            "event5.png",
            "event6.png",
        };
        BindingContext = this;
    }

    private void OnImageClicked(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string selectedImage)
        {
            //Take the number from the image name and assign it to the ImageIndex property
            int? ImageIndex = int.Parse(selectedImage.Substring(5, 1));

            OnDataReturned?.Invoke(ImageIndex);

            Navigation.PopAsync();
        }
    }

}