using System;
using System.Collections.Generic;
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

namespace CaesarCipher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly int numberOfCpuThreads;
        public const int numberOfCharShifts = 20; //Czech language uses 42 chars. (I use 41 because I'm not counting CH as one letter but as separated letters) And set max shift to 20
        public readonly char[] alphabetSet = new char[] //I know thare is a possble way using it with localization but I wanted to that with array since I cannot use letter to ASCII because of Czech :D
        {
            'a','á' ,'b' ,'c' ,'č' ,'d' ,'ď' ,'e' ,'é' ,'ě' ,'f',
            'g' ,'h' ,'i' ,'í' ,'j' ,'k' ,'l' ,'m' ,'n' ,'ň' ,'o',
            'ó' ,'p' ,'q' ,'r' ,'ř' ,'s','š' ,'t' ,'ť' ,'u' ,'ú' ,
            'ů' ,'v' , 'w', 'x', 'y', 'ý', 'z', 'ž', ' ', '.', ',', '?', '!', ':', '-'
        }; //Would like to see that in mandarin XD

        CaesarEncrytion caesarCipher;

        private void InitialSetup(int cpuThreads)
        {
            //add number of threads to combobox
            for (int i = 1; i <= cpuThreads; i++)
                comboBoxThreadsSel.Items.Add(i);

            //add shifts to combobox
            for (int i = 1; i <= numberOfCharShifts; i++)
                comboBoxCharShift.Items.Add(i);

            caesarCipher = new CaesarEncrytion();
        }

        public MainWindow()
        {
            numberOfCpuThreads = Environment.ProcessorCount;
            InitializeComponent();
            InitialSetup(numberOfCpuThreads);
        }

        private void buttonSingleThread_Click(object sender, RoutedEventArgs e)
        {
            textboxtOutput.Text = caesarCipher.SingleThreaded(textboxtInput.Text);
        }

        private void buttonMultithreading_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
