using PhoneNumbers;

namespace NukeProjectUtils.Services.PhoneServices;

internal sealed class PhonesServices
{
    private static readonly Lazy<PhonesServices> _instance = new (() => new PhonesServices());
    public static PhonesServices Instance => _instance.Value;

    public readonly PhoneNumberUtil _phoneUtil;

    private PhonesServices() => _phoneUtil = PhoneNumberUtil.GetInstance();
}