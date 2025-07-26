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

        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            PRIdTextBox.Text = "";
            SummaryTextBox.Text = "";
            ManagerTextBox.Text = "";
            EngineerTextBox.Text = "";
            SeverityComboBox.SelectedIndex = -1;
            StatusTextBox.Text = "";
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
            PROriginTextBox.Text = "";
            ProductTextBox.Text = "";
            BlockingTextBox.Text = "";
            RCATextBox.Text = "";
            RCASummaryTextBox.Text = "";
            RCAActionTextBox.Text = "";
            RCATypeTextBox.Text = "";
            VTBFAgeTextBox.Text = "";
            PRDataGrid.SelectedIndex = -1;
        }

        private void CreatePR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = @"
                    INSERT INTO prtable (
                        pr, summary, manager_name, engineer_name, severity, status, tester_name,
                        category, sub_category, meeting_minutes, pr_cr_date, rtr, pr_active,
                        action_owner, age, engineering_comment, num_blocking_tc, pr_release,
                        pr_origin, vf_product, blocking, pr_rca, pr_rcasummary, pr_rca_action,
                        pr_rca_type, vtbf_age
                    ) VALUES (
                        @pr, @summary, @manager_name, @engineer_name, @severity, @status, @tester_name,
                        @category, @sub_category, @meeting_minutes, NOW(), @rtr, @pr_active,
                        @action_owner, @age, @engineering_comment, @num_blocking_tc, @pr_release,
                        @pr_origin, @vf_product, @blocking, @pr_rca, @pr_rcasummary, @pr_rca_action,
                        @pr_rca_type, @vtbf_age
                    )";

                using var cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@pr", Convert.ToInt32(PRIdTextBox.Text));
                cmd.Parameters.AddWithValue("@summary", SummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@manager_name", ManagerTextBox.Text);
                cmd.Parameters.AddWithValue("@engineer_name", EngineerTextBox.Text);
                cmd.Parameters.AddWithValue("@severity", ((ComboBoxItem)SeverityComboBox.SelectedItem)?.Content.ToString());
                cmd.Parameters.AddWithValue("@status", StatusTextBox.Text);
                cmd.Parameters.AddWithValue("@tester_name", TesterTextBox.Text);
                cmd.Parameters.AddWithValue("@category", CategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@sub_category", SubCategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@meeting_minutes", MeetingMinutesTextBox.Text);
                cmd.Parameters.AddWithValue("@rtr", RtrTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_active", PRActiveTextBox.Text);
                cmd.Parameters.AddWithValue("@action_owner", ActionOwnerTextBox.Text);
                cmd.Parameters.AddWithValue("@age", Convert.ToDouble(AgeTextBox.Text));
                cmd.Parameters.AddWithValue("@engineering_comment", EngineeringCommentTextBox.Text);
                cmd.Parameters.AddWithValue("@num_blocking_tc", Convert.ToInt32(NumBlockingTCTextBox.Text));
                cmd.Parameters.AddWithValue("@pr_release", PRReleaseTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_origin", PROriginTextBox.Text);
                cmd.Parameters.AddWithValue("@vf_product", ProductTextBox.Text);
                cmd.Parameters.AddWithValue("@blocking", BlockingTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca", RCATextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rcasummary", RCASummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca_action", RCAActionTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca_type", RCATypeTextBox.Text);
                cmd.Parameters.AddWithValue("@vtbf_age", Convert.ToDouble(VTBFAgeTextBox.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ PR Created Successfully!");
                LoadPRs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error inserting PR: " + ex.Message);
            }
        }

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

                string sql = "SELECT * FROM prtable";
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

        private void UpdatePR_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PRIdTextBox.Text))
            {
                MessageBox.Show("Please enter a PR ID to update.");
                return;
            }

            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = @"
                    UPDATE prtable SET
                        summary = @summary,
                        manager_name = @manager_name,
                        engineer_name = @engineer_name,
                        severity = @severity,
                        status = @status,
                        tester_name = @tester_name,
                        category = @category,
                        sub_category = @sub_category,
                        meeting_minutes = @meeting_minutes,
                        rtr = @rtr,
                        pr_active = @pr_active,
                        action_owner = @action_owner,
                        age = @age,
                        engineering_comment = @engineering_comment,
                        num_blocking_tc = @num_blocking_tc,
                        pr_release = @pr_release,
                        pr_origin = @pr_origin,
                        vf_product = @vf_product,
                        blocking = @blocking,
                        pr_rca = @pr_rca,
                        pr_rcasummary = @pr_rcasummary,
                        pr_rca_action = @pr_rca_action,
                        pr_rca_type = @pr_rca_type,
                        vtbf_age = @vtbf_age
                    WHERE pr = @pr";

                using var cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@pr", Convert.ToInt32(PRIdTextBox.Text));
                cmd.Parameters.AddWithValue("@summary", SummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@manager_name", ManagerTextBox.Text);
                cmd.Parameters.AddWithValue("@engineer_name", EngineerTextBox.Text);
                cmd.Parameters.AddWithValue("@severity", ((ComboBoxItem)SeverityComboBox.SelectedItem)?.Content.ToString());
                cmd.Parameters.AddWithValue("@status", StatusTextBox.Text);
                cmd.Parameters.AddWithValue("@tester_name", TesterTextBox.Text);
                cmd.Parameters.AddWithValue("@category", CategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@sub_category", SubCategoryTextBox.Text);
                cmd.Parameters.AddWithValue("@meeting_minutes", MeetingMinutesTextBox.Text);
                cmd.Parameters.AddWithValue("@rtr", RtrTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_active", PRActiveTextBox.Text);
                cmd.Parameters.AddWithValue("@action_owner", ActionOwnerTextBox.Text);
                cmd.Parameters.AddWithValue("@age", Convert.ToDouble(AgeTextBox.Text));
                cmd.Parameters.AddWithValue("@engineering_comment", EngineeringCommentTextBox.Text);
                cmd.Parameters.AddWithValue("@num_blocking_tc", Convert.ToInt32(NumBlockingTCTextBox.Text));
                cmd.Parameters.AddWithValue("@pr_release", PRReleaseTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_origin", PROriginTextBox.Text);
                cmd.Parameters.AddWithValue("@vf_product", ProductTextBox.Text);
                cmd.Parameters.AddWithValue("@blocking", BlockingTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca", RCATextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rcasummary", RCASummaryTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca_action", RCAActionTextBox.Text);
                cmd.Parameters.AddWithValue("@pr_rca_type", RCATypeTextBox.Text);
                cmd.Parameters.AddWithValue("@vtbf_age", Convert.ToDouble(VTBFAgeTextBox.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ PR Updated.");
                LoadPRs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error updating PR: " + ex.Message);
            }
        }

        private void DeletePR_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PRIdTextBox.Text))
            {
                MessageBox.Show("Please enter a PR ID to delete.");
                return;
            }

            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = "DELETE FROM prtable WHERE pr = @pr";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pr", Convert.ToInt32(PRIdTextBox.Text));
                cmd.ExecuteNonQuery();

                MessageBox.Show("🗑️ PR Deleted.");
                LoadPRs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error deleting PR: " + ex.Message);
            }
        }

        private void PRDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PRDataGrid.SelectedItem is not DataRowView row) return;

            PRIdTextBox.Text = row["pr"].ToString();
            SummaryTextBox.Text = row["summary"].ToString();
            ManagerTextBox.Text = row["manager_name"].ToString();
            EngineerTextBox.Text = row["engineer_name"].ToString();
            StatusTextBox.Text = row["status"].ToString();
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
            PROriginTextBox.Text = row["pr_origin"].ToString();
            ProductTextBox.Text = row["vf_product"].ToString();
            BlockingTextBox.Text = row["blocking"].ToString();
            RCATextBox.Text = row["pr_rca"].ToString();
            RCASummaryTextBox.Text = row["pr_rcasummary"].ToString();
            RCAActionTextBox.Text = row["pr_rca_action"].ToString();
            RCATypeTextBox.Text = row["pr_rca_type"].ToString();
            VTBFAgeTextBox.Text = row["vtbf_age"].ToString();

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
