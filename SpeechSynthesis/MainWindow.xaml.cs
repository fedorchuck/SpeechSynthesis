using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SpeechSynthesis
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Thread thread;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            /*Paragraph outPutString = new Paragraph();
            outPutString.Inlines.Add(logic.GetSyllables());
            richTextBox1.Document.Blocks.Add(outPutString);*/
            thread = new Thread(Run);
            thread.Start();
            //logic.SpeachText(logic.GetSyllables());
        }

        private void Run()
        {
            Logic logic = new Logic();
            Handler obj = new Handler();
            
            String text = obj.HandlerText(new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text);
            
            logic.Dictionary();

            logic.SpeachText(logic.GetSyllables(text));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (thread!=null) thread.Abort();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //File.WriteAllBytes(@"PATH.mp3", audioBytes);  
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (thread != null)
            { 
                thread.Abort();
            }
        }
    }
}
