using _3inRow.Infrastructure;
using _3inRow.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace _3inRow.ViewModel
{
    class MainViewModel : BaseNotifyPropertyChanged
    {
        private Random random { get; set; }

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

        private string congrText;
        public string CongrText
        {
            get => congrText;
            set
            {
                congrText = value;
                Notify();
            }
        }

        private string[,] numbers = new string[9, 9];
        public ICommand Start { get; set; }
        public MainViewModel()
        {
            random = new Random();
            Data = new DataTable();
            InitCommands();
            InitGrid();
        }
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
        private void InitCommands()
        {
            Start = new RelayCommand(async x =>
            {
                do
                {
                    await CheckHorizontal();
                    await Task.Delay(1000);
                    await CheckVertical();
                } while (!IsDone()) ;

                CongrText = "Done!!!";
            });
        }
        private void InitGrid()
        {
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
                    numbers[i, j] = " " + random.Next(0, 3).ToString();
                    var a = Data.Rows[i].ItemArray;
                    a[j] = numbers[i, j];
                    Data.Rows[i].ItemArray = a;
                }
        }
        private bool IsDone()
        {
            for (int i = 8; i > -1; i--)
            {
                for (int j = 0; j < 7; j++)
                    if (numbers[i, j] == numbers[i, j + 1] &&
                        numbers[i, j + 2] == numbers[i, j + 1])
                        return false;
            }
            return true;
        }
        private void Fall(ThreeNumItem item)
        {
            if (item.IsHorizontal)
            {
                for (int k = 0; k < 3; k++)
                    for (int i = item.p[0].i; i > 0; i--)
                        numbers[i, item.p[k].j] = numbers[i - 1, item.p[k].j];
                numbers[0, item.p[0].j] = " " + random.Next(0, 3).ToString();
                numbers[0, item.p[1].j] = " " + random.Next(0, 3).ToString();
                numbers[0, item.p[2].j] = " " + random.Next(0, 3).ToString();
            }
            else
            {
                for (int i = item.p[0].i; i > 0; i--)
                    numbers[i + 2, item.p[0].j] = numbers[i - 1, item.p[0].j];
                numbers[0, item.p[0].j] = " " + random.Next(0, 3).ToString();
                numbers[1, item.p[0].j] = " " + random.Next(0, 3).ToString();
                numbers[2, item.p[0].j] = " " + random.Next(0, 3).ToString();

            }
        }
        private async Task CheckVertical()
        {
            int level = 8;
            for (int j = level; j > -1; j = level)
            {
                bool isFalled = false;
                for (int i = 0; i < 7; i++)
                    if (numbers[i, j] == numbers[i + 1, j] && numbers[i + 2, j] == numbers[i + 1, j])
                    {
                        ThreeNumItem itm = new ThreeNumItem()
                        {
                            IsHorizontal = false,
                            p = new List<ThreeNumItem.Point>() {
                            new ThreeNumItem.Point() { i = i,j=j },
                            new ThreeNumItem.Point() { i = i+1,j=j },
                            new ThreeNumItem.Point() { i = i+2,j=j }

                        }
                        };

                        isFalled = true;
                        await ShowThreeNums(itm);
                        Fall(itm);
                        Print();
                        await Task.Delay(400);
                        break;
                    }
                if (!isFalled)
                    level--;

            }
        }

        private async Task CheckHorizontal()
        {
            int level = 8;
            for (int i = level; i > -1; i = level)
            {
                bool isFalled = false;
                for (int j = 0; j < 7; j++)
                    if (numbers[i, j] == numbers[i, j + 1] && numbers[i, j + 2] == numbers[i, j + 1])
                    {
                        ThreeNumItem itm = new ThreeNumItem()
                        {
                            IsHorizontal = true,
                            p = new List<ThreeNumItem.Point>() {
                            new ThreeNumItem.Point() { i = i,j=j },
                            new ThreeNumItem.Point() { i = i,j=j+1 },
                            new ThreeNumItem.Point() { i = i,j=j+2 }

                        }
                        };

                        isFalled = true;
                        await ShowThreeNums(itm);
                        Fall(itm);
                        Print();
                        await Task.Delay(400);

                        break;
                    }
                if (!isFalled)
                    level--;

            }
        }
        private async Task ShowThreeNums(ThreeNumItem item)
        {
            if (item.IsHorizontal)
            {
                var a = Data.Rows[item.p[0].i].ItemArray;
                a[item.p[0].j] = " -";
                a[item.p[1].j] = " -";
                a[item.p[2].j] = " -";
                Data.Rows[item.p[0].i].ItemArray = a;


                await Task.Delay(400);
                a[item.p[0].j] = "" + numbers[item.p[0].i, item.p[0].j];
                a[item.p[1].j] = "" + numbers[item.p[0].i, item.p[1].j];
                a[item.p[2].j] = "" + numbers[item.p[0].i, item.p[2].j];
                Data.Rows[item.p[0].i].ItemArray = a;
                await Task.Delay(400);
                a[item.p[0].j] = " -";
                a[item.p[1].j] = " -";
                a[item.p[2].j] = " -";
                Data.Rows[item.p[0].i].ItemArray = a;
                await Task.Delay(400);
            }
            else
            {
                var a = Data.Rows[item.p[0].i].ItemArray;
                var a2 = Data.Rows[item.p[1].i].ItemArray;
                var a3 = Data.Rows[item.p[2].i].ItemArray;
                a[item.p[0].j] = " -";
                a2[item.p[1].j] = " -";
                a3[item.p[2].j] = " -";
                Data.Rows[item.p[0].i].ItemArray = a;
                Data.Rows[item.p[1].i].ItemArray = a2;
                Data.Rows[item.p[2].i].ItemArray = a3;


                await Task.Delay(400);
                a[item.p[0].j] = "" + numbers[item.p[0].i, item.p[0].j];
                a2[item.p[1].j] = "" + numbers[item.p[0].i, item.p[1].j];
                a3[item.p[2].j] = "" + numbers[item.p[0].i, item.p[2].j];
                Data.Rows[item.p[0].i].ItemArray = a;
                Data.Rows[item.p[1].i].ItemArray = a2;
                Data.Rows[item.p[2].i].ItemArray = a3;

                await Task.Delay(400);
                a[item.p[0].j] = " -";
                a2[item.p[1].j] = " -";
                a3[item.p[2].j] = " -";
                Data.Rows[item.p[0].i].ItemArray = a;
                Data.Rows[item.p[1].i].ItemArray = a2;
                Data.Rows[item.p[2].i].ItemArray = a3;

                await Task.Delay(400);

            }

        }
       
    }
}
