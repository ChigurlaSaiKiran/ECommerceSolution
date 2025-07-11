# ECommerceSolution

This is a modular **ASP.NET Core Web API** for an **E-Commerce Admin Dashboard**.

It provides secure, maintainable RESTful APIs to manage core business entities like **Products**, **Categories**, **Orders**, and more — following **Clean Architecture** principles.

---

## 🗂️ Project Structure

This solution is organized with multiple projects to keep things clean and maintainable:

ECommerceSolution/
├── ECommerce.API # Web API entry point (ASP.NET Core)
├── ECommerce.Application # Application layer (services, DTOs)
├── ECommerce.Domain # Domain layer (entities, enums, core logic)
├── ECommerce.Infrastructure# Infrastructure (service implementations)
├── ECommerce.Persistence # Data access (EF Core, Repositories)
├── ECommerceSolution.sln # Solution file

yaml
Copy
Edit

---

## 🚀 How to Run

Follow these steps to build and run the API locally:

1. **Clone the repository**
   ```bash
   git clone https://github.com/ChigurlaSaiKiran/ECommerceSolution.git
Open the solution in Visual Studio

Build the solution

Run the API

Right-click ECommerce.API → Set as Startup Project

Click Run

By default, the API will run at:

arduino
Copy
Edit
https://localhost:7050
🔑 Features
✅ JWT Authentication & Authorization
✅ Generic Repository Pattern
✅ Secure RESTful APIs for CRUD operations
✅ Entity Framework Core integration
✅ Clean Architecture best practices
✅ CORS enabled for React frontend connection

🔗 Connect with Frontend
This backend is designed to connect with your React.js Admin Dashboard project.

Example:

javascript
Copy
Edit
const API_BASE_URL = "https://localhost:7050/api";
Make sure your frontend uses this base URL to call API endpoints.

⚙️ Requirements
.NET 8 SDK

Visual Studio 2022 or newer

SQL Server (or your preferred database)

📁 .gitignore
This repo uses a .gitignore to avoid committing temp files:

markdown
Copy
Edit
.vs/
bin/
obj/
*.user
*.suo
*.DS_Store
🤝 Contributing
Feel free to fork this repository, open pull requests, or submit issues!

👤 Author
Chigurla Sai Kiran
🔗 GitHub Profile

📝 License
This project is for learning and personal use.
Feel free to adapt and expand it for your own solutions!


