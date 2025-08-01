# PropVivo Task and Attendance Management System - System Overview

## 🎯 System Purpose

The PropVivo Task and Attendance Management System is a comprehensive backend API designed to support Android applications for employee task management, time tracking, and attendance monitoring. The system ensures 8-hour workday compliance and provides role-based access control for employees and superiors.

## 🏗️ Architecture Overview

### Technology Stack
- **Framework**: ASP.NET Core 8.0
- **Database**: Azure Cosmos DB (NoSQL)
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: Swagger/OpenAPI
- **Architecture Pattern**: Clean Architecture with CQRS (Command Query Responsibility Segregation)

### Project Structure
```
PropVivo/
├── PropVivo.API/           # Web API Controllers & Middleware
├── PropVivo.Application/    # Business Logic & DTOs
├── PropVivo.Domain/         # Entities & Enums
├── PropVivo.Infrastructure/ # Data Access & External Services
└── PropVivo.AzureStorage/  # File Storage Services
```

## 🔐 Core Features

### 1. User Authentication & Authorization
- **JWT-based authentication** with role-based access control
- **Three user roles**:
  - **Employee**: Can manage own tasks, track time, raise queries
  - **Superior**: Can assign tasks, view team reports, resolve queries
  - **Admin**: Full system access and user management

### 2. Task Management
- **Task Assignment**: Superiors can assign tasks to employees
- **Self-Added Tasks**: Employees can create tasks for themselves
- **Task States**: Assigned → InProgress → Paused → Completed
- **Priority Levels**: Low, Medium, High, Critical
- **Constraint**: Only one active task per user at any time

### 3. Time Tracking System
- **Real-time Tracking**: Start, pause, resume, and stop time tracking
- **8-Hour Compliance**: Automatic calculation of work hours vs break hours
- **Break Management**: Track lunch breaks, coffee breaks, and other breaks
- **Daily Logs**: Maintain detailed logs of work hours and breaks
- **Compliance Status**: Indicates if 8-hour work rule is fulfilled

### 4. Task Query System
- **Query Creation**: Employees can raise queries on assigned tasks
- **File Attachments**: Support for screenshots and documents
- **Query States**: Open → InProgress → Resolved → Closed
- **Superior Management**: Superiors can assign, update, and resolve queries
- **Real-time Notifications**: Instant notifications for new queries

### 5. Dashboard & Reporting
- **Employee Dashboard**: View assigned tasks, active timer, daily hours
- **Superior Dashboard**: Team overview, task assignments, query management
- **Attendance Reports**: Daily, weekly, and monthly attendance tracking
- **Task Reports**: Completion rates, time spent, productivity metrics
- **Time Tracking Reports**: Detailed breakdown of work hours and breaks

## 📊 Data Models

### Core Entities

#### User
```csharp
public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserRole Role { get; set; }
    public string? SuperiorId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
```

#### Task
```csharp
public class Task : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string AssignedToId { get; set; }
    public string AssignedById { get; set; }
    public decimal EstimatedHours { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsSelfAdded { get; set; }
}
```

#### TimeTracking
```csharp
public class TimeTracking : BaseEntity
{
    public string UserId { get; set; }
    public string TaskId { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeTrackingStatus Status { get; set; }
    public decimal TotalHours { get; set; }
    public decimal BreakHours { get; set; }
    public decimal WorkHours { get; set; }
    public bool IsEightHourCompliant { get; set; }
}
```

#### TaskQuery
```csharp
public class TaskQuery : BaseEntity
{
    public string TaskId { get; set; }
    public string RaisedById { get; set; }
    public string? AssignedToId { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public QueryStatus Status { get; set; }
    public QueryPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? Resolution { get; set; }
    public string? ResolvedById { get; set; }
    public List<string> Attachments { get; set; }
}
```

## 🔄 Business Rules & Constraints

### 1. Task Management Rules
- **Single Active Task**: Only one task can be active per user at any time
- **Task Ownership**: Users can only modify tasks they own or are assigned to
- **Self-Added Tasks**: Employees can create tasks for themselves
- **Task Completion**: Tasks must be marked as completed before starting new ones

### 2. Time Tracking Rules
- **8-Hour Compliance**: System enforces 8-hour working day requirement
- **Break Tracking**: Automatic tracking of breaks during work hours
- **Work Hours Calculation**: Total hours = Work hours + Break hours
- **Compliance Status**: Automatic calculation of 8-hour rule fulfillment

