# HostMe Hostel Booking System API

The HostMe Hostel Booking System API is a comprehensive API built with C# using ASP.NET Core 7.0. It provides a complete solution for managing hostel bookings, user roles, and authentication.

## Features

- **JWT Authentication**: The API utilizes JSON Web Tokens (JWT) for user authentication, ensuring secure access to the system.
- **Caching and Throttling Abilities**: The API has caching and throttling to ensure good performance.
- **Role-based Access Control**: The API supports role-based access control with the following roles:

  - **Default**: The default role assigned to new users upon registration. It provides basic access to the system's features.
  - **Resident**: The resident role is assigned to a user who has booked a room and their booking has been approved by the hostel manager.
  - **Manager**: The manager role is assigned by the Super Administrator when creating hostels. Managers have the authority to create, update, and delete rooms within their assigned hostels.
  - **Super Administrator**: The Super Administrator role is assigned to one user upon system startup. This role has the highest level of access and can perform administrative tasks such as creating hostels and assigning managers.

> **Note:** The API is built using Microsoft SQL Server as the default database management system (DBMS). However, it can be adjusted to work with other DBMS of your choice by modifying the database configuration accordingly.

## Endpoints

### Authentication

- **POST** `/api/auth/register`: Allows all users register with the system.
- **POST** `/api/auth/login`: This endpoint is for users to Authenticate and obtain a JWT token to gain system access.

### Booking

- **GET** `/api/book/all`: Accessed by users with the Manager role to retrieve all bookings.
- **POST** `/api/book`: Accessed by users with the Default role to make a booking.

### Hostel

- **GET** `/api/hostel`: Accessible by users with the Default, Super Administrator, and Resident roles to retrieve all hostels.
- **POST** `/api/hostel/hostel`: Accessible by users with the Default, Super Administrator, and Resident roles to retrieve a specific hostel.
- **POST** `/api/hostel/create`: Accessed by the Super Administrator role to create a hostel.
- **PUT** `/api/hostel/update`: Accessed by the Super Administrator role to update a hostel.
- **DELETE** `/api/hostel/delete`: Accessed by the Super Administrator role to delete a hostel.

### Room

- **POST** `/api/room/hostelrooms`: Accessed by users with the Default and Manager roles to retrieve all rooms for a specific hostel.
- **POST** `/api/room/create`: Accessed by users with the Manager role to create a room.
- **PUT** `/api/room/update`: Accessed by users with the Manager role to update a room.
- **DELETE** `/api/room/delete`: Accessed by users with the Manager role to delete a room.

### UserMgt

- **POST** `/api/user/approve-resident`: Accessed by users with the Manager role to approve a resident.
- **POST** `/api/user/roles`: Accessed by the Super Administrator role to update a user's role.
- **GET** `/api/user/all`: Accessed by the Super Administrator role to retrieve all users and their roles.

> **Note:** The access permissions mentioned above are for guidance and may vary based on your specific implementation and authorization logic.
