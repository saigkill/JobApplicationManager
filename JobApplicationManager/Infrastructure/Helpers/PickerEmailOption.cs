using MailKit.Security;

namespace JobApplicationManager.Infrastructure.Helpers;

public class PickerEmailOption
{
    public static SecureSocketOptions GetSecureSocketOptionObject(string pickerString)
    {
        switch (pickerString)
        {
            case "None":
                return SecureSocketOptions.None;
            case "Auto":
                return SecureSocketOptions.Auto;
            case "SslOnConnect":
                return SecureSocketOptions.SslOnConnect;
            case "StartTls":
                return SecureSocketOptions.StartTls;
            case "StartTlsWhenAvailable":
                return SecureSocketOptions.StartTlsWhenAvailable;
            default:
                return SecureSocketOptions.None;
        }
    }

    public static string GetSecureSocketOptionString(SecureSocketOptions options)
    {
        switch (options)
        {
            case SecureSocketOptions.None:
                return "None";
            case SecureSocketOptions.Auto:
                return "Auto";
            case SecureSocketOptions.SslOnConnect:
                return "SslOnConnect";
            case SecureSocketOptions.StartTls:
                return "StartTls";
            case SecureSocketOptions.StartTlsWhenAvailable:
                return "StartTlsWhenAvailable";
            default:
                return "None";
        }
    }
}
