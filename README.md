# StreakerConsole

StreakerConsole is a console application designed to help users track and manage their habits and streaks. It offers a simple interface for creating new streaks, checking in on existing ones, and viewing progress over time. The application can sync with a remote MySQL database for data persistence and offers offline access to local session data.

## Features

- **User Management**: Load and save user sessions, including streak information.
- **Streak Tracking**: Create new habits and track daily check-ins.
- **Database Integration**: Connects to a MySQL database for storing user data and streaks.
- **Offline Mode**: When offline, users can view local session data without the ability to modify it, ensuring data integrity.
- **Dynamic Configuration**: Utilizes environment variables for sensitive configurations, like database connection strings.
- **Error Handling**: Graceful handling of database connection errors, falling back to local session data.
- **User Interaction**: Provides a command-line interface for user interaction with commands such as:
  - `new`: Create a new streak.
  - `checkin`: Add a day to an existing streak.
  - `list`: List all current streaks.
  - `help`: Display available commands.

## Getting Started

1. Clone the repository.
2. Set up the necessary environment variables for database connection.
3. Build and run the application in your .NET environment.
4. Follow on-screen instructions to start tracking your habits!

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue to discuss potential improvements.

## License

This project is licensed under the MIT License
