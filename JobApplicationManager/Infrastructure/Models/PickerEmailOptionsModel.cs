namespace JobApplicationManager.Infrastructure.Models;

/// <summary>
/// Class PickerEmailOptionsModel.
/// This class represents options for email picker settings in the application.
/// </summary>
public class PickerEmailOptionsModel
{
    public string Option { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public List<PickerEmailOptionsModel> GetOptions()
    {
        var options = new List<PickerEmailOptionsModel>
                      {
                          new PickerEmailOptionsModel() { Option = "None", Value = "None" },
                          new PickerEmailOptionsModel() { Option = "Auto", Value = "Auto" },
                          new PickerEmailOptionsModel() { Option = "SslOnConnect", Value = "SslOnConnect" },
                          new PickerEmailOptionsModel() { Option = "StartTls", Value = "StartTls" },
                          new PickerEmailOptionsModel()
                          {
                              Option = "StartTlsWhenAvailable", Value = "StartTlsWhenAvailable"
                          }
                      };
        return options;
    }
}
