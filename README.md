# Wscad.VectorGraphicViewer

A **.NET 8 + WPF** application developed as a technical challenge to render graphic primitives (lines, circles, and triangles) from different **pluggable data sources** (JSON, XML, and API).  

---

## 🚀 Technologies
- **.NET 8** (WPF Application)
- **Native Dependency Injection (Microsoft.Extensions.DependencyInjection)**
- **IOptions** for environment-based configuration
- **Newtonsoft.Json** (flexible JSON serialization)
- **System.Xml.Serialization** (XML serialization)
- Architecture based on **DDD (Domain-Driven Design)**

---

## 📂 Project Structure
- **Application** → Orchestration and configuration (DI, high-level services)  
- **Domain** → Entities, ValueObjects, Enums, Extensions, and Business Rules  
- **Infrastructure** → DataProviders (JSON, XML, API), DTOs, and Mappers  
- **WpfApp** → UI with WPF (primitive rendering)

---

## 🏗️ Layered Architecture
- **Domain**:  
  - `Primitive` entity represents lines, circles, and triangles.  
  - ValueObjects (`PointD`, `Rgba`) and Extensions (`PointExtensions`, `RgbaExtensions`).  
  - Domain services (`GeometryService`).  

- **Infrastructure**:  
  - **DataProviders (pluggable):**  
    - `PrimitivesJsonSource`  
    - `PrimitivesXmlSource`  
    - `PrimitivesApiSource`  
  - DTOs and Mappers (e.g., `PrimitiveJsonDto` → `Primitive`)  
  - Repository with cache (`PrimitiveRepository`)  

- **Application**:  
  - `PrimitiveService` (data orchestration)  
  - `App.xaml.cs` → DI configuration + Options binding from `appSettings`.  

- **WpfApp**:  
  - `MainWindow` (UI)  
  - Next step: render primitives in a canvas.

---

## ⚙️ Configuration
Configuration is centralized in **`appSettings.{env}.json`** (per environment):

```json
{
  "DataSourceConfig": {
    "PrimitiveDataSourceType": 1, // 1=Json, 2=Xml, 3=Api
    "Json": {
      "PrimitivesPath": "Workloads/primitives.json",
      "AllowComments": false,
      "CaseInsensitive": true
    },
    "Xml": {
      "PrimitivesPath": "Workloads/primitives.xml",
      "RootElement": "primitives",
      "Namespace": null,
      "ValidateSchema": false,
      "SchemaPath": null
    },
    "Api": {
      "BaseUrl": "https://www.wscad.com/",
      "PrimitivesEndpoint": "/api/primitives",
      "ApiKey": "",
      "TimeoutSeconds": 20,
      "MaxRetries": 2
    }
  }
}
