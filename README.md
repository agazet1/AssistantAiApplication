# AssistantApp

The project is a prototype AI chatbot that simulates a simple assistant. Users can ask questions, to which the chatbot generates random answers (this could be expanded in the future to include AI-generated answers). Users can rate answers (like/dislike), interrupt them, and view chat history.

## Features
- Sending questions to the chatbot
- Receiving random responses
- Rating responses (like/dislike)
- Interrupting responses
- Viewing chat history
- No user division or authorisation (one user and one chatbot)

## Architecture
- Backend: .NET 8
- Frontend: Angular 17.3.17
- Database: MS SQL Server 2019

## Configuration

**Backend**

In the appsettings.json file, specify:
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_MSSQL_CONNECTION_STRING"
  },
  "FrontendUrl": "http://localhost:4200"
}
```
**Frontend**

In the assets/config/config.json file, set:
```bash
{
  "apiBaseUrl": "https://localhost:7242/"
}
```

## Run Locally

Clone the project from git.

**Backend:**

Make sure you have .NET 8 SDK installed.  
Configure the database connection in appsettings.json.  
Restore NuGet Packages for main project.

Launch the backend with the command:
```bash
dotnet run
```

The API will be available by default at: https://localhost:7242/  
Swagger UI: https://localhost:7242/swagger/index.html

**Frontend:**

```bash
  git clone https://link-to-project
```

Go to the project directory and install dependencies
```bash
  npm install
```

Start the server
```bash
  npm run start
```
or 
```bash
  ng serve
```

The application will be available at: http://localhost:4200