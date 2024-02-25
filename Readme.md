
# Transactional Outbox Pattern ASP.NET Core Sample

This repository showcases the implementation and utilization of MassTransit's transactional outbox feature.

**Orders API Controller:** Demonstrates how to integrate the transactional outbox with an API controller. The order service leverages IPublishEndpoint to publish a domain event, which is seamlessly written to the transactional outbox.

Adds the transactional outbox delivery service as a hosted service, which delivers outbox message to the transport

**Order Processor Service:** This separate service illustrates the consumption of order events. It autonomously processes the events, showcasing the decoupled nature of event-driven architectures.

## Build and Run the Docker Containers

Open a terminal in the same directory where `compose.yml` file and run:

    docker-compose up --build

This command builds and runs the Docker containers as defined in the `compose.yml` file.

    docker-compose down

This command kills the running container

### Additional Tips:

- **Running 'master' Branch:**
    - It uses; **Azure Service Bus** transport

- **Running 'jaeger-opentelemetry' Branch:**
    - It uses; **Azure Service Bus** transport & OpenTelemetry + Jaeger exporter, however not complete yet

- **Running 'rabbitmq' Branch:**
    - It uses; **RabbitMQ** transport.