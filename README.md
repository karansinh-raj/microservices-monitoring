# .NET8 Microservices with OpenTelemetry
Welcome to the Microservices Demo project! This project demonstrates how to set up and monitor microservices using OpenTelemetry, export and visualize telemetry data to Aspire Dashboard. 

## Overview
This demo project showcases the implementation of microservices with comprehensive logging, tracing, and metrics collection using OpenTelemetry. 
The services communicate asynchronously through RabbitMQ and synchronously via HTTP REST APIs. 
Monitoring is facilitated through Aspire Dashboard.

## Features
- 📝 Newsletter API: Create and retrieve articles.
- 📊 Newsletter Reporting API: Track and report article events.
- 🐇 RabbitMQ: Asynchronous communication between services.
- 🌐 HTTP REST API: Synchronous communication between services.
- 📈 OpenTelemetry: Instrumentation for logging, tracing, and metrics.
- 🐳 Docker: Containerization and orchestration.
  
## Technologies Used
- .NET 8
- OpenTelemetry
- MassTransit
- RabbitMQ
- PostgreSQL
- Docker
- Aspire Dashboard
  
## Endpoints
Newsletter API
- POST /api/articles - Create a new article
- GET /api/articles - Retrieve all articles
- GET /api/articles/{id} - Retrieve an article by ID
- GET /api/articles/{id}/events - Retrieve events for an article
  
Newsletter Reporting API
- GET /api/articles/{id} - Retrieve events for an article
  
## Getting Started
Follow these steps to run the project:

1. Clone the repository:
```
git clone https://github.com/karansinh-raj/microservices-monitoring.git
cd microservices-monitoring
```

2. Run Docker Compose:
```
docker-compose up
```

Access Swagger for APIs:

- Newsletter API: https://localhost:5101/swagger
- Newsletter Reporting API: https://localhost:5201/swagger
- Access Aspire Dashboard: http://localhost:18888/login?t={token}

### Happy coding! 🚀
