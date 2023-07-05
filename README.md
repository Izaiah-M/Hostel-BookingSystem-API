# HostMe Hostel Booking System API

The HostMe Hostel Booking System API is a comprehensive API built with C# using ASP.NET Core 7.0. It provides a complete solution for managing hostel bookings, user roles, and authentication.

## Features

- **JWT Authentication**: The API utilizes JSON Web Tokens (JWT) for user authentication, ensuring secure access to the system.
- **Role-based Access Control**: The API supports role-based access control with the following roles:
  - Default
  - Resident
  - Manager
  - Super Administrator

## Endpoints

The API provides the following endpoints:

- **Authentication**

  - `POST /api/auth/register`: Register a new user.
  - `POST /api/auth/login`: Authenticate and obtain a JWT token.

- **Booking**

  - `GET /api/book/all`: Get a list of all bookings.
  - `POST /api/book`: Create a new booking.

- **Hostel**

  - `GET /api/hostel`: Get a list of all hostels.
  - `POST /api/hostel/hostel`: Get a single hostel.
  - `POST /api/hostel/create`: Create a new hostel.
  - `PUT /api/hostel/update`: Update details of a specific hostel.
  - `DELETE /api/hostel/delete`: Delete a specific hostel.

- **Room**

  - `POST /api/room/hostelrooms`: Get a list of rooms for a specific hostel.
  - `POST /api/room/create`: Create a new room.
  - `PUT /api/room/update`: Update details of a specific room.
  - `DELETE /api/room/delete`: Delete a specific room.

- **User Management**
  - `POST /api/user/approve-resident`: Approve a user as a resident.
  - `POST /api/user/roles`: Used to update a user's role.
  - `GET /api/user/all`: Get a list of all users.
