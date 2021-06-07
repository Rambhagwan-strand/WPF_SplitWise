using SplitWiseMVVM.Model;
using SplitWiseMVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SplitWiseMVVM.View.Command
{
    class AddTransactionCommand : ICommand
    {
        public SplitWiseVM VM { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AddTransactionCommand(SplitWiseVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //TODO

            VM.addToUsers();
        }
    }
}
