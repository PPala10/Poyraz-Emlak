# AirBnbClone - Reservation & iyzico Payment System

This project is a high-performance, modern web application designed as an Airbnb clone, focusing heavily on robust reservation management and seamless enterprise payment gateway integration. Built on top of the **ASP.NET Core MVC** architecture, the platform provides a complete lifecycle for property bookings, asynchronous payment captures, and relational transaction logging.

---

## 🚀 Features

### 1. Reservation & Booking Lifecycle
* **Dynamic Status Tracking:** Bookings are initialized with a `PendingPayment` status, protecting the property availability until financial clearance is obtained.
* **State-Driven UI Actions:** Action interfaces adapt dynamically to booking states (e.g., displaying the **Pay Now** button only during payment windows and swapping it with **Write Review** once confirmed).
* **Graceful Cancellations:** Standardized cancellation requests handled via server-side post-backs.

### 2. Comprehensive iyzico Payment Gateway Integration
* **Hosted Checkout Form Mimarisi:** Utilizes the official iyzico .NET SDK (`Iyzipay`) to render secure, PCI-DSS compliant payment forms natively.
* **Secure Webhook/Callback Infrastructure:** Implements an asynchronous post-payment verification endpoint utilizing `[IgnoreAntiforgeryToken]` and `[FromForm]` payload parsing.
* **Fault-Tolerant Routing:** Overcomes test-environment mapping irregularities by strictly binding stateful transaction keys through precise query string routing (`?reservationId={id}`).

### 3. Database & Architecture Standards
* **Data Persistence:** Relational database integration using **Entity Framework Core** paired with **PostgreSQL** mapping.
* **Entity Framework Optimization:** Deep-loading database associations via multi-level `.Include()` and `.ThenInclude()` statements to prevent lazy-loading performance overheads.
* **Strict Temporal Constraints:** Automated UTC temporal alignment for cross-platform database consistency using explicitly declared timezone specifications (`DateTimeKind.Utc`).

---

## 🛠️ Tech Stack

* **Backend:** .NET 10 / ASP.NET Core MVC
* **Data Access:** Entity Framework Core (EF Core) 10
* **Database:** PostgreSQL
* **External API / SDK:** iyzico .NET SDK (Iyzipay)
* **Frontend Components:** Bootstrap 5, Bootstrap Icons, Razor Syntax (HTML5 / CSS3)

---

## ⚙️ Configuration & Installation

### Prerequisites
Ensure you have the following installed on your local environment (e.g., macOS / Windows):
* [.NET 10 SDK or higher](https://dotnet.microsoft.com/download)
* [PostgreSQL Server](https://www.postgresql.org/download/)

### 1. Clone & Navigate
```bash
git clone [https://github.com/your-username/AirBnbClone.git](https://github.com/your-username/AirBnbClone.git)
cd AirBnbClone/AirBnb
