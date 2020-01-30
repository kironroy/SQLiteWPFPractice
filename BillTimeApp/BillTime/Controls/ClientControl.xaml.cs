using BillTimeLibrary.DataAccess;
using BuildTimeLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ClientControl.xaml
    /// </summary>
    public partial class ClientControl : UserControl
    {
        ObservableCollection<ClientModel> clients = new ObservableCollection<ClientModel>();
        bool isNewEntry = true;

        public ClientControl()
        {
            InitializeComponent();

            InitializeClientList();

            WireUpClientDropDown();

            ToggleFormFieldsDisplay(false);
        }

        private void WireUpClientDropDown()
        {
            clientDropDown.ItemsSource = clients;
            clientDropDown.DisplayMemberPath = "Name";
            clientDropDown.SelectedValuePath = "Id";
        }

        private void InitializeClientList()
        {
            string sql = "select * from Client order by Name";
            var clientList = SqliteDataAccess.LoadData<ClientModel>(sql, new Dictionary<string, object>());

            clientList.ForEach(x => clients.Add(x));
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            clientStackPanel.Visibility = Visibility.Collapsed;
            editButton.Visibility = Visibility.Collapsed;

            isNewEntry = true;

            LoadDefaults();

            ToggleFormFieldsDisplay(true);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientDropDown.SelectedItem == null)
            {
                MessageBox.Show("Please select a client first.");
                return;
            }

            clientStackPanel.Visibility = Visibility.Collapsed;
            newButton.Visibility = Visibility.Collapsed;

            isNewEntry = false;

            LoadClient();

            ToggleFormFieldsDisplay(true);
        }

        private void LoadClient()
        {
            ClientModel client = (ClientModel)clientDropDown.SelectedItem;

            nameTextbox.Text = client.Name;
            emailTextbox.Text = client.Email;
            hourlyRateTextbox.Text = client.HourlyRate.ToString();
            preBillCheckbox.IsChecked = (client.HourlyRate > 0);
            hasCutOffCheckbox.IsChecked = (client.HasCutOff > 0);
            cutOffTextbox.Text = client.CutOff.ToString();
            minimumHoursTextbox.Text = client.MinimumHours.ToString();
            billingIncrementTextbox.Text = client.BillingIncrement.ToString();
            roundUpAfterXMinuteTextbox.Text = client.RoundUpAfterXMinutes.ToString();
        }

        private void LoadDefaults()
        {
            string sql = "select * from Defaults";
            DefaultsModel model = SqliteDataAccess.LoadData<DefaultsModel>(sql, new Dictionary<string, object>()).FirstOrDefault();

            if (model != null)
            {
                nameTextbox.Text = "";
                emailTextbox.Text = "";
                hourlyRateTextbox.Text = model.HourlyRate.ToString();
                preBillCheckbox.IsChecked = (model.HourlyRate > 0);
                hasCutOffCheckbox.IsChecked = (model.HasCutOff > 0);
                cutOffTextbox.Text = model.CutOff.ToString();
                minimumHoursTextbox.Text = model.MinimumHours.ToString();
                billingIncrementTextbox.Text = model.BillingIncrement.ToString();
                roundUpAfterXMinuteTextbox.Text = model.RoundUpAfterXMinutes.ToString();
            }
            else
            {
                ClearFormData();
            }
        }

        private void ClearFormData()
        {
            nameTextbox.Text = "";
            emailTextbox.Text = "";
            hourlyRateTextbox.Text = "0";
            preBillCheckbox.IsChecked = true;
            hasCutOffCheckbox.IsChecked = false;
            cutOffTextbox.Text = "0";
            minimumHoursTextbox.Text = "0.25";
            billingIncrementTextbox.Text = "0.25";
            roundUpAfterXMinuteTextbox.Text = "0";
        }

        private void submitForm_Click(object sender, RoutedEventArgs e)
        {
            if (isNewEntry == true)
            {
                InsertNewClient();
            }
            else
            {
                UpdateClientRecord();
            }

            ResetForm();
        }

        private (bool isValid, ClientModel model) ValidateForm()
        {
            bool isValid = true;
            ClientModel model = new ClientModel();

            try
            {
                model.Name = nameTextbox.Text;
                model.Email = emailTextbox.Text;
                model.PreBill = (bool)preBillCheckbox.IsChecked ? 1 : 0;
                model.HasCutOff = (bool)hasCutOffCheckbox.IsChecked ? 1 : 0;
                model.HourlyRate = double.Parse(hourlyRateTextbox.Text);
                model.CutOff = int.Parse(cutOffTextbox.Text);
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

        private void InsertNewClient()
        {
            string sql = "insert into Client (Name, HourlyRate, Email, PreBill, HasCutOff, CutOff, MinimumHours, BillingIncrement, RoundUpAfterXMinutes) " +
                "values (@Name, @HourlyRate, @Email, @PreBill, @HasCutOff, @CutOff, @MinimumHours, @BillingIncrement, @RoundUpAfterXMinutes)";

            var form = ValidateForm();

            if (form.isValid == false)
            {
                MessageBox.Show("Invalid form. Please check your data and try again.");
                return;
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Name", form.model.Name },
                { "@HourlyRate", form.model.HourlyRate },
                { "@Email", form.model.Email },
                { "@PreBill", form.model.PreBill },
                { "@HasCutOff", form.model.HasCutOff },
                { "@CutOff", form.model.CutOff },
                { "@MinimumHours", form.model.MinimumHours },
                { "@BillingIncrement", form.model.BillingIncrement },
                { "@RoundUpAfterXMinutes", form.model.RoundUpAfterXMinutes }
            };

            SqliteDataAccess.SaveData(sql, parameters);

            clients.Add(form.model);

            MessageBox.Show("Success");
        }

        private void UpdateClientRecord()
        {
            string sql = "update Client set Name = @Name, HourlyRate = @HourlyRate, Email = @Email, PreBill = @PreBill, HasCutOff = @HasCutOff, " +
                "CutOff = @CutOff, MinimumHours = @MinimumHours, BillingIncrement = @BillingIncrement, RoundUpAfterXMinutes = @RoundUpAfterXMinutes " +
                "where Id = @Id";

            var form = ValidateForm();

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Id", (int)clientDropDown.SelectedValue },
                { "@Name", form.model.Name },
                { "@HourlyRate", form.model.HourlyRate },
                { "@Email", form.model.Email },
                { "@PreBill", form.model.PreBill },
                { "@HasCutOff", form.model.HasCutOff },
                { "@CutOff", form.model.CutOff },
                { "@MinimumHours", form.model.MinimumHours },
                { "@BillingIncrement", form.model.BillingIncrement },
                { "@RoundUpAfterXMinutes", form.model.RoundUpAfterXMinutes }
            };

            SqliteDataAccess.SaveData(sql, parameters);

            ClientModel currentRecord = (ClientModel)clientDropDown.SelectedItem;

            currentRecord.Name = form.model.Name;
            currentRecord.HourlyRate = form.model.HourlyRate;
            currentRecord.Email = form.model.Email;
            currentRecord.PreBill = form.model.PreBill;
            currentRecord.HasCutOff = form.model.HasCutOff;
            currentRecord.CutOff = form.model.CutOff;
            currentRecord.MinimumHours = form.model.MinimumHours;
            currentRecord.BillingIncrement = form.model.BillingIncrement;
            currentRecord.RoundUpAfterXMinutes = form.model.RoundUpAfterXMinutes;

            MessageBox.Show("Success");
        }

        private void ResetForm()
        {
            clientStackPanel.Visibility = Visibility.Visible;
            editButton.Visibility = Visibility.Visible;
            newButton.Visibility = Visibility.Visible;

            isNewEntry = true;

            ClearFormData();

            ToggleFormFieldsDisplay(false);
        }

        private void ToggleFormFieldsDisplay(bool displayFields)
        {
            Visibility display = displayFields ? Visibility.Visible : Visibility.Collapsed;

            nameStackPanel.Visibility = display;
            emailStackPanel.Visibility = display;
            hourlyRateStackPanel.Visibility = display;
            preBillStackPanel.Visibility = display;
            checkboxesStackPanel.Visibility = display;
            incrementsStackPanel.Visibility = display;
            buttonStackPanel.Visibility = display;

        }

        private void clearForm_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }
    }
}
