using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PRTrackerApp
{
    public partial class MainWindow : Window
    {
        private string connStr = "server=localhost;user=root;password=AnshChhabra2110;database=alcatel;";

        public MainWindow()
        {
            InitializeComponent();
        }

        // Helper method: Get selected text from ComboBox safely
        private string GetComboBoxValue(ComboBox comboBox)
        {
            return (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
        }

        // Clear all input fields
        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            PRIdTextBox.Text = "";
            SummaryTextBox.Text = "";
            ManagerTextBox.Text = "";
            EngineerTextBox.Text = "";
            SeverityComboBox.SelectedIndex = -1;
            StatusComboBox.SelectedIndex = -1;
            TesterTextBox.Text = "";
            CategoryTextBox.Text = "";
            SubCategoryTextBox.Text = "";
            MeetingMinutesTextBox.Text = "";
            RtrTextBox.Text = "";
            PRActiveTextBox.Text = "";
            ActionOwnerTextBox.Text = "";
            AgeTextBox.Text = "";
            EngineeringCommentTextBox.Text = "";
            NumBlockingTCTextBox.Text = "";
            PRReleaseTextBox.Text = "";
            PROriginComboBox.SelectedIndex = -1;
            ProductTextBox.Text = "";
            BlockingTextBox.Text = "";
            RCATextBox.SelectedIndex = -1;
            RCASummaryTextBox.Text = "";
            RCAActionComboBox.SelectedIndex = -1;
            RCATypeComboBox.SelectedIndex = -1;
            VTBFAgeTextBox.Text = "";
            PRDataGrid.SelectedIndex = -1;
        }

        // Load all PRs
        private void LoadPRs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = "SELECT * FROM prtable";
                using var cmd = new MySqlCommand(sql, conn);
                using var adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                PRDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading PRs: " + ex.Message);
            }
        }

        // Create PR
        private void CreatePR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = @"INSERT INTO prtable (pr, summary, manager_name, engineer_name, severity, status, tester_name, category, sub_category, meeting_minutes, pr_cr_date, rtr, pr_active, action_owner, age, engineering_comment, num_blocking_tc, pr_release, pr_origin, vf_product, blocking, pr_rca, pr_rcasummary, pr_rca_action, pr_rca_type, vtbf_age)
                               VALUES (@pr, @summary, @manager_name, @engineer_name, @severity, @status, @tester_name, @category, @sub_category, @meeting_minutes, NOW(), @rtr, @pr_active, @action_owner, @age, @engineering_comment, @num_blocking_tc, @pr_release, @pr_origin, @vf_product, @blocking, @pr_rca, @pr_rcasummary, @pr_rca_action, @pr_rca_type, @vtbf_age)";

                using var cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@pr", Convert.ToInt32(PRIdTextBox.Text));
                cmd.Parameters.AddWithValue("@summary", SummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@manager_name", ManagerTextBox.Text);
                cmd.Parameters.AddWithValue("@engineer_name", EngineerTextBox.Text);
                cmd.Parameters.AddWithValue("@severity", GetComboBoxValue(SeverityComboBox));
                cmd.Parameters.AddWithValue("@status", GetComboBoxValue(StatusComboBox));
                cmd.Parameters.AddWithValue("@tester_name", TesterTextBox.Text);
                cmd.Parameters.AddWithValue("@category", CategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@sub_category", SubCategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@meeting_minutes", MeetingMinutesTextBox.Text);
                cmd.Parameters.AddWithValue("@rtr", RtrTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_active", PRActiveTextBox.Text);
                cmd.Parameters.AddWithValue("@action_owner", ActionOwnerTextBox.Text);
                cmd.Parameters.AddWithValue("@age", AgeTextBox.Text);
                cmd.Parameters.AddWithValue("@engineering_comment", EngineeringCommentTextBox.Text);
                cmd.Parameters.AddWithValue("@num_blocking_tc", NumBlockingTCTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_release", PRReleaseTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_origin", GetComboBoxValue(PROriginComboBox));
                cmd.Parameters.AddWithValue("@vf_product", ProductTextBox.Text);
                cmd.Parameters.AddWithValue("@blocking", BlockingTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca", GetComboBoxValue(RCATextBox));
                cmd.Parameters.AddWithValue("@pr_rcasummary", RCASummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca_action", GetComboBoxValue(RCAActionComboBox));
                cmd.Parameters.AddWithValue("@pr_rca_type", GetComboBoxValue(RCATypeComboBox));
                cmd.Parameters.AddWithValue("@vtbf_age", VTBFAgeTextBox.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("PR Created Successfully!");
                LoadPRs_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating PR: " + ex.Message);
            }
        }

        // Update PR
        private void UpdatePR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = @"UPDATE prtable SET summary=@summary, manager_name=@manager_name, engineer_name=@engineer_name, severity=@severity, status=@status, tester_name=@tester_name, category=@category, sub_category=@sub_category, meeting_minutes=@meeting_minutes, rtr=@rtr, pr_active=@pr_active, action_owner=@action_owner, age=@age, engineering_comment=@engineering_comment, num_blocking_tc=@num_blocking_tc, pr_release=@pr_release, pr_origin=@pr_origin, vf_product=@vf_product, blocking=@blocking, pr_rca=@pr_rca, pr_rcasummary=@pr_rcasummary, pr_rca_action=@pr_rca_action, pr_rca_type=@pr_rca_type, vtbf_age=@vtbf_age WHERE pr=@pr";

                using var cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@pr", Convert.ToInt32(PRIdTextBox.Text));
                cmd.Parameters.AddWithValue("@summary", SummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@manager_name", ManagerTextBox.Text);
                cmd.Parameters.AddWithValue("@engineer_name", EngineerTextBox.Text);
                cmd.Parameters.AddWithValue("@severity", GetComboBoxValue(SeverityComboBox));
                cmd.Parameters.AddWithValue("@status", GetComboBoxValue(StatusComboBox));
                cmd.Parameters.AddWithValue("@tester_name", TesterTextBox.Text);
                cmd.Parameters.AddWithValue("@category", CategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@sub_category", SubCategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@meeting_minutes", MeetingMinutesTextBox.Text);
                cmd.Parameters.AddWithValue("@rtr", RtrTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_active", PRActiveTextBox.Text);
                cmd.Parameters.AddWithValue("@action_owner", ActionOwnerTextBox.Text);
                cmd.Parameters.AddWithValue("@age", AgeTextBox.Text);
                cmd.Parameters.AddWithValue("@engineering_comment", EngineeringCommentTextBox.Text);
                cmd.Parameters.AddWithValue("@num_blocking_tc", NumBlockingTCTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_release", PRReleaseTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_origin", GetComboBoxValue(PROriginComboBox));
                cmd.Parameters.AddWithValue("@vf_product", ProductTextBox.Text);
                cmd.Parameters.AddWithValue("@blocking", BlockingTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca", GetComboBoxValue(RCATextBox));
                cmd.Parameters.AddWithValue("@pr_rcasummary", RCASummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca_action", GetComboBoxValue(RCAActionComboBox));
                cmd.Parameters.AddWithValue("@pr_rca_type", GetComboBoxValue(RCATypeComboBox));
                cmd.Parameters.AddWithValue("@vtbf_age", VTBFAgeTextBox.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("PR Updated Successfully!");
                LoadPRs_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating PR: " + ex.Message);
            }
        }

        // Delete PR
        private void DeletePR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = "DELETE FROM prtable WHERE pr=@pr";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pr", Convert.ToInt32(PRIdTextBox.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("PR Deleted Successfully!");
                LoadPRs_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting PR: " + ex.Message);
            }
        }

        // Populate fields when a row is selected
        private void PRDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PRDataGrid.SelectedItem is not DataRowView row) return;

            PRIdTextBox.Text = row["pr"].ToString();
            SummaryTextBox.Text = row["summary"].ToString();
            ManagerTextBox.Text = row["manager_name"].ToString();
            EngineerTextBox.Text = row["engineer_name"].ToString();

            // Set ComboBox selections based on text values
            void SetCombo(ComboBox combo, string value)
            {
                foreach (ComboBoxItem item in combo.Items)
                {
                    if ((string)item.Content == value)
                    {
                        combo.SelectedItem = item;
                        break;
                    }
                }
            }

            SetCombo(SeverityComboBox, row["severity"].ToString());
            SetCombo(StatusComboBox, row["status"].ToString());
            TesterTextBox.Text = row["tester_name"].ToString();
            CategoryTextBox.Text = row["category"].ToString();
            SubCategoryTextBox.Text = row["sub_category"].ToString();
            MeetingMinutesTextBox.Text = row["meeting_minutes"].ToString();
            RtrTextBox.Text = row["rtr"].ToString();
            PRActiveTextBox.Text = row["pr_active"].ToString();
            ActionOwnerTextBox.Text = row["action_owner"].ToString();
            AgeTextBox.Text = row["age"].ToString();
            EngineeringCommentTextBox.Text = row["engineering_comment"].ToString();
            NumBlockingTCTextBox.Text = row["num_blocking_tc"].ToString();
            PRReleaseTextBox.Text = row["pr_release"].ToString();
            SetCombo(PROriginComboBox, row["pr_origin"].ToString());
            ProductTextBox.Text = row["vf_product"].ToString();
            BlockingTextBox.Text = row["blocking"].ToString();
            SetCombo(RCATextBox, row["pr_rca"].ToString());
            RCASummaryTextBox.Text = row["pr_rcasummary"].ToString();
            SetCombo(RCAActionComboBox, row["pr_rca_action"].ToString());
            SetCombo(RCATypeComboBox, row["pr_rca_type"].ToString());
            VTBFAgeTextBox.Text = row["vtbf_age"].ToString();
        }
    }
}
