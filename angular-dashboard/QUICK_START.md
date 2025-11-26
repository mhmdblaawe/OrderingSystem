# Quick Start Guide

## Prerequisites
- Node.js 18+ installed
- .NET 6.0+ SDK installed
- SQL Server (or SQL Server Express) running

## Step 1: Start the Backend API

1. Open a terminal in the project root
2. Navigate to the API project:
   ```bash
   cd OrderingSystem.Api
   ```
3. Run the API using the HTTP profile (recommended to avoid redirect issues):
   ```bash
   dotnet run --launch-profile http
   ```
   This will start the API on `http://localhost:5121` only (no HTTPS redirect)
   
   **Alternative**: You can also use `dotnet run`, but make sure it's using the HTTP profile
   
4. Verify the API is running on `http://localhost:5121` (check the console output)
   
   **Important**: 
   - The API must run on HTTP (port 5121) to match the Angular app configuration
   - HTTPS redirection is disabled in development to prevent CORS issues
   - If you see CORS errors, restart the API after configuration changes

## Step 2: Start the Angular Dashboard

1. Open a new terminal
2. Navigate to the Angular dashboard:
   ```bash
   cd angular-dashboard
   ```
3. Install dependencies (first time only):
   ```bash
   npm install
   ```
4. Start the development server:
   ```bash
   npm start
   ```
5. The dashboard will open at `http://localhost:4200`

## Step 3: Login

- Use your API credentials to log in
- The JWT token will be automatically stored and used for all API requests

## Troubleshooting

### Port Conflicts
If port 4200 is already in use, Angular will prompt you to use a different port.

### API Connection Issues / CORS Errors
- **Important**: Make sure the API is running on the **HTTP profile** (port 5121), not HTTPS
- Run the API with: `dotnet run --launch-profile http` or just `dotnet run` (defaults to HTTP)
- Verify the API is running on `http://localhost:5121` (not https)
- Check `src/environments/environment.ts` has the correct API URL (`http://localhost:5121`)
- HTTPS redirection is disabled in development to prevent CORS issues
- If you still see CORS errors, restart the API after configuration changes

### Database Connection
Make sure your SQL Server is running and the connection string in `appsettings.json` is correct.

## Features Available

- **Products**: View, create, edit, and delete products
- **Customers**: View, create, and delete customers
- **Search**: Filter products and customers by various criteria
- **Pagination**: Navigate through large datasets

