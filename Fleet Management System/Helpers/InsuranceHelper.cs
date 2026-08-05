using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Helpers;

public static class InsuranceHelper
{
    public const int ExpiringSoonDays = 30;

    public static InsuranceStatus GetStatus(DateTime expiry, DateTime? asOf = null)
    {
        var today = (asOf ?? DateTime.UtcNow).Date;
        if (expiry.Date < today)
            return InsuranceStatus.Expired;
        if (expiry.Date <= today.AddDays(ExpiringSoonDays))
            return InsuranceStatus.ExpiringSoon;
        return InsuranceStatus.Valid;
    }
}
