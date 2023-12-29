using Syncfusion.Maui.Picker;

#pragma warning disable CS8601 // Possible null reference assignment.
namespace JobApplicationManager.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;
    private readonly Services.IFolderPicker _folderPicker;

    public SettingsPage(SettingsViewModel viewModel, Services.IFolderPicker folderPicker)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _folderPicker = folderPicker;
        BindingContext = _viewModel;
    }

    private void ButtonSave_Clicked(object sender, EventArgs e)
    {
        _viewModel.SaveSettings();
    }

    private void ButtonLatexPath_Clicked(object sender, EventArgs e)
    {
        _viewModel.LatexPath = _folderPicker.PickFolder().ToString();
    }

    private void picker_SelectionChanged(object? sender, PickerSelectionChangedEventArgs pickerSelectionChangedEventArgs)
    {
        _viewModel.picker_SelectionChanged();
    }

    private void picker_OkButtonClicked(object? sender, EventArgs eventArgs)
    {
        _viewModel.picker_OkButtonClicked();
    }
}
