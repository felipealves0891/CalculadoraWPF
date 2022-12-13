using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CalculadoraWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Controller _controller;
        private Model _model;

        public MainWindow()
        {
            InitializeComponent();
            _controller = new Controller();
            _model = new Model();
        }

        private void Numeral_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if(button is null)
                return;

            var content = button.Content.ToString();
            _model = _controller.SetNumeral(_model, content);
            txtFormula.Text = _model.ToString();
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button is null)
                return;

            var content = button.Content.ToString();
            _model = _controller.SetOperation(_model, content);
            txtFormula.Text = _model.ToString();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = _controller.Calculate(_model).ToString();
            _model = new Model();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearTheLast_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button is null)
                return;

            _model = _controller.ClearTheLast(_model);
            txtFormula.Text = _model.ToString();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _model = new Model();
            txtFormula.Text = "";
        }

    }

    public class Model
    {
        public string Numerador { get; set; } = string.Empty;

        public string Denumerador { get; set; } = string.Empty;

        public string Operation { get; set; } = string.Empty;

        public double Result => Operation switch
        {
            "+" => double.Parse(Numerador) + double.Parse(Denumerador),
            "-" => double.Parse(Numerador) - double.Parse(Denumerador),
            "/" => double.Parse(Numerador) / double.Parse(Denumerador),
            "*" => double.Parse(Numerador) * double.Parse(Denumerador),
            _ => throw new InvalidOperationException($"Operação invalida: {Operation}")
        };

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Numerador) 
                && !string.IsNullOrEmpty(Denumerador) 
                && !string.IsNullOrEmpty(Operation);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Numerador);
            builder.Append(" ");
            builder.Append(Operation);
            builder.Append(" ");
            builder.Append(Denumerador);
            builder.Append(" = ");
            return builder.ToString();
        }

    }

    public class Controller
    {
        public double Calculate(Model model)
        {
            if (!model.IsValid())
                throw new InvalidOperationException("Falta informações para efetuar o calculo!");

            return model.Result;
        }
        
        public Model SetOperation(Model model, string? operation)
        {
            if (operation == "+" || operation == "-" || operation == "/" || operation == "*")
            {
                model.Operation = operation;
            }

            return model;
        }

        public Model SetNumeral(Model model, string? numeral)
        {
            if (string.IsNullOrEmpty(numeral))
                return model;

            if (string.IsNullOrEmpty(model.Operation.ToString()))
            {
                model.Numerador += numeral;
            }
            else
            {
                model.Denumerador += numeral;
            }

            return model;
        }

        public Model ClearTheLast(Model model)
        {
            if (!string.IsNullOrEmpty(model.Denumerador.ToString()))
            {
                if (model.Denumerador.Length > 0)
                    model.Denumerador = model.Denumerador.Substring(0, model.Denumerador.Length - 1);
            }
            else if (!string.IsNullOrEmpty(model.Operation.ToString()))
            {
                model.Operation = "";
            }
            else
            {
                if (model.Numerador.Length > 0)
                    model.Numerador = model.Numerador.Substring(0, model.Numerador.Length - 1);
            }

            return model;
        }

    }

}
