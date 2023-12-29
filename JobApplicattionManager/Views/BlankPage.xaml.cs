namespace JobApplicattionManager.Views;

public partial class BlankPage : ContentPage
{
	public BlankPage(BlankViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
