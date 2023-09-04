# Appointment Scheduler Project

The Appointment Scheduler Project is a web application that allows users to schedule and manage appointments. It provides features for both administrators and users to create, view, and manage appointments efficiently.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Features

- User Authentication: Secure user registration and login using JWT tokens.
- Admin and User Roles: Different roles with varying levels of access and permissions.
- Appointment Management: Schedule, view, and manage appointments with ease.
- Free Slots Availability: Check available free slots for appointments.
- Notifications: Receive reminders and notifications for upcoming appointments.
- Clean Architecture: Built following the principles of the Clean Architecture.

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/appointment-scheduler.git
   cd appointment-scheduler

1-Install dependencies:

```bash
dotnet restore
```
Configure Database:

Update the connection string in appsettings.json to point to your SQL Server database.

Apply Database Migrations:

```bash
dotnet Update-Database
```

Run the Application:
```bash
dotnet run
```
## Usage
- Visit http://localhost:5033 in your web browser.
- Log in with your credentials or register if you're a new user.
- Users can view and schedule appointments.
- Admins can manage appointments and user roles.

## Contributing
Contributions are welcome! If you'd like to contribute to this project, please follow these steps:
1- Fork the repository.
2- Create a new branch for your feature/fix: git checkout -b feature-name.
3- Make your changes and test thoroughly.
4- Commit your changes: git commit -m "Add feature/fix description".
5- Push to your forked repository: git push origin feature-name.
6- Open a pull request with a detailed description of your changes.

## License
This project is licensed under the MIT License.