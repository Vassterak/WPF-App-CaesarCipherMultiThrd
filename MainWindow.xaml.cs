﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            'ů' ,'v' , 'w', 'x', 'y', 'ý', 'z', 'ž', ' ', '.', ',', '?', '!', ':', '-', ';'
        }; //Would like to see that in mandarin XD

        SingleThreadCipher cipherSingleThread;
        MultiThreadCipher cipherMultiThread;
        Stopwatch stopWatch;

        private void InitialSetup(int cpuThreads)
        {
            //add number of threads to combobox
            if (cpuThreads > 1)
            {
                for (int i = 2; i <= cpuThreads; i++)
                    comboBoxThreadsSel.Items.Add(i);
            }

            else
            {
                comboBoxThreadsSel.Items.Add("Není podporováno.");
                buttonMultithreading.IsEnabled = false;
            }

            //add shifts to combobox
            for (int i = 1; i <= numberOfCharShifts; i++)
                comboBoxCharShift.Items.Add(i);

            cipherSingleThread = new SingleThreadCipher(alphabetSet);
            cipherMultiThread = new MultiThreadCipher(alphabetSet);
        }
        private bool CheckUserInput()
        {
            if (textboxtInput.Text.Length > 3)
                return true;

            else
            {
                MessageBox.Show("Vstup nesmí být prázdný, nebo příliš krátký!");
                return false;
            }
        }

        private void RunSingleThread()
        {
            if (CheckUserInput())
            {
                stopWatch.Start();
                cipherSingleThread.CharShift = comboBoxCharShift.SelectedIndex + 1;
                textboxtOutput.Text = cipherSingleThread.SingleThreaded(textboxtInput.Text);
                stopWatch.Stop();
                MessageBox.Show("Běh trval: " + stopsWatch.ElapsedMilliseconds.ToString() + " ms");
                stopWatch.Reset();
            }
        }

        private void RunMultiThread()
        {
            if (CheckUserInput())
            {
                stopWatch.Start();
                cipherMultiThread.NumberOfThreads = (int)comboBoxThreadsSel.SelectedValue;
                textboxtOutput.Text = cipherMultiThread.MultiThreaded(textboxtInput.Text);
                stopWatch.Stop();
                MessageBox.Show("Běh trval: " + stopWatch.ElapsedMilliseconds.ToString() + " ms");
                stopWatch.Reset();
            }
        }

        public MainWindow()
        {
            numberOfCpuThreads = Environment.ProcessorCount;
            InitializeComponent();
            InitialSetup(numberOfCpuThreads);
            stopWatch = new Stopwatch();
        }

        private void buttonSingleThread_Click(object sender, RoutedEventArgs e)
        {
            RunSingleThread();
        }

        private void buttonMultithreading_Click(object sender, RoutedEventArgs e)
        {
            RunMultiThread();
        }
    }
}
