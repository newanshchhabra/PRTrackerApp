using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PRTrackerApp
{
    public partial class MainWindow : Window
    {
        // ✅ Your MySQL connection string
        private string connStr = "server=localhost;user=root;password=AnshChhabra2110;database=alcatel;";

        public MainWindow()
        {
            InitializeComponent();
        }

        // ✅ CREATE a new PR
        private void CreatePR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = @"INSERT INTO prtable (
                    pr, summary, engineer_name, severity, status,
                    tester_name, category, sub_category, meeting_minutes, pr_cr_date,
                    rtr, pr_active, action_owner, age, engineering_comment,
                    num_blocking_tc, pr_release, pr_origin, vf_product,
                    blocking, pr_rca, pr_rcasummary, pr_rca_action, pr_rca_type, vtbf_age
                ) VALUES (
                    @pr, @summary, @engineer_name, @severity, 'Open',
                    'Tester1', '', '', '', NOW(), 'rtr', 'active', 'owner', 0, '',
                    0, '7.3.4.R02', 'Engineering', 'Product',
                    'F', 'unknown', '', 'noAction', 'Undefined', 0
                )";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pr", new Random().Next(1000, 9999));
                cmd.Parameters.AddWithValue("@summary", SummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@engineer_name", EngineerTextBox.Text);
                cmd.Parameters.AddWithValue("@severity", ((ComboBoxItem)SeverityComboBox.SelectedItem)?.Content.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ PR Created Successfully!");
                LoadPRs(); // Refresh table
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error inserting PR: " + ex.Message);
            }
        }

        // ✅ LOAD all PRs into DataGrid
        private void LoadPRs_Click(object sender, RoutedEventArgs e)
        {
            LoadPRs();
        }

        private void LoadPRs()
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = "SELECT pr, summary, engineer_name, severity, status FROM prtable";
                using var cmd = new MySqlCommand(sql, conn);
                using var adapter = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                PRDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error loading PRs: " + ex.Message);
            }
        }

        // ✅ UPDATE selected PR
        private void UpdatePR_Click(object sender, RoutedEventArgs e)
        {
            if (PRDataGrid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Please select a PR to update.");
                return;
            }

            int prId = Convert.ToInt32(row["pr"]);

            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = @"UPDATE prtable
                               SET summary = @summary,
                                   engineer_name = @engineer_name,
                                   severity = @severity
                               WHERE pr = @pr";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pr", prId);
                cmd.Parameters.AddWithValue("@summary", SummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@engineer_name", EngineerTextBox.Text);
                cmd.Parameters.AddWithValue("@severity", ((ComboBoxItem)SeverityComboBox.SelectedItem)?.Content.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ PR Updated.");
                LoadPRs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error updating PR: " + ex.Message);
            }
        }

        // ✅ DELETE selected PR
        private void DeletePR_Click(object sender, RoutedEventArgs e)
        {
            if (PRDataGrid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Please select a PR to delete.");
                return;
            }

            int prId = Convert.ToInt32(row["pr"]);

            var confirm = MessageBox.Show($"Are you sure you want to delete PR #{prId}?", "Confirm Delete", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = "DELETE FROM prtable WHERE pr = @pr";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pr", prId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("🗑️ PR Deleted.");
                LoadPRs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error deleting PR: " + ex.Message);
            }
        }

        // ✅ Auto-fill fields when selecting a row
        private void PRDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PRDataGrid.SelectedItem is not DataRowView row) return;

            SummaryTextBox.Text = row["summary"].ToString();
            EngineerTextBox.Text = row["engineer_name"].ToString();

            string severity = row["severity"].ToString();
            foreach (ComboBoxItem item in SeverityComboBox.Items)
            {
                if ((string)item.Content == severity)
                {
                    SeverityComboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
