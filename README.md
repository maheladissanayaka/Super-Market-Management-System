# 🛒 Supermarket Management System

> A robust C# Desktop Application for managing supermarket operations, bridging the gap between inventory control and point-of-sale (POS) billing.

## 🌟 Overview
This system provides a dual-interface solution for **Admin** and **Cashier** roles. It streamlines daily retail tasks such as stock management, billing, sales reporting, and user administration, ensuring data accuracy and operational efficiency.

## 🚀 Key Features

### 👮 Admin Module
* **User Management:** Add, update, and manage cashier/admin accounts.
* **Inventory Control:** Add new products, update prices, and monitor stock levels.
* **Categories:** Organize products into categories for better sorting.
* **Sales Reporting:** View total revenue and category-wise sales performance.

### 🧑‍💼 Cashier Module
* **POS Billing:** Fast and efficient interface for processing customer orders.
* **Transaction History:** View daily transactions and generated bills.
* **Receipt Generation:** Automated bill calculation with tax and discounts.

## 🛠️ Tech Stack
* **Language:** C#
* **Framework:** .NET Framework (WinForms/WPF)
* **Database:** SQL Server
* **Tools:** Visual Studio 2022

## 📸 Screenshots
*(Add your screenshots here later by dragging and dropping them into the GitHub editor)*
* **Login Screen:** Secure entry point for staff.
* **Admin Dashboard:** Overview of business performance.
* **Billing System:** The cashier's main interface.

## 🗄️ Database Schema
The system relies on a relational database including tables for:
* `UsersTbl` (ID, Name, Password, Phone)
* `ProductTbl` (ID, Name, Qty, Price, Cat)
* `CategoryTbl` (ID, Name, Desc)
* `BillTbl` (ID, Seller, Date, Amount)

## ⚙️ How to Run Locally

1.  **Clone the Repo:**
    ```bash
    git clone [https://github.com/maheladissanayaka/Super-Market-Management-System.git](https://github.com/maheladissanayaka/Super-Market-Management-System.git)
    ```
2.  **Database Setup:**
    * Open SQL Server Management Studio (SSMS).
    * Run the provided SQL script (if available) or create a database named `smarketdb`.
    * Update the connection string in `App.config` or `App.xaml.cs`.
3.  **Build:**
    * Open the solution in Visual Studio.
    * Press **F5** to build and run.

## 🔮 Future Improvements
* [ ] Barcode Scanner integration.
* [ ] PDF Export for sales reports.
* [ ] Email notifications for low stock.

---
**Developed by [Mahela Dissanayaka](https://github.com/maheladissanayaka)**