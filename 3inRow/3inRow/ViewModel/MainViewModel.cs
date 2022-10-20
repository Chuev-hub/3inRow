using _3inRow.Infrastructure;
using _3inRow.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace _3inRow.ViewModel
{
    class MainViewModel : BaseNotifyPropertyChanged
    {
        private Random random { get; set; } = new Random();
        private DataTable _data;
        public DataTable Data 
        {
            get => _data;
            set
            {
                _data = value;
                Notify();
            }
        }
        private string[,] numbers = new string[9, 9];

        private void Print()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    var a = Data.Rows[i].ItemArray;
                    a[j] = numbers[i, j];
                    Data.Rows[i].ItemArray = a;
                }
        }

        public MainViewModel() 
        {
            Data = new DataTable();
            for (int i = 0; i < 9; i++)
                Data.Rows.Add();
           
            for (int i = 0; i < 9; i++)
            {
                Data.Columns.Add(new DataColumn(Data.Columns.Count.ToString(), typeof(string))
                {
                    AllowDBNull = false,
                    DefaultValue = ""
                });
            }
            for (int i = 0; i < 9; i++)
               for (int j = 0; j < 9; j++)
                {
                    numbers[i, j] = " "+random.Next(0, 3).ToString();
                    var a = Data.Rows[i].ItemArray; 
                    a[j] = numbers[i, j];
                    Data.Rows[i].ItemArray = a;
                }

        }

        ////public ICommand AZCommand { get; private set; }

        //public ObservableCollection<Item> Cars { get; set; } = new ObservableCollection<Item>();
        //public MainViewModel()
        //{
        //    InitCommands();
        //    Window.
        //}

        //private void InitCommands()
        //{

        //    //    ImportCommand = new RelayCommand(x =>
        //    //    {



        //    //    });
        //}
    }
    }
