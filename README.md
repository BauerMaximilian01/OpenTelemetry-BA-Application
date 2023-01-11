# OpenTelemetry-BA-Application
Short ASP.NETCore application with an Angular Frontend to show the Functionality of OpenTelemetry with Jaeger for Traces, Prometheus for Metrics and Grafana for visualization of Metrics.

## Getting Started
Clone 
```
git clone https://github.com/BauerMaximilian01/OpenTelemetry-BA-Application.git
```

or Download Repo.

- Start all Docker Containers with ```docker compose up -d``` inside the directory /Docker.
  - ports of Docker Containers:
    - Jaeger: 16686
    - Grafana: 3000
    - Prometheus: 9090
- Start the Services /OpenTelemetryDemo/InventoryService and /OpenTelemetryDemo/OderService
- Start /Angular/OpenTelemetryFrontend with ```ng serve```. The site will be reachable under http://localhost:4200.

## TroubleShooting
If Grafana won't show any data or Prometheus does not gather data from the services endpoints try changing the /Docker/prometheus.yml configuration from
```
- targets: ["host.docker.internal:<port>"]
```
to 
```
- targets: ["localhost:<port>"]
```
