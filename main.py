import sys
import mysql.connector
from PyQt5.QtWidgets import (
    QApplication, QWidget, QLabel, QLineEdit, QComboBox, QPushButton,
    QVBoxLayout, QHBoxLayout, QTableWidget, QTableWidgetItem, QMessageBox,
    QScrollArea, QFormLayout
)
from PyQt5.QtCore import Qt

class PRTracker(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("PR Tracker")
        self.setGeometry(100, 100, 1200, 800)

        self.db_config = {
    'host': 'localhost',
    'user': 'root',
    'password': 'GotoHell1@3',
    'database': 'alcatel'
}


        # Create UI
        self.create_ui()

    # --------------------------
    # Database Connection Helper
    # --------------------------
    def get_connection(self):
        return mysql.connector.connect(**self.db_config)

    # --------------------------
    # Build the Form UI
    # --------------------------
    def create_ui(self):
        main_layout = QVBoxLayout()

        # Scrollable form
        scroll_area = QScrollArea()
        scroll_widget = QWidget()
        form_layout = QFormLayout()

        # ----- Fields -----
        self.inputs = {}

        def add_text_field(name, label):
            field = QLineEdit()
            form_layout.addRow(QLabel(label), field)
            self.inputs[name] = field

        def add_combo_field(name, label, options):
            combo = QComboBox()
            combo.addItems(options)
            form_layout.addRow(QLabel(label), combo)
            self.inputs[name] = combo

        # Add fields
        add_text_field("pr", "PR ID")
        add_text_field("summary", "Summary")
        add_text_field("manager_name", "Manager Name")
        add_text_field("engineer_name", "Engineer Name")
        add_combo_field("severity", "Severity", ["Critical", "Major", "Minor"])
        add_combo_field("status", "Status", [
            "Canceled","Closed","Duplicate","Evaluating","Feedback","Informational",
            "Monitoring","Open","Verify","Deleted","Deferred"
        ])
        add_text_field("tester_name", "Tester Name")
        add_text_field("category", "Category")
        add_text_field("sub_category", "Sub-Category")
        add_text_field("meeting_minutes", "Meeting Minutes")
        add_text_field("rtr", "RTR")
        add_combo_field("pr_active", "PR Active", ["active","inactive"])
        add_text_field("action_owner", "Action Owner")
        add_text_field("age", "Age")
        add_text_field("engineering_comment", "Engineering Comment")
        add_text_field("num_blocking_tc", "# Blocking TCs")
        add_text_field("pr_release", "Release")
        add_combo_field("pr_origin", "PR Origin", [
            "Testing Engineering","Customer Service","SQA","Engineering","Simulation",
            "HQA","Manufacturing","Dev-AOS-Reuse","MIS","PT-AOS-Reuse","SQA-Delta"
        ])
        add_text_field("vf_product", "Product")
        add_combo_field("blocking", "Blocking", ["T","F"])
        add_combo_field("pr_rca", "RCA", [
            "noRCA","minorCE","largeCE","minorDesign","majorDesign","merge","designComm",
            "duplicate","existing","nofix","collateralDamage","scalability","user",
            "invalidTest","unknown","linux","hw","other","reqUnclear","extern(BCM)","Deferred"
        ])
        add_text_field("pr_rcasummary", "RCA Summary")
        add_combo_field("pr_rca_action", "RCA Action", [
            "noAction","addUT","addPALTest","informReviewer","informArchitecture"
        ])
        add_combo_field("pr_rca_type", "RCA Type", ["Undefined","newCode","Existing","Broken","N/A"])
        add_text_field("vtbf_age", "VTBF Age")

        scroll_widget.setLayout(form_layout)
        scroll_area.setWidget(scroll_widget)
        scroll_area.setWidgetResizable(True)
        main_layout.addWidget(scroll_area)

        # Buttons
        btn_layout = QHBoxLayout()
        create_btn = QPushButton("Create PR")
        create_btn.clicked.connect(self.create_pr)
        load_btn = QPushButton("Load PRs")
        load_btn.clicked.connect(self.load_prs)
        update_btn = QPushButton("Update PR")
        update_btn.clicked.connect(self.update_pr)
        delete_btn = QPushButton("Delete PR")
        delete_btn.clicked.connect(self.delete_pr)

        btn_layout.addWidget(create_btn)
        btn_layout.addWidget(load_btn)
        btn_layout.addWidget(update_btn)
        btn_layout.addWidget(delete_btn)

        main_layout.addLayout(btn_layout)

        # Table
        self.table = QTableWidget()
        main_layout.addWidget(self.table)

        self.setLayout(main_layout)

    # --------------------------
    # CRUD Operations
    # --------------------------
    def create_pr(self):
        try:
            conn = self.get_connection()
            cursor = conn.cursor()

            columns = ",".join(self.inputs.keys())
            placeholders = ",".join(["%s"] * len(self.inputs))
            sql = f"INSERT INTO prtable ({columns}) VALUES ({placeholders})"

            values = [self.get_value(k) for k in self.inputs.keys()]
            cursor.execute(sql, values)
            conn.commit()

            QMessageBox.information(self, "Success", "PR created successfully!")
            self.load_prs()
        except Exception as e:
            QMessageBox.warning(self, "Error", str(e))
        finally:
            conn.close()

    def load_prs(self):
        try:
            conn = self.get_connection()
            cursor = conn.cursor()
            cursor.execute("SELECT * FROM prtable")
            rows = cursor.fetchall()

            # Populate table
            self.table.setRowCount(len(rows))
            self.table.setColumnCount(len(self.inputs))
            self.table.setHorizontalHeaderLabels(self.inputs.keys())

            for i, row in enumerate(rows):
                for j, val in enumerate(row):
                    self.table.setItem(i, j, QTableWidgetItem(str(val)))

        except Exception as e:
            QMessageBox.warning(self, "Error", str(e))
        finally:
            conn.close()

    def update_pr(self):
        try:
            conn = self.get_connection()
            cursor = conn.cursor()

            set_clause = ",".join([f"{k}=%s" for k in self.inputs if k != "pr"])
            sql = f"UPDATE prtable SET {set_clause} WHERE pr=%s"

            values = [self.get_value(k) for k in self.inputs if k != "pr"]
            values.append(self.inputs["pr"].text())  # WHERE pr=?

            cursor.execute(sql, values)
            conn.commit()

            QMessageBox.information(self, "Success", "PR updated successfully!")
            self.load_prs()
        except Exception as e:
            QMessageBox.warning(self, "Error", str(e))
        finally:
            conn.close()

    def delete_pr(self):
        try:
            conn = self.get_connection()
            cursor = conn.cursor()
            cursor.execute("DELETE FROM prtable WHERE pr=%s", (self.inputs["pr"].text(),))
            conn.commit()

            QMessageBox.information(self, "Success", "PR deleted successfully!")
            self.load_prs()
        except Exception as e:
            QMessageBox.warning(self, "Error", str(e))
        finally:
            conn.close()

    # Helper: Get value from field
    def get_value(self, key):
        widget = self.inputs[key]
        if isinstance(widget, QComboBox):
            return widget.currentText()
        return widget.text()

# Run app
if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = PRTracker()
    window.show()
    sys.exit(app.exec_())
