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
using System.Windows.Shapes;

namespace VanGoghSimulator
{
    /// <summary>
    /// Interakční logika pro Window1.xaml
    /// </summary>
    /// 

    public partial class Window1 : Window
    {
        public bool Success;

        public Color col = Color.FromRgb(0, 0, 0);
        public Window1()
        {
            InitializeComponent();
            rect.Fill = new SolidColorBrush(col);
        }
        private void sli_red_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            fillrect((byte)sli_red.Value, "red");
            txtbox_red.Text = ((byte)sli_red.Value).ToString();
        }

        private void sli_green_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            fillrect((byte)sli_green.Value, "green");
            txtbox_green.Text = ((byte)sli_green.Value).ToString();
        }
        private void sli_blue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            fillrect((byte)sli_blue.Value, "blue");
            txtbox_blue.Text = ((byte)sli_blue.Value).ToString();
        }
        private void txtbox_red_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbox_red.Text))
            {
                return;
            }
            if (byte.TryParse(txtbox_red.Text, out byte val))
            {
                sli_red.Value = val;
                fillrect(val, "red");
            }
            else
            {
                msgbox();
                txtbox_red.Text = ((byte)sli_red.Value).ToString();
            }
        }

        private void txtbox_green_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbox_green.Text))
            {
                return;
            }
            if (byte.TryParse(txtbox_green.Text, out byte val))
            {
                sli_green.Value = val;
                fillrect(val, "green");
            }
            else
            {
                msgbox();
                txtbox_green.Text = ((byte)sli_green.Value).ToString();
            }
        }

        private void txtbox_blue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbox_blue.Text))
            {
                return;
            }
            if (byte.TryParse(txtbox_blue.Text, out byte val))
            {
                sli_blue.Value = val;
                fillrect(val, "blue");
            }
            else
            {
                msgbox();
                txtbox_blue.Text = ((byte)sli_blue.Value).ToString();
            }
        }

        private void fillrect(byte value, string colourName)
        {
            if (value > 255)
            {

            }

            switch (colourName)
            {
                case "red":
                    col.R = value;
                    break;
                case "green":
                    col.G = value;
                    break;
                case "blue":
                    col.B = value;
                    break;
            }

            rect.Fill = new SolidColorBrush(col);
        }

        private void msgbox()
        {
            MessageBox.Show($"Zadejte číslo mezi <0, 255>",
                            ">:(",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        private void sucksess(object sender, RoutedEventArgs e)
        {
            Success = true;
            col.R = (byte)sli_red.Value;
            col.G = (byte)sli_green.Value;
            col.B = (byte)sli_blue.Value;

            Close();
        }
    }
}

