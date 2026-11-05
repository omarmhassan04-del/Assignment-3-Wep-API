# Library System Web API (Session 01, Session 02 & Session 03 Architecture)

This project applies the exact enterprise architecture and design patterns from the instructor's repository (`WebApplication1` - Session 01, Session 02, and Session 03) to the **Library System** (Project 2).

---

## 1. Architectural Highlights Applied from Instructor's Repository (Commit 3 / Session 03):

1. **Result Design Pattern:**
   - `Result`: Encapsulates operation status (`IsSuccess`, `IsFailure`, `Error`).
   - `GenericResult<T>`: Strongly-typed result holding data values (`Value`) alongside outcome status.
   - Standardized API responses across all controllers, eliminating unhandled exceptions and raw data leaking.

2. **Pagination with `PaginatedList<T>`:**
   - Configurable pagination metadata (`PageNumber`, `PageSize`, `TotalRecords`, `TotalPages`).
   - Caps page size to a maximum limit (`_maxPageSize = 20`, default `_pageSize = 10`).
   - Dynamic search capabilities with integrated query filtering.

3. **Repository Pattern:**
   - **Generic Repository (`IGenericRepository<T>`, `GenericRepository<T>`):** Base CRUD operations for all entities inheriting from `BaseEntity`.
   - **Specific Repositories (`IAuthorRepository`, `IBookRepository`, `IBorrowerRepository`, `ILoanRepository`):**
     - Custom eager loading (`Include`, `ThenInclude`).
     - Pagination & keyword search (`GetAllAsync(pageNumber, pageSize, searchTerm)`).
     - Composite primary key management for `Loan` (`[BookId, BorrowerId, LoanDate]`).

4. **Service Layer:**
   - Business logic encapsulation (`IAuthorService`, `IBookService`, `IBorrowerService`, `ILoanService`).
   - Returns `GenericResult<T>` and `GenericResult<PaginatedList<T>>`.
   - Handles entity-to-DTO transformations using AutoMapper (`IMapper`).
   - Enforces referential integrity checks before write operations.

5. **Clean Asynchronous Controllers:**
   - All controllers delegate to the Service layer.
   - Controllers return standardized HTTP responses (`Ok`, `NotFound`, `BadRequest`) based on `GenericResult.IsSuccess`.

6. **Dependency Injection:**
   - Registered in `ApplicationServiceExtension.AddServices()`:
     - `IGenericRepository<>` -> `GenericRepository<>`
     - Specific Repositories: `Scoped`
     - Services: `Scoped`

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
├── DTOs/
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
├── Repositories/
│   ├── IGenericRepository.cs
│   ├── GenericRepository.cs
│   ├── IAuthorRepository.cs
│   ├── AuthorRepository.cs
│   ├── IBookRepository.cs
│   ├── BookRepository.cs
│   ├── IBorrowerRepository.cs
│   ├── BorrowerRepository.cs
│   ├── ILoanRepository.cs
│   └── LoanRepository.cs
├── Results/
│   ├── Result.cs
│   ├── GenericResult.cs
│   └── PaginatedList.cs
├── Services/
│   ├── IAuthorService.cs
│   ├── AuthorService.cs
│   ├── IBookService.cs
│   ├── BookService.cs
│   ├── IBorrowerService.cs
│   ├── BorrowerService.cs
│   ├── ILoanService.cs
│   └── LoanService.cs
├── Program.cs
└── appsettings.json
```
