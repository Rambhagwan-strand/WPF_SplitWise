using SplitWiseMVVM.Model;
using SplitWiseMVVM.View.Command;
using SplitWiseMVVM.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWiseMVVM.ViewModel
{
    class SplitWiseVM : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private int moneyGiven;

        public int MoneyGiven
        {
            get { return moneyGiven; }
            set 
            {
                if (moneyGiven != value)
                {
                    moneyGiven = value;
                    OnPropertyChanged("MoneyGiven");
                }
                
            }
        }

        

        private double share = 1;

        public double Share
        {
            get { return share; }
            set 
            {
                share = value;
                OnPropertyChanged("Share");
            }
        }

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<string> ResultOfSettlement { get; set; }


        public void addToUsers()
        {
            User user = new User(Name, Convert.ToInt32(MoneyGiven), Share);
            Users.Add(user);
            Name = "";
            MoneyGiven = 0;
            Share = 1;
        }

        public AddTransactionCommand AddTransactionCommand { get; set; }
        public CalculateSettlementCommand CalculateSettlementCommand { get; set; }
        public SplitWiseVM()
        {
            Users = new ObservableCollection<User>();
            ResultOfSettlement = new ObservableCollection<string>();
            AddTransactionCommand = new AddTransactionCommand(this);
            CalculateSettlementCommand = new CalculateSettlementCommand(this);
            
        }




        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PrintSettledShareForOneGroup(ObservableCollection<User> Users, ObservableCollection<string> ResultOfSettlement)
        {
            //No person is there so no settlement required.
            if (Users.Count == 0)
            {
                return;
            }
            ResultOfSettlement.Clear();

            double SumOfRatio = 0;
            double totalMoney = 0;
            double MoneyAdjustmentAfterSettlement = 0;
            double ExpectedFromCurrentPerson;
            string Dialogue;
            for (int i = 0; i < Users.Count; i++)
            {
                SumOfRatio += Users[i].share;
                totalMoney += Users[i].moneyGiven;
            }
            if (SumOfRatio == 0)
            {
                Console.WriteLine("Total sum of share can not be zero");
            }
            else
            {
                ResultOfSettlement.Add("Group Calculation");
                for (int i = 0; i < Users.Count; i++)
                {

                    ExpectedFromCurrentPerson = (double)(Users[i].share / SumOfRatio) * totalMoney;
                    MoneyAdjustmentAfterSettlement = Math.Round(Users[i].moneyGiven - ExpectedFromCurrentPerson, 2);
                    if (MoneyAdjustmentAfterSettlement >= 0)
                    {
                        Dialogue = "need to receive";
                    }
                    else
                    {
                        Dialogue = "need to pay";
                    }
                    string ans = Users[i].name + " " + Dialogue + " " + Convert.ToString(Math.Abs(MoneyAdjustmentAfterSettlement));

                    ResultOfSettlement.Add(ans);
                }
                ResultOfSettlement.Add("***************************************************");

            }

        }
    }
}
