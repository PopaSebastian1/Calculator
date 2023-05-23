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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double firstNumber;
        private String operatorSymbol;
        private double secondNumber;
        public MainWindow()
        {
            InitializeComponent();
            firstNumber = int.MinValue; secondNumber = int.MinValue;
        }

        private void OnDigitButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;
            if (button.Content is not string digitString)
                return;
            var isNumber = int.TryParse(digitString, out var digit);
            if (isNumber && digit is >= 0 and <= 9)
                Display.Text += digitString;
        }

        private void OnDeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length == 0)
                return;
            Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
        }

        private void OnOperatorButtonClicked(object sender, RoutedEventArgs e)
        {

            if (sender is not Button button)
                return;
            if (button.Content.ToString() != "=")
            {

                firstNumber = double.Parse(Display.Text);
                operatorSymbol = button.Content.ToString();
                Display.Text = "";
                Expression.Text = firstNumber.ToString() + operatorSymbol;
                return;


            }
            else if (button.Content.ToString() == "=")
            {
                secondNumber = double.Parse(Display.Text);
                operatorSymbol = "=";
                Expression.Text = Expression.Text + secondNumber.ToString() + operatorSymbol;
                Display.Text = calculateExpression(Expression.Text).ToString();
                History.Items.Add(Expression.Text + Display.Text);
                firstNumber = int.MinValue;
                secondNumber = int.MinValue;
                return;
            }

        }
        private double calculateExpression(String expression)
        {
            //count the number of - in the expression
            int negativeNumberCheck = expression.IndexOfAny(new char[] { '-' });
            string[] parts = expression.Split(new[] { '+', '-', 'X', '%', '=' }, StringSplitOptions.RemoveEmptyEntries);
            double x;
            string symbol;
            if (negativeNumberCheck == 0)
            {
                x = -double.Parse(parts[0]);
                symbol = expression.Substring(parts[0].Length + 1, 1);
            }
            else
            {
                x = double.Parse(parts[0]);
                symbol = expression.Substring(parts[0].Length, 1);
            }
            double y = double.Parse(parts[1]);

            double result = 0;
            switch (symbol)
            {
                case "+":
                    result = x + y;
                    break;
                case "-":
                    result = x - y;
                    break;
                case "X":
                    result = x * y;
                    break;
                case "%":
                    result = x / y;
                    break;
                default:
                    Console.WriteLine("Operatorul specificat nu este valid.");
                    break;
            }
            return result;
        }

        private void RestartButton(object sender, RoutedEventArgs e)
        {
            Display.Text = string.Empty;
            Expression.Text = string.Empty;
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificați dacă un element a fost selectat
            if (History.SelectedItem != null)
            {
                // Obțineți textul elementului selectat
                string selectedText = History.SelectedItem.ToString();

                // Puneți textul în TextBox
                Expression.Text = selectedText;
                Display.Text = calculateExpression(selectedText).ToString();
                firstNumber = double.Parse(Display.Text);
            }
        }
    }
}
