using UAParser;

namespace NukeProjectUtils.ValueObjects.Base;

public record UserAgentInfo
{
    public string RawValue { get; set; }
    public string? Browser { get; set; }
    public string? BrowserMajor { get; set; }
    public string? System { get; set; }
    public string? SystemMajor { get; set; }
    public string? Device { get; set; }
    public string? DeviceBrand { get; set; }
    public bool? SpiderOrBot { get; set; }

    private UserAgentInfo() { RawValue = null!; }

    public UserAgentInfo(string userAgentComplete)
    {
        RawValue = userAgentComplete;

        ClientInfo client = Parser.GetDefault().Parse(userAgentComplete);

        Browser = client.UA.Family;
        BrowserMajor = client.UA.Major;
        System = client.OS.Family;
        SystemMajor = client.OS.Major;
        Device = client.Device.Family;
        DeviceBrand = client.Device.Brand;
        SpiderOrBot = client.Device.IsSpider;
    }
}
