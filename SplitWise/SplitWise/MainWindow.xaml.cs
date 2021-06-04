using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SplitWise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Member> Persons = new List<Member>();

        //It will store final settlement.
        List<string> Result = new List<string>();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void takeTSVfileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "tsv files (*.tsv)|*.tsv";

            txtBox.Text = Convert.ToString(openFileDialog.ShowDialog());
            //FileStream fs = File.Create();
            if (openFileDialog.ShowDialog() == true)
            {
                //string tsvfile = openFileDialog.FileName;
                string [] tsvfile = File.ReadAllLines(openFileDialog.FileName);
                OutputFinalSettlementForAllGroup(tsvfile, Persons, Result);
            }
            resultListView.ItemsSource = Result;
        }

        private static void OutputFinalSettlementForAllGroup(string[] tsvfile, List<Member> Persons, List<string> Result)
        {
            for (int rowNumber = 1; rowNumber < tsvfile.Length; rowNumber++)
            {
                string[] rowData = tsvfile[rowNumber].Split('\t');

                //Group ended because there is an empty line in tsv file.
                //So we will calculate for current group till now and create new group further
                if (rowData[0].Length == 0)
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Group Calculation");
                    PrintSettledShareForOneGroup(Persons, Result);  //calculate for current group
                    Persons.Clear(); //clearing existing group
                    continue;
                }

                //if share value is not given. Then we are assuming equal share.
                if (rowData[2].Length == 0)
                {
                    rowData[2] = "1";
                }
                Member member = new Member(rowData[0], Convert.ToInt32(rowData[1]), Convert.ToDouble(rowData[2]));
                Persons.Add(member);
            }


            Console.WriteLine(" ");
            Console.WriteLine("Group Calculation");
            PrintSettledShareForOneGroup(Persons, Result);
        }

        private static void PrintSettledShareForOneGroup(List<Member> Persons, List<string> Result)
        {
            //No person is there so no settlement required.
            if (Persons.Count == 0)
            {
                return;
            }

            double SumOfRatio = 0;
            double totalMoney = 0;
            double MoneyAdjustmentAfterSettlement = 0;
            double ExpectedFromCurrentPerson;
            string Dialogue;
            for (int i = 0; i < Persons.Count; i++)
            {
                SumOfRatio += Persons[i].share;
                totalMoney += Persons[i].moneyGiven;
            }
            if (SumOfRatio == 0)
            {
                Console.WriteLine("Total sum of share can not be zero");
            }
            else
            {
                Result.Add("Group Calculation");
                for (int i = 0; i < Persons.Count; i++)
                {

                    ExpectedFromCurrentPerson = (double)(Persons[i].share / SumOfRatio) * totalMoney;
                    MoneyAdjustmentAfterSettlement = Math.Round(Persons[i].moneyGiven - ExpectedFromCurrentPerson, 2);
                    if (MoneyAdjustmentAfterSettlement >= 0)
                    {
                        Dialogue = "need to receive";
                    }
                    else
                    {
                        Dialogue = "need to pay";
                    }
                    string ans = Persons[i].name + " " + Dialogue + " " + Convert.ToString(Math.Abs(MoneyAdjustmentAfterSettlement));

                    Result.Add(ans);
                }
                Result.Add("***************************************************");
                
            }

        }
    }
}
