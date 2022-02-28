namespace YazilimNedirBSCToken.Models
{
    public class DashboardViewModel
    {
        public CommissionWalletModel CommissionWalletData { get; set; }
    }

    public class CommissionWalletModel
    {
        public double FormattedBalance { get; set; }
        public string Symbol { get; set; }
    }
}
