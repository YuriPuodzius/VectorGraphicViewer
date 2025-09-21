# Wscad.VectorGraphicViewer

A **.NET 8 + WPF** application developed as a technical challenge to render graphic primitives (lines, circles, and triangles) from different **pluggable data sources** (JSON, XML, and API).

The project is designed with **SOLID** principles and follows the **Ports and Adapters (Hexagonal) Architecture**, ensuring a clean separation between the core domain and infrastructure. This makes data sources easily replaceable without impacting the business logic, promoting flexibility and maintainability.

📖 For a deeper dive into the design, see [ARCHITECTURE.md](ARCHITECTURE.md).

---

## 🚀 Features

- **Primitives domain model** (shapes like circle, triangle, etc.) enriched with **Value Objects** (`Rgba` for color, `Point` for coordinates).  
- **Configurable data sources**:
  - JSON (default workload file)
  - XML
  - API (illustrative, ready for extension)
- **Repository + Service pattern**:
  - `PrimitiveRepository` handles caching and delegates to the selected data source.
  - `PrimitiveService` orchestrates access to the repository and prepares results for the UI.
  - `GeometryService` centralizes business rules (e.g., geometric calculations).
- **Ports and Adapters**: data source interfaces (`IPrimitivesDataSource`) act as **ports**, while concrete implementations (JSON, XML, API) are **adapters**.  
- **Environment-based configuration**: automatically switches between environments (`Development`, `Production`) using `DOTNET_ENVIRONMENT`.  
- **In-memory caching** in the repository to simulate efficient retrieval (GetAll vs. GetByType).  

---

## 🏗️ Architecture

- **Domain**  
  Entities (`Primitive`), Enums, and Value Objects (`Rgba`, `Point`).  
  Contains the **business core** and rules (GeometryService).  

- **Application**  
  Orchestration services (`PrimitiveService`), configuration binding (`IOptions`), and dependency injection setup.  

- **Infrastructure (Adapters)**  
  Data providers (`PrimitivesJsonSource`, `PrimitivesXmlSource`, `PrimitivesApiSource`) implementing the `IPrimitivesDataSource` port.  
  Also includes repository and mapping logic for deserialization.  

- **WPF UI**  
  Entry point (`App.xaml.cs`) based on MVVM, consuming services via DI.  
  Responsible only for user interaction and presentation.  

---

## 🔧 How to Run

1. Set environment variable (default = Development):
   ```powershell
   $env:DOTNET_ENVIRONMENT="Development"
