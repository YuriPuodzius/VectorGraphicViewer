# 🏗️ Architecture Overview

This project follows a **Ports and Adapters (Hexagonal) Architecture**, ensuring a clean separation between the **Domain (core business logic)**, the **Infrastructure (data access, configuration, and external concerns)**, and the **Presentation Layer (WPF, MVVM)**.  

This approach allows the system to easily support **pluggable data sources** (e.g., JSON, XML, API) and extend new **primitives/rendering strategies** without impacting the business rules.

📖 *For a quick project summary, see the [README.md](./README.md).*

---

## 📁 Project Structure

<details>
<summary>Expand project structure</summary>

```text
Wscad.VectorGraphicViewer.Application
└─ Orchestration
   ├─ Interfaces
   │  └─ IPrimitiveService.cs
   └─ PrimitiveService.cs

Wscad.VectorGraphicViewer.Domain
├─ Contracts
│  ├─ Repositories
│  │  └─ IPrimitiveRepository.cs
│  └─ IPrimitivesDataSource.cs
├─ Services
│  └─ IGeometryService.cs
├─ Entities
│  └─ Primitive.cs
├─ Enums
│  ├─ PrimitiveDataSourceTypeEnum.cs
│  └─ PrimitiveTypeEnum.cs
├─ Extensions
│  ├─ PointExtensions.cs
│  └─ RgbaExtensions.cs
├─ Services
│  └─ GeometryService.cs
└─ ValueObjects
   ├─ PointD.cs
   └─ Rgba.cs

Wscad.VectorGraphicViewer.Infrastructure
└─ DataProviders
   ├─ Contracts
   │  └─ IPrimitivesDataSource.cs
   ├─ DTOs
   │  ├─ PrimitiveJsonDto.cs
   │  └─ PrimitiveXmlDto.cs
   ├─ Mappers
   │  ├─ PrimitiveJsonMapper.cs
   │  └─ PrimitiveXmlMapper.cs
   ├─ Options
   │  ├─ PrimitivesApiOptions.cs
   │  ├─ PrimitivesJsonOptions.cs
   │  └─ PrimitivesXmlOptions.cs
   ├─ Sources
   │  ├─ PrimitivesApiSource.cs
   │  ├─ PrimitivesJsonSource.cs
   │  └─ PrimitivesXmlSource.cs
   ├─ Repository
   │  └─ PrimitiveRepository.cs
   └─ Workloads
      ├─ primitives.csv
      ├─ primitives.json
      └─ primitives.xml

Wscad.VectorGraphicViewer.WpfApp
├─ Properties (AssemblyInfo.cs)
├─ 0 - AppSettings (appSettings.(Development|Staging|Production).json)
├─ 1 - Views (MainWindow.xaml)
├─ 2 - ViewModels (MainViewModel.cs)
├─ 3 - Commands (RelayCommand.cs)
├─ 4 - Rendering
│  ├─ 4.1 Orchestration (PrimitiveRenderCoordinator.cs)
│  ├─ 4.2 Drawing
│  │  ├─ Contracts (IPrimitiveDrawer.cs)
│  │  └─ Drawers
│  │     ├─ CircleDrawer.cs
│  │     ├─ LineDrawer.cs
│  │     └─ TriangleDrawer.cs
│  └─ 4.3 Helpers (DrawingHelpers.cs)
├─ Assets (WSCAD-Background-VectorGraphicViewer.png)
├─ App.xaml
└─ App.xaml.cs
```
</details>

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
   - Built with **MVVM**.  
   - `MainViewModel` binds primitives and commands to the UI (`MainWindow`).  
   - `RelayCommand` connects UI actions to application logic.  
   - `Rendering` folder contains the drawing pipeline:
     - `PrimitiveRenderCoordinator` orchestrates the drawing process.
     - `LineDrawer`, `CircleDrawer`, `TriangleDrawer` implement the rendering of each primitive.  
   - Acts as the **entry point**, configuring DI, loading app settings, and rendering primitives on a WPF `Canvas`.  
   - MVVM mapping in this project:
     - **Model** → Domain layer (`Primitive`, `Rgba`, `PointD`, `GeometryService`, etc.).  
     - **ViewModel** → `MainViewModel`.  
     - **View** → `MainWindow.xaml`.  
     - **Rendering** → UI-specific rendering services (decoupled from domain). 
   
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
   - Shared cache avoids repeated parsing of files or multiple API/DataBase calls.  

4. **Value Objects & Extensions**  
   - `PointD` and `Rgba` are immutable **Value Objects**.  
   - Extension methods provide safe parsing (`TryParse`) and domain logic helpers.  

5. **MVVM for WPF UI**  
   - The presentation layer uses **MVVM** to separate UI concerns from application logic.  
   - Rendering logic is delegated to **Drawer classes**, keeping the ViewModels thin and UI-focused.  

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