### 3. Authorization Rules
- **Employee Access**: Can only access own data and assigned tasks
- **Superior Access**: Can access team member data and manage assignments
- **Admin Access**: Full system access and user management
- **Data Isolation**: Users cannot access data outside their scope

### 4. Query Management Rules
- **Query Creation**: Employees can only create queries for assigned tasks
- **Query Assignment**: Superiors can assign queries to team members
- **Query Resolution**: Only superiors and admins can resolve queries
- **File Attachments**: Support for multiple file types in queries

## 🚀 API Features

### Authentication & Security
- **JWT Token Authentication**: Secure token-based authentication
- **Role-Based Authorization**: Granular permission control
- **Password Hashing**: Secure password storage
- **Token Expiration**: Configurable token lifetime

### Real-time Features
- **WebSocket Support**: Real-time updates for time tracking
- **Push Notifications**: Instant notifications for task queries
- **Live Dashboard**: Real-time dashboard updates

### File Management
- **Azure Blob Storage**: Secure file storage for attachments
- **File Upload**: Support for multiple file types
- **File Validation**: Size and type restrictions

### Reporting & Analytics
- **Attendance Reports**: Detailed attendance tracking
- **Task Analytics**: Task completion and productivity metrics
- **Time Analytics**: Work hour analysis and compliance reports
- **Team Reports**: Superior dashboard with team overview

## 🔧 Technical Implementation

### CQRS Pattern
- **Commands**: Handle write operations (Create, Update, Delete)
- **Queries**: Handle read operations (Get, List, Search)
- **MediatR**: Implements CQRS with mediator pattern

### Repository Pattern
- **Generic Repository**: Common CRUD operations
- **Specialized Repositories**: Domain-specific operations
- **Cosmos DB Integration**: NoSQL database with document storage

### Validation & Error Handling
- **FluentValidation**: Request validation
- **Global Exception Handling**: Centralized error management
- **Custom Exceptions**: Domain-specific error types
- **Error Responses**: Consistent error response format

### Performance & Scalability
- **Async/Await**: Non-blocking operations
- **Caching**: Redis integration for performance
- **Pagination**: Large dataset handling
- **Rate Limiting**: API usage control

## 📱 Android Integration

### API Integration Points
- **Authentication**: Login/logout with JWT tokens
- **Dashboard**: Real-time dashboard data
- **Task Management**: CRUD operations for tasks
- **Time Tracking**: Start/stop/pause/resume functionality
- **Query System**: Create and manage task queries
- **Reporting**: Generate and view reports

### Real-time Features
- **WebSocket Connection**: Live updates for time tracking
- **Push Notifications**: Task query notifications
- **Background Sync**: Offline capability with sync

### Security Considerations
- **Token Storage**: Secure token storage on device
- **Data Encryption**: Encrypted data transmission
- **Certificate Pinning**: SSL certificate validation
- **Biometric Authentication**: Optional biometric login

## 🎯 Key Benefits

### For Employees
- **Easy Task Management**: Simple interface for task tracking
- **Time Compliance**: Automatic 8-hour workday tracking
- **Query System**: Easy communication with superiors
- **Self-Added Tasks**: Flexibility to add personal tasks

### For Superiors
- **Team Overview**: Complete visibility of team activities
- **Task Assignment**: Easy task distribution
- **Query Management**: Efficient query resolution
- **Reporting**: Detailed analytics and reports

### For Organizations
- **Compliance**: Automatic 8-hour workday enforcement
- **Productivity**: Track and improve team productivity
- **Communication**: Streamlined employee-superior communication
- **Analytics**: Data-driven decision making

## 🔮 Future Enhancements

### Planned Features
- **Mobile Push Notifications**: Native push notifications
- **Advanced Analytics**: Machine learning insights
- **Integration APIs**: Third-party system integration
- **Multi-language Support**: Internationalization
- **Advanced Reporting**: Custom report builder

### Scalability Considerations
- **Microservices Architecture**: Service decomposition
- **Event Sourcing**: Audit trail and event history
- **CQRS with Event Store**: Advanced data modeling
- **Kubernetes Deployment**: Container orchestration

This system provides a robust foundation for Android-based task and attendance management, ensuring compliance, productivity, and effective team communication. 