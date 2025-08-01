# PropVivo Task and Attendance Management System - API Endpoints

## Base URL
```
https://localhost:7001/api
```

## Authentication
All endpoints except `/auth/login` and `/auth/register` require authentication via Bearer token.

## 1. Authentication Endpoints

### Login
- **POST** `/auth/login`
- **Description**: Authenticate user and get access token
- **Request Body**:
```json
{
  "username": "string",
  "password": "string"
}
```
- **Response**: LoginResponse with JWT token

### Register
- **POST** `/auth/register`
- **Description**: Register new user (Superior/Admin only)
- **Authorization**: Superior, Admin
- **Request Body**:
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "firstName": "string",
  "lastName": "string",
  "role": "Employee|Superior|Admin",
  "superiorId": "string"
}
```

### Logout
- **POST** `/auth/logout`
- **Description**: Logout user (invalidate token)
- **Authorization**: All authenticated users

## 2. Dashboard Endpoints

### Get User Dashboard
- **GET** `/dashboard`
- **Description**: Get current user's dashboard data
- **Authorization**: All authenticated users
- **Response**: DashboardResponse with tasks, time tracking, and stats

### Get Superior Dashboard
- **GET** `/dashboard/superior`
- **Description**: Get superior's dashboard with team overview
- **Authorization**: Superior, Admin

### Get Dashboard Stats
- **GET** `/dashboard/stats?startDate={date}&endDate={date}`
- **Description**: Get detailed statistics for date range
- **Authorization**: All authenticated users

## 3. Task Management Endpoints

### Get All Tasks
- **GET** `/task?assignedToId={userId}`
- **Description**: Get tasks assigned to user (defaults to current user)
- **Authorization**: All authenticated users

### Get Task by ID
- **GET** `/task/{id}`
- **Description**: Get specific task details
- **Authorization**: All authenticated users

### Create Task
- **POST** `/task`
- **Description**: Create new task (Superior/Admin only)
- **Authorization**: Superior, Admin
- **Request Body**:
```json
{
  "title": "string",
  "description": "string",
  "assignedToId": "string",
  "estimatedHours": 8.0,
  "priority": "Low|Medium|High|Critical",
  "isSelfAdded": false
}
```

### Create Self Task
- **POST** `/task/self`
- **Description**: Create task for self (Employee only)
- **Authorization**: Employee
- **Request Body**: Same as Create Task

### Update Task
- **PUT** `/task/{id}`
- **Description**: Update task details
- **Authorization**: Task owner or Superior/Admin
- **Request Body**:
```json
{
  "title": "string",
  "description": "string",
  "estimatedHours": 8.0,
  "priority": "Low|Medium|High|Critical",
  "status": "Assigned|InProgress|Paused|Completed|Cancelled"
}
```

### Delete Task
- **DELETE** `/task/{id}`
- **Description**: Delete task
- **Authorization**: Superior, Admin

### Start Task
- **POST** `/task/{id}/start`
- **Description**: Start working on a task
- **Authorization**: Employee

### Complete Task
- **POST** `/task/{id}/complete`
- **Description**: Mark task as completed
- **Authorization**: Employee

## 4. Time Tracking Endpoints

### Start Time Tracking
- **POST** `/timetracking/start`
- **Description**: Start tracking time for a task
- **Authorization**: Employee
- **Request Body**:
```json
{
  "taskId": "string"
}
```

### Stop Time Tracking
- **POST** `/timetracking/stop`
- **Description**: Stop current time tracking
- **Authorization**: Employee

### Pause Time Tracking
- **POST** `/timetracking/pause`
- **Description**: Pause current time tracking
- **Authorization**: Employee

### Resume Time Tracking
- **POST** `/timetracking/resume`
- **Description**: Resume paused time tracking
- **Authorization**: Employee

### Get Current Time Tracking
- **GET** `/timetracking/current`
- **Description**: Get active time tracking session
- **Authorization**: Employee

### Get Time Tracking History
- **GET** `/timetracking/history?startDate={date}&endDate={date}`
- **Description**: Get time tracking history for date range
- **Authorization**: All authenticated users

### Get Today's Time Tracking
- **GET** `/timetracking/today`
- **Description**: Get today's time tracking data
- **Authorization**: All authenticated users

## 5. Break Management Endpoints

### Start Break
- **POST** `/break/start`
- **Description**: Start a break
- **Authorization**: Employee
- **Request Body**:
```json
{
  "type": "Lunch|Coffee|Regular|Emergency",
  "reason": "string"
}
```

### End Break
- **POST** `/break/end`
- **Description**: End current break
- **Authorization**: Employee

### Get Break History
- **GET** `/break/history?startDate={date}&endDate={date}`
- **Description**: Get break history for date range
- **Authorization**: Employee

### Get Today's Breaks
- **GET** `/break/today`
- **Description**: Get today's break data
- **Authorization**: Employee

## 6. Task Query Endpoints

### Get All Task Queries
- **GET** `/taskquery?taskId={taskId}&raisedById={userId}`
- **Description**: Get task queries (defaults to current user's queries)
- **Authorization**: All authenticated users

### Get Superior Task Queries
- **GET** `/taskquery/superior?status={status}&priority={priority}`
- **Description**: Get task queries for superior's team
- **Authorization**: Superior, Admin

### Get Task Query by ID
- **GET** `/taskquery/{id}`
- **Description**: Get specific task query details
- **Authorization**: All authenticated users

### Create Task Query
- **POST** `/taskquery`
- **Description**: Create new task query
- **Authorization**: Employee
- **Request Body**:
```json
{
  "taskId": "string",
  "subject": "string",
  "description": "string",
  "priority": "Low|Medium|High|Critical",
  "attachments": ["string"]
}
```

### Update Task Query
- **PUT** `/taskquery/{id}`
- **Description**: Update task query (Superior/Admin only)
- **Authorization**: Superior, Admin
- **Request Body**:
```json
{
  "status": "Open|InProgress|Resolved|Closed",
  "assignedToId": "string",
  "priority": "Low|Medium|High|Critical"
}
```

### Resolve Task Query
- **POST** `/taskquery/{id}/resolve`
- **Description**: Resolve task query (Superior/Admin only)
- **Authorization**: Superior, Admin
- **Request Body**:
```json
{
  "resolution": "string"
}
```

### Get Query Stats
- **GET** `/taskquery/stats`
- **Description**: Get query statistics
- **Authorization**: All authenticated users

## 7. Report Endpoints

### Get Attendance Report
- **GET** `/report/attendance?startDate={date}&endDate={date}`
- **Description**: Get attendance report for date range
- **Authorization**: All authenticated users

### Get Task Report
- **GET** `/report/task?startDate={date}&endDate={date}`
- **Description**: Get task completion report for date range
- **Authorization**: All authenticated users

### Get Time Tracking Report
- **GET** `/report/timetracking?startDate={date}&endDate={date}`
- **Description**: Get detailed time tracking report
- **Authorization**: All authenticated users

### Get Employee Report (Superior)
- **GET** `/report/superior/employee/{employeeId}?startDate={date}&endDate={date}`
- **Description**: Get specific employee's report
- **Authorization**: Superior, Admin

### Get Team Report (Superior)
- **GET** `/report/superior/team?startDate={date}&endDate={date}`
- **Description**: Get team overview report
- **Authorization**: Superior, Admin

## Response Format

All endpoints return responses in the following format:

```json
{
  "success": true,
  "message": "string",
  "data": {},
  "errors": []
}
```

## Error Codes

- `400` - Bad Request (validation errors)
- `401` - Unauthorized (authentication required)
- `403` - Forbidden (insufficient permissions)
- `404` - Not Found (resource not found)
- `409` - Conflict (business rule violation)
- `500` - Internal Server Error

## Business Rules

1. **One Active Task**: Only one task can be active per user at any time
2. **8-Hour Rule**: System enforces 8-hour working day compliance
3. **Role-Based Access**: 
   - Employees can only access their own data
   - Superiors can access their team's data
   - Admins have full access
4. **Time Tracking**: 
   - Must start time tracking before working on tasks
   - Breaks are automatically tracked
   - System calculates work hours vs break hours
5. **Task Queries**: 
   - Employees can raise queries on assigned tasks
   - Superiors can assign and resolve queries
   - File attachments supported for queries

## Authentication

Use Bearer token authentication:
```
Authorization: Bearer {jwt_token}
```

## Rate Limiting

- 100 requests per minute per user
- 1000 requests per hour per user

## File Upload

For task query attachments, use multipart/form-data:
```
Content-Type: multipart/form-data
```

## WebSocket Endpoints (Real-time Features)

### Time Tracking Updates
- **WS** `/ws/timetracking/{userId}`
- **Description**: Real-time time tracking updates

### Task Query Notifications
- **WS** `/ws/notifications/{userId}`
- **Description**: Real-time task query notifications

### Dashboard Updates
- **WS** `/ws/dashboard/{userId}`
- **Description**: Real-time dashboard updates 