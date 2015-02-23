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

namespace SpeechSynthesis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Logic logic = new Logic();
            logic.Text = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text.ToLower();

            Paragraph outPutString = new Paragraph();
            outPutString.Inlines.Add(logic.tmp());
            richTextBox1.Document.Blocks.Add(outPutString);

            logic.Dictionary();
            logic.GetSyllables();
            logic.CreateSpeach();
            logic.SpeachText();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
