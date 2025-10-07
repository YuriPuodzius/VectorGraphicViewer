# Wscad.VectorGraphicViewer

A **.NET 8 + WPF** application developed as a technical challenge to render graphic primitives (lines, circles, and triangles) from different **pluggable data sources** (JSON, XML, and API), with flexibility to easily extend support for new primitives.  

The project is designed with **SOLID** and **Clean Code** principles and follows the **Ports and Adapters (Hexagonal) and Clean Architecture**, ensuring a clean separation between the core domain and infrastructure. This makes data sources and rendering logic easily replaceable without impacting the business logic, promoting flexibility and maintainability.  

In addition, the implementation leverages classic design patterns such as:  
- **Repository** — centralizes access and caching, isolating data origin.  
- **Strategy** — enables flexible selection of data sources and rendering logic at runtime.  
- **Adapter** — bridges external formats (JSON, XML, API) to the domain contracts.  

## 🧩 Architecture Overview

Below is the visual representation of the project structure following the .NET Clean Architecture principles:

![Clean Architecture](https://github.com/YuriPuodzius/VectorGraphicViewer/blob/main/CleanArch-VectorGraphicViewer.png)

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
- **Environment-based configuration**: automatically switches between environments (`Development`,`Staging` or `Production`) using `DOTNET_ENVIRONMENT`.  
- **In-memory caching** in the repository to simulate efficient retrieval (GetAll vs. GetByType).  
- **WPF UI with MVVM**: clear separation between `View` (MainWindow), `ViewModel` (MainViewModel), and rendering logic.  
- **Extensible rendering pipeline**: `PrimitiveRenderCoordinator` delegates drawing to specialized **Drawer classes**, making it easy to add new primitives without impacting existing code.  
---

## 🏗️ Architecture

- **Domain**  
  Entities (`Primitive`), Enums, and Value Objects (`Rgba`, `Point`).  
  Contains the **business core** and rules (`GeometryService`).  

- **Application**  
  Orchestration services (`PrimitiveService`), configuration binding (`IOptions`), and dependency injection setup.  

- **Infrastructure (Adapters)**  
  Data providers (`PrimitivesJsonSource`, `PrimitivesXmlSource`, `PrimitivesApiSource`) implementing the `IPrimitivesDataSource` port.  
  Also includes repository and mapping logic for deserialization.  

- **WPF UI**  
  Based on **MVVM**, responsible for presentation and user interaction.  
  Rendering is handled by pluggable **Drawer** classes, coordinated by `PrimitiveRenderCoordinator`.  

---

## 🔧 How to Run

1. Set environment variable (default = Development):
   ```powershell
   $env:DOTNET_ENVIRONMENT="Development"
