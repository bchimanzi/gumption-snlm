# Withdrawal Application

## Description
The purpose of this solution is to illustrate CQRS and Event Sourcing.  The application uses Kafka as the message bus and MongoDB as eventstore and Postgres as the database.

The application has two microservices:
1. Bank.Command - for handling withdrawal requests and producing a message to message bus.
2. Bank.Query - For querying the Withdrawal records
   Background Service -  consuming the event messages, perform balance checks, updating the account balance & saving the withdrawal records.
                      -  The servive can then produce a status notificatin message to the message bus which can be consumed by another service.

## Tech Stack
- C#
- .Net Core
- Docker
- Kafka `(besides its own advantages, used it for ease of local testing as compared to SNS)`
- MongoDB
- Postgres

## Design Pattern
- CQRS
- Event Sourcing


## Notes
- The application is not production ready, it is just a demonstration of CQRS and Event Sourcing.
- The following aspects are not included:
- Authentication
- Health Checks
- LeaderElection
- Circuit Breaker
