using BillTimeLibrary.DataAccess;
using BuildTimeLibrary.DataAccess;
using BuildTimeLibrary.Models;
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

namespace BillTime.Controls
{
    /// <summary>
    /// Interaction logic for DefaultsControl.xaml
    /// </summary>
    public partial class DefaultsControl : UserControl
    {
        public DefaultsControl()
        {
            InitializeComponent();
            LoadDefaultsFromDatabase();
        }

        private void LoadDefaultsFromDatabase()
        {
            string sql = "select * from Defaults";
            DefaultsModel model = SqliteDataAccess.LoadData<DefaultsModel>(sql, new Dictionary<string, object>()).FirstOrDefault();

            if (model != null)
            {
                hourlyRateTextbox.Text = model.HourlyRate.ToString();
                preBillCheckbox.IsChecked = (model.HourlyRate > 0);
                hasCutOffCheckbox.IsChecked = (model.HasCutOff > 0);
                cutoffTextbox.Text = model.CutOff.ToString();
                minimumHoursTextbox.Text = model.MinimumHours.ToString();
                billingIncrementTextbox.Text = model.BillingIncrement.ToString();
                roundUpAfterXMinuteTextbox.Text = model.RoundUpAfterXMinutes.ToString();
            }
            else
            {
                hourlyRateTextbox.Text = "0";
                preBillCheckbox.IsChecked = true;
                hasCutOffCheckbox.IsChecked = false;
                cutoffTextbox.Text = "0";
                minimumHoursTextbox.Text = "0.25";
                billingIncrementTextbox.Text = "0.25";
                roundUpAfterXMinuteTextbox.Text = "0";
            }
        }

        private (bool isValid, DefaultsModel model) ValidateForm()
        {
            bool isValid = true;
            DefaultsModel model = new DefaultsModel();

            try
            {
                model.PreBill = (bool)preBillCheckbox.IsChecked ? 1 : 0;
                model.HasCutOff = (bool)hasCutOffCheckbox.IsChecked ? 1: 0;
                model.HourlyRate = double.Parse(hourlyRateTextbox.Text);
                model.CutOff = int.Parse(cutoffTextbox.Text);
                model.MinimumHours = double.Parse(minimumHoursTextbox.Text);
                model.BillingIncrement = double.Parse(billingIncrementTextbox.Text);
                model.RoundUpAfterXMinutes = int.Parse(roundUpAfterXMinuteTextbox.Text);
            }
            catch 
            {
                isValid = false;
            }
            
            return (isValid, model);
        }

        private void SaveToDatabase(DefaultsModel model)
        {
            string sql = "delete from Defaults";
            SqliteDataAccess.SaveData(sql, new Dictionary<string, object>());

            sql = "insert into Defaults (HourlyRate, PreBill, HasCutOff, CutOff, MinimumHours, BillingIncrement, RoundUpAfterXMinutes) " +
                "values (@HourlyRate, @PreBill, @HasCutOff, @CutOff, @MinimumHours, @BillingIncrement, @RoundUpAfterXMinutes)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@HourlyRate", model.HourlyRate },
                {"@PreBill", model.PreBill },
                {"@HasCutOff", model.HasCutOff },
                {"@CutOff", model.CutOff },
                {"@MinimumHours", model.MinimumHours },
                {"@BillingIncrement", model.BillingIncrement },
                {"@RoundUpAfterXMinutes", model.RoundUpAfterXMinutes },
            };
            
            SqliteDataAccess.SaveData(sql, parameters);
        }
        private void submitForm_Click(object sender, RoutedEventArgs e)
        {
            var form = ValidateForm();

            if (form.isValid == true)
            {
                SaveToDatabase(form.model);
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("The form is not valid. Please check your entries and try again");
                return;
            }
        }
    }
}
