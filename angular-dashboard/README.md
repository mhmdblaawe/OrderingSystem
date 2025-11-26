# Ordering System Dashboard

A modern Angular dashboard for managing products and customers in the Ordering System.

## Features

- 🔐 **Authentication**: Secure login with JWT tokens
- 📦 **Products Management**: Create, read, update, and delete products
- 👥 **Customers Management**: Create, read, and delete customers
- 🎨 **Modern UI**: Built with Angular Material for a beautiful, responsive design
- 📱 **Responsive Layout**: Works seamlessly on desktop and mobile devices

## Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Angular CLI (v17 or higher)
- .NET 6.0 or higher (for the backend API)

## Installation

1. Navigate to the angular-dashboard directory:
```bash
cd angular-dashboard
```

2. Install dependencies:
```bash
npm install
```

3. Configure the API URL in `src/app/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7000' // Update this to match your API URL
};
```

## Running the Application

1. Start the Angular development server:
```bash
npm start
```

2. Open your browser and navigate to:
```
http://localhost:4200
```

## Project Structure

```
angular-dashboard/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── guards/          # Route guards
│   │   │   ├── interceptors/    # HTTP interceptors
│   │   │   ├── models/          # Data models
│   │   │   └── services/        # Core services
│   │   ├── features/
│   │   │   ├── auth/            # Authentication components
│   │   │   ├── customers/       # Customer management
│   │   │   ├── dashboard/       # Main dashboard layout
│   │   │   └── products/        # Product management
│   │   ├── environments/        # Environment configuration
│   │   ├── app.component.ts     # Root component
│   │   └── app.routes.ts        # Application routes
│   ├── assets/                  # Static assets
│   ├── index.html
│   ├── main.ts                  # Application entry point
│   └── styles.scss              # Global styles
├── angular.json
├── package.json
└── tsconfig.json
```

## API Integration

The dashboard communicates with the .NET API. Make sure:

1. The API is running and accessible
2. CORS is properly configured in the API (already added in Program.cs)
3. The API URL in `environment.ts` matches your API's URL

## Default Login

Use your API's authentication credentials to log in. The JWT token will be stored in localStorage and automatically included in all API requests.

## Building for Production

To build the application for production:

```bash
npm run build
```

The production build will be in the `dist/ordering-system-dashboard` directory.

## Technologies Used

- **Angular 17**: Frontend framework
- **Angular Material**: UI component library
- **RxJS**: Reactive programming
- **TypeScript**: Type-safe JavaScript

## Development

The application uses:
- Standalone components (Angular 17 feature)
- Reactive forms for form handling
- HTTP interceptors for authentication
- Route guards for protected routes
- Material Design components for UI

## Troubleshooting

### CORS Issues
If you encounter CORS errors, make sure:
- The API has CORS configured (already added)
- The API URL in `environment.ts` is correct
- The API is running and accessible

### Authentication Issues
- Check that the API is returning a valid JWT token
- Verify the token is being stored in localStorage
- Check browser console for any errors

## License

This project is part of the Ordering System application.

