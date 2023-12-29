namespace JobApplicationManager.ViewModels;

public partial class LocalizationViewModel : BaseViewModel
{
	public string LocalizedText => JobApplicationManager.Resources.Strings.AppResources.HelloMessage;
}
