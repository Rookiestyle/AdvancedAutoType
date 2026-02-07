using System;
using System.Reflection;
using KeePassLib;

namespace PluginTools
{
  internal class OTPStub
  {
    private KeePass.Plugins.Plugin _kpotp = null;

    private PropertyInfo _piPlaceholder = null;
    private PropertyInfo _piPlacholderEnter = null;
    private MethodInfo _miHasOTPDefined = null;

    private bool KPOTP = false;

    //taken from EntryUtil.cs
    internal const string OtpSecret = "Secret";
    internal const string OtpSecretHex = OtpSecret + "-Hex";
    internal const string OtpSecretBase32 = OtpSecret + "-Base32";
    internal const string OtpSecretBase64 = OtpSecret + "-Base64";

    internal const string HotpPlh = "{HMACOTP}";
    internal const string HotpPrefix = "HmacOtp-";

    internal const string TotpPlh = "{TIMEOTP}";
    internal const string TotpPrefix = "TimeOtp-";
    
    public string GetOTPPlaceholderForAutotype(PwEntry pe)
    {
      string sPlaceholder = GetKPOTPForAutotype(pe);
      if (string.IsNullOrEmpty(sPlaceholder)) sPlaceholder = GetPlaceholder(pe);
      return sPlaceholder;
    }

    private string GetKPOTPForAutotype(PwEntry pe)
    {
      if (!KPOTP) return string.Empty;
      string sPlaceholder = (string)_piPlaceholder.GetValue(null, null);
      if (string.IsNullOrEmpty(sPlaceholder)) return string.Empty;
      bool? bEnter = (bool?)_piPlacholderEnter.GetValue(null, null);
      if (bEnter.HasValue && bEnter.Value) sPlaceholder += "{ENTER}";
      try
      {
        var otp = _miHasOTPDefined.Invoke(null, new object[] { pe });
        if (otp == null) return string.Empty;
        if (otp.ToString().ToLowerInvariant().Contains("none")) sPlaceholder = string.Empty;
      }
      catch { }
      return sPlaceholder;
    }
    public OTPStub(KeePass.Plugins.Plugin p)
    {
      _kpotp = p;
      if (_kpotp == null) return;
      Type tC = _kpotp.GetType().Assembly.GetType("KeePassOTP.Config");
      if (tC == null) return;
      _piPlaceholder = tC.GetProperty("Placeholder", BindingFlags.NonPublic | BindingFlags.Static);
      _piPlacholderEnter = tC.GetProperty("KPOTPAutoSubmit", BindingFlags.NonPublic | BindingFlags.Static);
      tC = _kpotp.GetType().Assembly.GetType("KeePassOTP.OTPDAO");
      if (tC == null) return;
      _miHasOTPDefined= tC.GetMethod("OTPDefined", BindingFlags.Public | BindingFlags.Static);

      KPOTP = _piPlaceholder != null && _miHasOTPDefined != null && _piPlacholderEnter != null;
    }

    private string GetPlaceholder(PwEntry pe)
    {
      if (pe.Strings.Exists(HotpPrefix + OtpSecret)) return HotpPlh;
      if (pe.Strings.Exists(HotpPrefix + OtpSecretHex)) return HotpPlh;
      if (pe.Strings.Exists(HotpPrefix + OtpSecretBase32)) return HotpPlh;
      if (pe.Strings.Exists(HotpPrefix + OtpSecretBase64)) return HotpPlh;

      if (pe.Strings.Exists(TotpPrefix + OtpSecret)) return TotpPlh;
      if (pe.Strings.Exists(TotpPrefix + OtpSecretHex)) return TotpPlh;
      if (pe.Strings.Exists(TotpPrefix + OtpSecretBase32)) return TotpPlh;
      if (pe.Strings.Exists(TotpPrefix + OtpSecretBase64)) return TotpPlh;

      return string.Empty;
    }
  }

}
