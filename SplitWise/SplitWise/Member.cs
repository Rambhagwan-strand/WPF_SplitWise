namespace SplitWise
{
    public class Member
    {
        public string name;
        public int moneyGiven;
        public double share;

        public Member(string customerName, int money, double shareInPercentage)
        {
            this.name = customerName;
            this.moneyGiven = money;
            this.share = shareInPercentage;
        }
    }
}