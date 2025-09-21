# 🏗️ Architecture Overview

This project follows a **Ports and Adapters (Hexagonal) Architecture**, ensuring a clean separation between the **Domain (core business logic)** and the **Infrastructure (data access, configuration, and external concerns)**.  

This approach allows the system to easily support **pluggable data sources** (e.g., JSON, XML, API) without impacting the business rules.

📖 *For a quick project summary, see the [README.md](./README.md).*

---

## 🗂️ Project Structure

Wscad.VectorGraphicViewer
├── Wscad.VectorGraphicViewer.Application   → Orchestration / Services that coordinate use cases
│   └── Orchestration
│       ├── Interfaces
│       │   └── IPrimitiveService.cs
│       └── PrimitiveService.cs
│
├── Wscad.VectorGraphicViewer.Domain        → Core business logic (agnostic to data sources)
│   ├── Contracts
│   │   ├── Repositories
│   │   │   ├── IPrimitiveRepository.cs
│   │   │   └── IPrimitivesDataSource.cs
│   │   └── Services
│   │       └── IGeometryService.cs
│   ├── Entities
│   │   └── Primitive.cs
│   ├── Enums
│   │   ├── PrimitiveDataSourceTypeEnum.cs
│   │   └── PrimitiveTypeEnum.cs
│   ├── Extensions
│   │   ├── PointExtensions.cs
│   │   ├── PrimitiveTypeExtensions.cs
│   │   └── RgbaExtensions.cs
│   ├── Services
│   │   └── GeometryService.cs
│   └── ValueObjects
│       ├── PointD.cs
│       └── Rgba.cs
│
├── Wscad.VectorGraphicViewer.Infrastructure → Adapters to external systems (DataSources, DTOs, etc.)
│   ├── DataProviders
│   │   ├── Contracts
│   │   │   └── IPrimitivesDataSource.cs
│   │   ├── DTOs
│   │   │   ├── PrimitiveJsonDto.cs
│   │   │   └── PrimitiveXmlDto.cs
│   │   ├── Mappers
│   │   │   ├── PrimitiveJsonMapper.cs
│   │   │   └── PrimitiveXmlMapper.cs
│   │   ├── Options
│   │   │   ├── PrimitivesApiOptions.cs
│   │   │   ├── PrimitivesJsonOptions.cs
│   │   │   └── PrimitivesXmlOptions.cs
│   │   └── Sources
│   │       ├── PrimitivesApiSource.cs
│   │       ├── PrimitivesJsonSource.cs
│   │       └── PrimitivesXmlSource.cs
│   └── Repository
│       └── PrimitiveRepository.cs
│
├── Wscad.VectorGraphicViewer.WpfApp        → Presentation Layer (UI with WPF)
│   ├── App.xaml / App.xaml.cs
│   ├── appSettings.{Development|Production|Staging}.json
│   ├── AssemblyInfo.cs
│   └── MainWindow.xaml
│
└── Workloads                               → Example input files
├── primitives.csv
├── primitives.json
└── primitives.xml

---

## ⚙️ Flow of Responsibilities

1. **Application Layer**  
   - `PrimitiveService` orchestrates data loading and delegates calculations to the domain.  
   - Exposes **use case-oriented interfaces** (e.g., `IPrimitiveService`).

2. **Domain Layer**  
   - Holds the **business entities** (`Primitive`, `PointD`, `Rgba`) and **rules** (`GeometryService`).  
   - Defines contracts (`IPrimitiveRepository`, `IPrimitivesDataSource`) that are later implemented by the Infrastructure.  
   - Totally agnostic of external technologies (JSON, XML, APIs, databases).

3. **Infrastructure Layer**  
   - Provides **DataSources** (JSON, XML, API), each configurable via `appSettings.*.json`.  
   - Uses **DTOs + Mappers** to translate external formats into domain entities.  
   - `PrimitiveRepository` coordinates between `IPrimitivesDataSource` and the Domain.  
   - Clear demonstration of **Ports (interfaces in Domain)** and **Adapters (implementations in Infrastructure)**.

4. **Presentation Layer (WPF)**  
   - Entry point of the application.  
   - Configures Dependency Injection.  
   - Reads configuration (`appSettings`) to decide which DataSource to bind at runtime.  
   - Displays primitives on screen.  

---

---

## 🔑 Design Decisions

1. **Ports and Adapters (Hexagonal)**  
   - *Ports* → Defined in **Domain** as interfaces (`IPrimitivesDataSource`, `IPrimitiveRepository`).  
   - *Adapters* → Implemented in **Infrastructure** (e.g., `PrimitivesJsonSource`, `PrimitivesXmlSource`, `PrimitivesApiSource`).  
   - This makes the **domain independent from technical details**.

2. **DTOs and Mappers**  
   - Each data source (JSON, XML, API) has its **own DTOs** that reflect the raw format.  
   - Mappers convert DTOs → **Domain Entities**.  
   - Ensures the **Domain remains agnostic** of serialization details.

3. **Repository with In-Memory Cache**  
   - `PrimitiveRepository` centralizes access to primitives.  
   - Shared cache avoids repeated parsing of files or multiple API calls.  

4. **Value Objects & Extensions**  
   - `PointD` and `Rgba` are immutable **Value Objects**.  
   - Extension methods provide safe parsing (`TryParse`) and domain logic helpers.

---

## 📊 Diagram

```text
                ┌─────────────────────────┐
                │   WPF Application (UI)  │
                │  - MainWindow.xaml      │
                │  - App.xaml.cs          │
                └───────────▲─────────────┘
                            │
                            │ Calls services
                            │
                ┌───────────┴─────────────┐
                │   Application Layer     │
                │   (Use Case Orchestration)
                │                         │
                │  - PrimitiveService     │
                │  - IPrimitiveService    │
                └───────────▲─────────────┘
                            │
                            │ Ports (interfaces)
                            │
        ┌───────────────────┴─────────────────────┐
        │                 Domain                  │
        │  (Business Core – agnostic of Infra)    │
        │                                         │
        │  Entities: Primitive, PointD, Rgba      │
        │  Services: GeometryService              │
        │  Contracts:                             │
        │    - IPrimitiveRepository               │
        │    - IPrimitivesDataSource              │
        └───────────▲─────────────────────────────┘
                    │
                    │ Implementations (Adapters)
                    │
    ┌───────────────┴──────────────────────────────┐
    │               Infrastructure                 │
    │   (Adapters for external data sources)       │
    │                                              │
    │  DataProviders:                              │
    │    - PrimitivesJsonSource  (JSON file)       │
    │    - PrimitivesXmlSource   (XML file)        │
    │    - PrimitivesApiSource   (REST API)        │
    │                                              │
    │  DTOs + Mappers:                             │
    │    - PrimitiveJsonDto → Primitive            │
    │    - PrimitiveXmlDto  → Primitive            │
    │                                              │
    │  Repository:                                 │
    │    - PrimitiveRepository (shared cache)      │
    └──────────────────────────────────────────────┘