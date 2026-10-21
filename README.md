# Library System Web API (Session 01 & Session 02 Architecture)

This project applies the exact enterprise architecture and patterns from the instructor's repository (`WebApplication1` - Session 01 & Session 02) to the **Library System** (Project 2).

---

## 1. Architectural Highlights Applied from Instructor's Repository:
1. **BaseEntity & Auditing Structure:**
   - `Id` (Primary key)
   - `IsDeleted` (Soft delete flag)
   - `CreatedAt`, `CreatedBy`, `ModifiedAt`, `ModifiedBy` (Audit fields)
2. **Global Query Filters & Soft Delete:**
   - Applied via Entity Configurations (`HasQueryFilter(e => !e.IsDeleted)`).
   - Delete endpoints perform soft delete (`IsDeleted = true`).
3. **Fluent API Configurations:**
   - Modular `IEntityTypeConfiguration<T>` in `Data/Configurations/`.
   - `modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())` in `ApplicationDbContext`.
4. **AutoMapper Profiles:**
   - Centralized `LibraryProfile` inheriting from `Profile`.
   - Mapping entities to DTO responses (`GetAuthorResponse`, `GetBookResponse`, `GetBorrowerResponse`, `GetLoanResponse`).
5. **Dependency Injection & Clean Extensions:**
   - Service registration encapsulated in `ApplicationServiceExtension.AddServices()`.
6. **Automatic Migration & Seeding Pipeline:**
   - Scope-based automatic database migration and JSON seeding on startup (`LibraryContextSeeding.SeedAsync`).
7. **Clean Asynchronous CRUD Controllers:**
   - Full CRUD actions (`GetAll*`, `GetById`, `Create`, `Update`, `Remove`).
   - `AsNoTracking()` on read queries for optimal performance.

---

## 2. Solution Structure:
```
LibrarySystemApi/
├── Controllers/
│   ├── AuthorsController.cs
│   ├── BooksController.cs
│   ├── BorrowersController.cs
│   └── LoansController.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/
│   │   ├── AuthorConfigurations.cs
│   │   ├── BookConfigurations.cs
│   │   ├── BorrowerConfigurations.cs
│   │   └── LoanConfigurations.cs
│   └── Seeding/
│       ├── Authors.json
│       └── LibraryContextSeeding.cs
├── Dtos/
│   ├── AuthorDtos/
│   ├── BookDtos/
│   ├── BorrowerDtos/
│   └── LoanDtos/
├── Entities/
│   ├── BaseEntity.cs
│   ├── Author.cs
│   ├── Book.cs
│   ├── Borrower.cs
│   └── Loan.cs
├── Extensions/
│   └── ApplicationServiceExtension.cs
├── Helper/
│   └── LibraryProfile.cs
├── Program.cs
└── appsettings.json
```
