# Supermarket Management System

A comprehensive C# Desktop Application designed to streamline supermarket operations, including inventory management, sales tracking, and user role management (Admin/Cashier).

## 🚀 Features

* **Authentication:** Secure Login and Registration system for Admins and Cashiers.
* **Admin Dashboard:** Overview of total sales, total categories, and user management.
* **Product Management:** Add, update, and track stock levels of supermarket items.
* **Sales & Billing:** Cashier interface for processing customer orders and generating payments.
* **Reports:** View total sales history and category-wise performance.
* **User Management:** Admin capability to add and manage staff accounts.

## 🛠️ Tech Stack

* **Language:** C#
* **Framework:** .NET (WPF/WinForms)
* **Database:** SQL Server (assuming standard .NET integration)
* **UI:** XAML / Windows Forms

## 📂 Project Structure

* `AdminMainForm`: Main hub for administrative tasks.
* `CashierPaymentForm`: Interface for handling transactions.
* `AddProductForm`: Module for inventory updates.
* `TotalSalesForm`: Reporting and analytics module.

## ⚙️ Installation & Setup

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/YourUsername/SuperMarketManagementSystem.git](https://github.com/YourUsername/SuperMarketManagementSystem.git)
    ```
2.  **Open in Visual Studio:**
    Open the `SuperMarketManagementSystem.csproj` or the `.sln` file.
3.  **Database Configuration:**
    Ensure your SQL Server connection string in `App.config` or `App.xaml.cs` matches your local environment.
4.  **Build & Run:**
    Press `F5` in Visual Studio to compile and launch the application.
