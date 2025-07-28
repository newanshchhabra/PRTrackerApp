# PR Tracker Application

This repository contains two implementations of a PR Tracker desktop application:

1. **C# WPF Application** (Windows-only)
2. **Python PyQt5 Application** (Cross-platform: macOS, Linux, Windows)

Both applications provide a GUI for performing full CRUD (Create, Read, Update, Delete) operations on a MySQL table named `prtable` in the `alcatel` database.

---

## Features

- Full CRUD operations on `prtable`
- Scrollable form with **all 26 fields**
- ComboBoxes for MySQL `ENUM` fields (e.g., status, pr_origin, pr_rca)
- DataGrid/Table view for browsing entries
- Parameterized queries for security

---

## Technologies Used

- **C# WPF App:** .NET, XAML
- **Python App:** PyQt5
- **Database:** MySQL
- **Driver:** mysql-connector-python

---

## Project Structure

- **C# WPF App:** MainWindow.xaml, MainWindow.xaml.cs (present within the PRTrackerApp subfolder)
- **Python App:** main.py
- **SQL dump:** prtable.sql
