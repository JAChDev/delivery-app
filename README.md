# delivery-app
Web application focused on automate delivery process from a transport company.

This project was built with Angular 17 in frontend and .NET 6.0 in backend, uses rabbitMQ as message broker and SignalR notification server to send information from back to front in execution. For login, you can use any userName and "Hahn" in password, and you can find a SVG graph with all nodes and links between them. In the Accepted Orders module you can find the accepted orders list, you can click them to highlight nodes and links with the best path for any accepted order.

For start automated simulation you need click in "Start Sim" buttom, "Credtis" will be updated with any accepted order, cost and earnings.

Hahn Cargo Simulation API is required to execute the project, and a rabbitMQ server for Queue communication between Hahn Cargo Simulation API and backend API.



