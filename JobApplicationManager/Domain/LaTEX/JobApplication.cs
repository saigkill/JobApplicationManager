using BitlyAPI;

namespace JobApplicationManager.Domain.LaTEX;

public class JobApplication
{
    public string URL { get; set; }
    public string Email { get; set; }
    public string Jobtitle { get; set; }
    public string SubjectPrefix { get; set; }

    /// <summary>
    /// That Constructor creates the Application object by using the arguments.
    /// </summary>
    /// <param name="aurl">comes directly from the gui txtUrl.Text</param>
    /// <param name="aemail">comes directly from the gui txtEmail.Text</param>
    /// <param name="ajobtitle">comes directly from the gui txtJobtitle.Text</param>
    public JobApplication(string aurl, string aemail, string ajobtitle)
    {
        URL = aurl;
        Email = aemail;
        Jobtitle = FixJobApplication(ajobtitle);
        SubjectPrefix = "Resources.Strings.AppResources.ApplicationSubjectprefix;";
    }

    /// <summary>
    /// This method gets the jobtitle and escapes the hash symbol and the ampersand. Otherwise it breaks the LaTEX build.
    /// </summary>
    /// <param name="ajobtitle">comes from the constructor.</param>
    /// <returns></returns>
    public string FixJobApplication(string ajobtitle)
    {
        string _prepare = ajobtitle;
        _prepare = _prepare.Replace(@"#", @"\#");
        _prepare = _prepare.Replace(@"&", @"\&");
        return _prepare;
    }

    public async Task UseBitLy(string apkikey, string url)
    {
        var bitly = new Bitly(apkikey);
        var linkResponse = await bitly.PostShorten(url);
        URL = linkResponse.Link;
    }
}
