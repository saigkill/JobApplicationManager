#pragma warning disable CS8601 // Possible null reference assignment.
namespace JobApplicationManager.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void ButtonSave_Clicked(object sender, EventArgs e)
    {
        _viewModel.SaveSettings();
    }

    private void ButtonLatexPath_Clicked(object sender, EventArgs e)
    {
        _viewModel.GetLatexPath();
        if (_viewModel.LatexPath != null)
        {
            TxtUserLatexPathEntry.Text = _viewModel.LatexPath;
        }
    }
}
